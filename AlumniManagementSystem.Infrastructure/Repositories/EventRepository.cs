using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
 
public class EventRepository : IEventRepository{
  private readonly ApplicationDbContext _ctx;
  public EventRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Event>> GetAllAsync() => 
    await _ctx.Events
        .Include(e => e.Creator)
        .Include(e => e.RSVPs)
        .ToListAsync();

  public async Task<Event?> GetByIdAsync(Guid eventId)
      => await _ctx.Events
          .Include(e => e.RSVPs).ThenInclude(r => r.Alumni)
          .FirstOrDefaultAsync(e => e.EventId == eventId);
 
  public async Task<IEnumerable<Event>> GetUpcomingAsync()
      => await _ctx.Events
          .Where(e => e.Status == EventStatus.Upcoming && e.EventDate > DateTime.UtcNow)
          .OrderBy(e => e.EventDate)
          .ToListAsync();
 
  public async Task AddAsync(Event ev) => await _ctx.Events.AddAsync(ev);
  public void Update(Event ev) => _ctx.Events.Update(ev);
  public void Delete(Event ev) => _ctx.Events.Remove(ev);
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();

  public async Task<EventRSVP?> GetRsvpAsync(Guid eventId, Guid alumniId)  => await _ctx.EventRSVPs.FirstOrDefaultAsync(r => r.EventId == eventId && r.AlumniId == alumniId);
  public async Task AddRsvpAsync(EventRSVP rsvp) => await _ctx.EventRSVPs.AddAsync(rsvp);

}
