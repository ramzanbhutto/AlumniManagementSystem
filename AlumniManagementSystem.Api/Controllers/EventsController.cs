using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using AlumniManagementSystem.Shared;
using AlumniManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ControllerBase{
  private readonly EventService _svc;
  private readonly IAlumniRepository _alumniRepo;
  public EventsController(EventService svc, IAlumniRepository alumniRepo){
    _svc= svc; 
    _alumniRepo= alumniRepo; 
  }
 
  [HttpGet]
  public async Task<ActionResult> GetAll(){
    var events = await _svc.GetAllAsync();
    return Ok(events.Select(MapEvent));
  }
 
  [HttpGet("upcoming")]
  public async Task<ActionResult> GetUpcoming(){
    var events = await _svc.GetUpcomingAsync();
    return Ok(events.Select(MapEvent));
  }

  [HttpPost("{id:guid}/rsvp")]
  public async Task<ActionResult> Rsvp(Guid id){
    var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    var alumni = await _alumniRepo.GetByUserIdAsync(userId);
    if(alumni==null) return BadRequest("Alumni profile not found.");
    var existing = await _svc.GetRsvpAsync(id, alumni.AlumniId);
    if(existing != null)  return Conflict(new { message = "You have already RSVPed to this event." });
  
    var ev= await _svc.GetByIdAsync(id);
    if(ev==null) return NotFound();
    if(ev.Capacity.HasValue && (ev.RSVPs?.Count ?? 0) >= ev.Capacity.Value)
         return BadRequest(new { message = "Event is at full capacity." });
    var rsvp = new EventRSVP{
      RsvpId   = Guid.NewGuid(),
      EventId  = id,
      AlumniId = alumni.AlumniId,
    }; 
    await _svc.AddRsvpAsync(rsvp);
    return Ok(new { rsvp.RsvpId, message = "RSVP confirmed!" });
  }
 
  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Create(CreateEventDto dto){
    if(!Enum.TryParse<EventType>(dto.EventType, out var eventType))
      return BadRequest("Invalid event type. Valid: Reunion, Seminar, CareerFair, Social, Workshop");
 
    var ev = new Event
      {
        EventId= Guid.NewGuid(),
        Title= dto.Title,
        Description= dto.Description,
        EventDate= dto.EventDate,
        Venue= dto.Venue,
        Capacity= dto.Capacity,
        EventType= eventType,
        RegistrationDeadline= dto.RegistrationDeadline,
        CreatedBy= Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value),
      };
    await _svc.CreateAsync(ev);
    return Ok(new { ev.EventId });
  }

   [HttpPatch("{id:guid}/cancel")]
   [Authorize(Roles = "Admin")]
   public async Task<ActionResult> Cancel(Guid id){
     var ev = await _svc.GetByIdAsync(id);
     if(ev == null) return NotFound();
     ev.Status = EventStatus.Cancelled;
     await _svc.UpdateAsync(ev);
     return Ok(new { message = "Event cancelled." });
   }

    private static EventDto MapEvent(Event e) => new(){
      EventId= e.EventId,
      Title= e.Title,
      Description= e.Description,
      EventDate= e.EventDate,
      Venue= e.Venue,
      Capacity= e.Capacity,
      RegistrationDeadline= e.RegistrationDeadline,
      EventType= e.EventType.ToString(),
      Status= ComputeStatus(e).ToString(),
      RsvpCount= e.RSVPs?.Count ?? 0,
      AttendingCount= e.RSVPs?.Count(r => r.Status == RsvpStatus.Attending) ?? 0,
      CheckedInCount= e.RSVPs?.Count(r => r.CheckedIn) ?? 0,
    };

    private static EventStatus ComputeStatus(Event e){
      if(e.Status==EventStatus.Cancelled) return EventStatus.Cancelled;
      var now = DateTime.UtcNow;
      if(e.EventDate.Date>now.Date) return EventStatus.Upcoming;
      if(e.EventDate.Date==now.Date) return EventStatus.Ongoing;
      return EventStatus.Completed;
    }

}
