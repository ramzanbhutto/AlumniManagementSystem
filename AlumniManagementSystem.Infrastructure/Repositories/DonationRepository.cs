using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
 
public class DonationRepository : IDonationRepository{
  private readonly ApplicationDbContext _ctx;
  public DonationRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Donation>> GetByAlumniIdAsync(Guid alumniId) => await _ctx.Donations.Where(d => d.AlumniId == alumniId).ToListAsync();
 
  public async Task<decimal> GetTotalByAlumniIdAsync(Guid alumniId) => await _ctx.Donations.Where(d => d.AlumniId == alumniId).SumAsync(d => d.Amount);
 
  public async Task AddAsync(Donation donation) => await _ctx.Donations.AddAsync(donation);
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
