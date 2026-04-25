using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
 
public class JobRepository : IJobRepository{
  private readonly ApplicationDbContext _ctx;
  public JobRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<JobPosting>> GetAllActiveAsync()
      => await _ctx.JobPostings
          .Where(j => j.IsActive)
          .Include(j => j.PostedByAlumni)
          .OrderByDescending(j => j.PostedAt)
          .ToListAsync();
 
  public async Task<JobPosting?> GetByIdAsync(Guid jobId) => await _ctx.JobPostings.FindAsync(jobId);
 
  public async Task AddAsync(JobPosting job) => await _ctx.JobPostings.AddAsync(job);
  public void Update(JobPosting job) => _ctx.JobPostings.Update(job);
  public void Delete(JobPosting job) => _ctx.JobPostings.Remove(job);
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
