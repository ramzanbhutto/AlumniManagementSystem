namespace AlumniManagementSystem.Shared;
public class SurveyDto{
  public Guid SurveyId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public DateTime? Deadline { get; set; }
  public string Status { get; set; } = string.Empty;
  public int ResponseCount { get; set; }
  public List<SurveyQuestionDto> Questions { get; set; } = [];
}
