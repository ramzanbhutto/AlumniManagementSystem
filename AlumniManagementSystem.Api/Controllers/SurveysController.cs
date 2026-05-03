using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
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
  private readonly IAlumniRepository _alumniRepo;
  public SurveysController(ISurveyRepository repo, IAlumniRepository alumniRepo){
    _repo= repo;
    _alumniRepo= alumniRepo;
  }

  // GET all active (alumni/guest view) 
  [HttpGet]
  public async Task<ActionResult> GetActive(){
    var surveys = await _repo.GetAllActiveAsync();
    return Ok(surveys.Select(MapSurvey));
  }

  // GET all including closed (admin view)
  [HttpGet("all")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> GetAll(){
    var surveys = await _repo.GetAllIncludingClosedAsync();
    return Ok(surveys.Select(MapSurvey));
  }

  // GET single with questions
  [HttpGet("{id:guid}")]
  public async Task<ActionResult> GetById(Guid id){
    var s = await _repo.GetByIdWithQuestionsAsync(id);
    if(s is null) return NotFound();
    return Ok(MapSurvey(s));
  }

  // POST create (admin only)
  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Create([FromBody] CreateSurveyDto dto){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var survey = new Survey{
      SurveyId= Guid.NewGuid(),
      Title= dto.Title,
      Description = dto.Description,
      Deadline= dto.Deadline,
      CreatedBy= userId,
      Status= SurveyStatus.Active,
      Questions = dto.Questions.Select((q, i) => new SurveyQuestion{
        QuestionId= Guid.NewGuid(),
        QuestionText = q.QuestionText,
        QuestionType = Enum.TryParse<QuestionType>(q.QuestionType, out var qt) ? qt : QuestionType.Text,
        OrderIndex   = i,
      }).ToList(),
    };
    await _repo.AddAsync(survey);
    await _repo.SaveChangesAsync();
    return Ok(MapSurvey(survey));
  }

  // PATCH close survey (admin only)
  [HttpPatch("{id:guid}/close")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Close(Guid id){
    var s = await _repo.GetByIdWithQuestionsAsync(id);
    if(s == null) return NotFound();
    s.Status = SurveyStatus.Closed;
    await _repo.SaveChangesAsync();
    return Ok();
  }

  // DELETE survey (admin only)
  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Delete(Guid id){
    var s = await _repo.GetByIdWithQuestionsAsync(id);
    if(s == null) return NotFound();
    _repo.Delete(s);
    await _repo.SaveChangesAsync();
    return Ok();
  }

  // GET responses for a survey (admin only)
  [HttpGet("{id:guid}/responses")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> GetResponses(Guid id){
    var responses = await _repo.GetResponsesAsync(id);
    return Ok(responses.Select(r => new SurveyResponseDto{
      ResponseId= r.ResponseId,
      AlumniName= $"{r.Alumni.FirstName} {r.Alumni.LastName}",
      AlumniRoll= r.Alumni.RollNumber,
      SubmittedAt= r.SubmittedAt,
      Answers = r.Answers.Select(a => new SurveyAnswerResultDto{
        QuestionText = a.Question.QuestionText,
        QuestionType = a.Question.QuestionType.ToString(),
        AnswerText= a.AnswerText,
      }).ToList(),
    }));
  }

  // POST respond
  [HttpPost("{id:guid}/respond")]
  public async Task<ActionResult> Submit(Guid id, SubmitSurveyDto dto){
  var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

  var alumni= await _alumniRepo.GetByUserIdAsync(userId);
  if(alumni==null) return BadRequest("Alumni profile not found.");

  // duplicate check
  var survey= await _repo.GetByIdWithQuestionsAsync(id);
  if(survey==null) return NotFound();
  var alreadyResponded= survey.Responses?.Any(r => r.AlumniId == alumni.AlumniId) ?? false;
  if(alreadyResponded)
    return Conflict(new { message = "You have already submitted this survey." });

  var response = new SurveyResponse{
    ResponseId = Guid.NewGuid(),
    SurveyId= id,
    AlumniId= alumni.AlumniId,
    Answers= dto.Answers.Select(a => new SurveyAnswer{
      AnswerId= Guid.NewGuid(),
      QuestionId = a.QuestionId,
      AnswerText = a.AnswerText,
    }).ToList(),
  };
  await _repo.AddResponseAsync(response);
  await _repo.SaveChangesAsync();
  return Ok(new { response.ResponseId, message = "Survey submitted successfully." });
}

  // helpers
  private static SurveyDto MapSurvey(Survey s) => new(){
    SurveyId= s.SurveyId,
    Title= s.Title,
    Description= s.Description,
    Deadline= s.Deadline,
    Status= s.Status.ToString(),
    ResponseCount= s.Responses?.Count ?? 0,
    Questions= s.Questions?.OrderBy(q => q.OrderIndex).Select(q => new SurveyQuestionDto{
      QuestionId  = q.QuestionId,
      QuestionText = q.QuestionText,
      QuestionType = q.QuestionType.ToString(),
      OrderIndex   = q.OrderIndex,
    }).ToList() ?? [],
  };
}
