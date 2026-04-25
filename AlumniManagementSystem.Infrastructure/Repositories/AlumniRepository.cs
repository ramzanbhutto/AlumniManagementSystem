using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
 
public class AlumniRepository : IAlumniRepository{
  private readonly ApplicationDbContext _ctx;
  public AlumniRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Alumni>> GetAllAsync()
      => await _ctx.Alumnis
          .Include(a => a.Program).ThenInclude(p => p.Department)
          .Include(a => a.User)
          .ToListAsync();
 
  public async Task<Alumni?> GetByIdAsync(Guid alumniId)
      => await _ctx.Alumnis
          .Include(a => a.Program)
          .Include(a => a.User)
          .FirstOrDefaultAsync(a => a.AlumniId == alumniId);
 
  public async Task<Alumni?> GetByRollNumberAsync(string rollNumber) => await _ctx.Alumnis.FirstOrDefaultAsync(a => a.RollNumber == rollNumber);
 
  public async Task<Alumni?> GetByUserIdAsync(Guid userId) => await _ctx.Alumnis.FirstOrDefaultAsync(a => a.UserId == userId);
 
  public async Task AddAsync(Alumni alumni) => await _ctx.Alumnis.AddAsync(alumni);
  public void Update(Alumni alumni) => _ctx.Alumnis.Update(alumni);
  public void Delete(Alumni alumni) => _ctx.Alumnis.Remove(alumni);
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
