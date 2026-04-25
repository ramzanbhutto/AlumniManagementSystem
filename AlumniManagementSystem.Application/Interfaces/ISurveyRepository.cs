using AlumniManagementSystem.Domain;
 
namespace AlumniManagementSystem.Application.Interfaces;
 
public interface ISurveyRepository{
  Task<IEnumerable<Survey>> GetAllActiveAsync();
  Task<Survey?> GetByIdWithQuestionsAsync(Guid surveyId);
  Task AddAsync(Survey survey);
  Task AddResponseAsync(SurveyResponse response);
  Task SaveChangesAsync();
}
