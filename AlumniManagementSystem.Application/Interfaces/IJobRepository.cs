using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IJobRepository{
  Task<IEnumerable<JobPosting>> GetAllActiveAsync();
  Task<JobPosting?> GetByIdAsync(Guid jobId);
  Task AddAsync(JobPosting job);
  void Update(JobPosting job);
  void Delete(JobPosting job);
  Task SaveChangesAsync();
}
