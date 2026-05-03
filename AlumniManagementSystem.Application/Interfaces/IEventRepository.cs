using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IEventRepository{
  Task<IEnumerable<Event>> GetAllAsync();
  Task<Event?> GetByIdAsync(Guid eventId);
  Task<IEnumerable<Event>> GetUpcomingAsync();
  Task AddAsync(Event ev);
  void Update(Event ev);
  void Delete(Event ev);
  Task SaveChangesAsync();
  Task<EventRSVP?> GetRsvpAsync(Guid eventId, Guid alumniId);
  Task AddRsvpAsync(EventRSVP rsvp);
}
