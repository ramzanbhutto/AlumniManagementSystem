using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface INewsletterRepository{
  Task<IEnumerable<Newsletter>> GetAllAsync();
  Task<Newsletter?> GetByIdAsync(Guid newsletterId);
  Task AddAsync(Newsletter newsletter);
  Task SaveChangesAsync();
}
