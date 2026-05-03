using AlumniManagementSystem.Domain;

namespace AlumniManagementSystem.Application.Interfaces;

public interface ISurveyRepository{
  Task<IEnumerable<Survey>> GetAllActiveAsync();
  Task<IEnumerable<Survey>> GetAllIncludingClosedAsync();
  Task<Survey?> GetByIdWithQuestionsAsync(Guid surveyId);
  Task AddAsync(Survey survey);
  Task AddResponseAsync(SurveyResponse response);
  void Delete(Survey survey);
  Task<IEnumerable<SurveyResponse>> GetResponsesAsync(Guid surveyId);
  Task SaveChangesAsync();
}
