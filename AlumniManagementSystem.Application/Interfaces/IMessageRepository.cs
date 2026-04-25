using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IMessageRepository{
  Task<IEnumerable<Message>> GetInboxAsync(Guid userId);
  Task<IEnumerable<Message>> GetSentAsync(Guid userId);
  Task AddAsync(Message message);
  Task SaveChangesAsync();
}
