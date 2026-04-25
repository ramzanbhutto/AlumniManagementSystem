namespace AlumniManagementSystem.Shared;
public class SurveyQuestionDto{
  public Guid QuestionId { get; set; }
  public string QuestionText { get; set; } = string.Empty;
  public string QuestionType { get; set; } = string.Empty;
  public int OrderIndex { get; set; }
}
