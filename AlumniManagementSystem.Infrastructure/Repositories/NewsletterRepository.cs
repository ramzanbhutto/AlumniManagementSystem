using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
public class NewsletterRepository : INewsletterRepository{
  private readonly ApplicationDbContext _ctx;
  public NewsletterRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Newsletter>> GetAllAsync() => await _ctx.Newsletters.Include(n => n.Creator).ToListAsync();
 
  public async Task<Newsletter?> GetByIdAsync(Guid id) => await _ctx.Newsletters.FindAsync(id);
 
  public async Task AddAsync(Newsletter n) => await _ctx.Newsletters.AddAsync(n);
  
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
