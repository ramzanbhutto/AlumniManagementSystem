using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IDonationRepository{
  Task<IEnumerable<Donation>> GetByAlumniIdAsync(Guid alumniId);
  Task<decimal> GetTotalByAlumniIdAsync(Guid alumniId);
  Task AddAsync(Donation donation);
  Task SaveChangesAsync();
}
