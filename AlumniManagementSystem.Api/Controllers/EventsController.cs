using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ControllerBase{
  private readonly EventService _svc;
  public EventsController(EventService svc) => _svc = svc;
 
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

    private static EventDto MapEvent(Event e) => new(){
      EventId= e.EventId,
      Title= e.Title,
      Description= e.Description,
      EventDate= e.EventDate,
      Venue= e.Venue,
      Capacity= e.Capacity,
      RegistrationDeadline= e.RegistrationDeadline,
      EventType= e.EventType.ToString(),
      Status= e.Status.ToString(),
      RsvpCount= e.RSVPs?.Count ?? 0,
      AttendingCount= e.RSVPs?.Count(r => r.Status == RsvpStatus.Attending) ?? 0,
      CheckedInCount= e.RSVPs?.Count(r => r.CheckedIn) ?? 0,
    };

}
