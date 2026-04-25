using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SurveysController : ControllerBase{
  private readonly ISurveyRepository _repo;
  public SurveysController(ISurveyRepository repo) => _repo = repo;
 
  [HttpGet]
  public async Task<ActionResult> GetActive(){
    var surveys = await _repo.GetAllActiveAsync();
    return Ok(surveys.Select(s => new SurveyDto
      {
        SurveyId = s.SurveyId,
        Title = s.Title,
        Description = s.Description,
        Deadline = s.Deadline,
        Status = s.Status.ToString(),
        ResponseCount = s.Responses?.Count ?? 0,
      }));
  }
 
  [HttpGet("{id:guid}")]
  public async Task<ActionResult> GetById(Guid id){
    var s = await _repo.GetByIdWithQuestionsAsync(id);
    if(s is null) return NotFound();
    return Ok(new SurveyDto
      {
        SurveyId = s.SurveyId,
        Title = s.Title,
        Description = s.Description,
        Deadline = s.Deadline,
        Status = s.Status.ToString(),
        ResponseCount = s.Responses?.Count ?? 0,
        Questions = s.Questions.OrderBy(q => q.OrderIndex).Select(q => new SurveyQuestionDto
          {
            QuestionId = q.QuestionId,
            QuestionText = q.QuestionText,
            QuestionType = q.QuestionType.ToString(),
            OrderIndex = q.OrderIndex,
          }).ToList(),
      });
  }
 
  [HttpPost("{id:guid}/respond")]
  public async Task<ActionResult> Submit(Guid id, SubmitSurveyDto dto){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var response = new SurveyResponse
      {
        ResponseId = Guid.NewGuid(),
        SurveyId = id,
        AlumniId = userId,
        Answers = dto.Answers.Select(a => new SurveyAnswer
          {
            AnswerId = Guid.NewGuid(),
            QuestionId = a.QuestionId,
            AnswerText = a.AnswerText,
          }).ToList(),
      };
    await _repo.AddResponseAsync(response);
    await _repo.SaveChangesAsync();
    return Ok(new { response.ResponseId, message = "Survey submitted successfully." });
  }
}
