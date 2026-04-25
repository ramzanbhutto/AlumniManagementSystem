using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Services;
 
public class DonationService{
  private readonly IDonationRepository _repo;
  public DonationService(IDonationRepository repo) => _repo = repo;
 
  public Task<IEnumerable<Donation>> GetByAlumniAsync(Guid alumniId) => _repo.GetByAlumniIdAsync(alumniId);
 
  public async Task<Donation> CreateAsync(Donation donation){
    await _repo.AddAsync(donation);
    await _repo.SaveChangesAsync();
    return donation;
  }
}
