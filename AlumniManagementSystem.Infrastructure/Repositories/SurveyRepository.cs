using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Infrastructure.Repositories;
public class SurveyRepository : ISurveyRepository{
  private readonly ApplicationDbContext _ctx;
  public SurveyRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
  public async Task<IEnumerable<Survey>> GetAllActiveAsync() => await _ctx.Surveys.Where(s => s.Status == SurveyStatus.Active).ToListAsync();
 
  public async Task<Survey?> GetByIdWithQuestionsAsync(Guid id) => await _ctx.Surveys.Include(s => s.Questions.OrderBy(q => q.OrderIndex)).FirstOrDefaultAsync(s => s.SurveyId == id);
 
  public async Task AddAsync(Survey s) => await _ctx.Surveys.AddAsync(s);

  public async Task AddResponseAsync(SurveyResponse r) => await _ctx.SurveyResponses.AddAsync(r);
  
  public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
