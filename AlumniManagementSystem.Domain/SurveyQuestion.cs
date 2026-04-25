using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class SurveyQuestion
{
    public Guid QuestionId { get; set; }
    public Guid SurveyId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public int OrderIndex { get; set; } = 0;

    public Survey Survey { get; set; } = null!;
    public ICollection<SurveyAnswer> Answers { get; set; } = [];
}
