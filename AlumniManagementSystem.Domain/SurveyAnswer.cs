namespace AlumniManagementSystem.Domain;

public class SurveyAnswer
{
    public Guid AnswerId { get; set; }
    public Guid ResponseId { get; set; }
    public Guid QuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;

    public SurveyResponse Response { get; set; } = null!;
    public SurveyQuestion Question { get; set; } = null!;
}
