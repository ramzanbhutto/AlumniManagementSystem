using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Services;
 
public class AlumniService{
  private readonly IAlumniRepository _repo;
  public AlumniService(IAlumniRepository repo) => _repo = repo;
 
  public Task<IEnumerable<Alumni>> GetAllAsync() => _repo.GetAllAsync();
  public Task<Alumni?> GetByRollNumberAsync(string rollNumber) => _repo.GetByRollNumberAsync(rollNumber);
  public Task<Alumni?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);
  public Task<Alumni?> GetByUserIdAsync(Guid userId) => _repo.GetByUserIdAsync(userId);
 
  public async Task<Alumni> CreateAsync(Alumni alumni){
    await _repo.AddAsync(alumni);
    await _repo.SaveChangesAsync();
    return alumni;
  }
 
  public async Task UpdateAsync(Alumni alumni){
    _repo.Update(alumni);
    await _repo.SaveChangesAsync();
  }
 
  public async Task DeleteAsync(Alumni alumni){
    _repo.Delete(alumni);
    await _repo.SaveChangesAsync();
  }

}
