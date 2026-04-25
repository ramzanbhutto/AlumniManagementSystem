using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface IAlumniRepository{
  Task<IEnumerable<Alumni>> GetAllAsync();
  Task<Alumni?> GetByIdAsync(Guid alumniId);
  Task<Alumni?> GetByRollNumberAsync(string rollNumber);
  Task<Alumni?> GetByUserIdAsync(Guid userId);
  Task AddAsync(Alumni alumni);
  void Update(Alumni alumni);
  void Delete(Alumni alumni);
  Task SaveChangesAsync();
}
