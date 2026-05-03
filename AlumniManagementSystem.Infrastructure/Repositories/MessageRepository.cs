using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
 
public class MessageRepository : IMessageRepository{
  private readonly ApplicationDbContext _ctx;
  public MessageRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Message>> GetInboxAsync(Guid userId)
      => await _ctx.Messages
          .Where(m => m.ReceiverId == userId && !m.IsDeleted)
          .Include(m => m.Sender)
          .OrderByDescending(m => m.SentAt)
          .ToListAsync();
 
  public async Task<IEnumerable<Message>> GetSentAsync(Guid userId)
      => await _ctx.Messages
          .Where(m => m.SenderId == userId && !m.IsDeleted)
          .Include(m => m.Receiver)
          .OrderByDescending(m => m.SentAt)
          .ToListAsync();
 
  public async Task AddAsync(Message message) => await _ctx.Messages.AddAsync(message);
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
  public async Task<Message?> GetByIdAsync(Guid id) => await _ctx.Messages.FindAsync(id);
}
