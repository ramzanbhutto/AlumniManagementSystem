using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Services;
 
public class EventService{
  private readonly IEventRepository _repo;
  public EventService(IEventRepository repo) => _repo = repo;
 
  public Task<IEnumerable<Event>> GetUpcomingAsync() => _repo.GetUpcomingAsync();
  public Task<IEnumerable<Event>> GetAllAsync() => _repo.GetAllAsync();
  public Task<Event?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);
 
  public async Task<Event> CreateAsync(Event ev){
    await _repo.AddAsync(ev);
    await _repo.SaveChangesAsync();
    return ev;
  }
}
