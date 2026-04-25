using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Services;
 
public class JobService{
  private readonly IJobRepository _repo;
  public JobService(IJobRepository repo) => _repo = repo;
 
  public Task<IEnumerable<JobPosting>> GetAllActiveAsync() => _repo.GetAllActiveAsync();
 
  public async Task<JobPosting> CreateAsync(JobPosting job){
    await _repo.AddAsync(job);
    await _repo.SaveChangesAsync();
    return job;
  }
}
