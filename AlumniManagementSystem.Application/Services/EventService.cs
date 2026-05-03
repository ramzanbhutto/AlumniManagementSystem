using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Services;
 
public class EventService{
  private readonly IEventRepository _repo;
  public EventService(IEventRepository repo) => _repo = repo;
 
  public Task<IEnumerable<Event>> GetUpcomingAsync() => _repo.GetUpcomingAsync();
  public Task<IEnumerable<Event>> GetAllAsync() => _repo.GetAllAsync();
  public Task<Event?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

  public Task<EventRSVP?> GetRsvpAsync(Guid eventId, Guid alumniId) => _repo.GetRsvpAsync(eventId, alumniId);
  public async Task AddRsvpAsync(EventRSVP rsvp){
    await _repo.AddRsvpAsync(rsvp);
    await _repo.SaveChangesAsync();
  }
 
  public async Task<Event> CreateAsync(Event ev){
    await _repo.AddAsync(ev);
    await _repo.SaveChangesAsync();
    return ev;
  }
}
