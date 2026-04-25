namespace AlumniManagementSystem.Shared;
public class SurveyAnswerDto
{
    [Required(ErrorMessage = "Question ID is required")]
    public Guid QuestionId { get; set; }

    [Required(ErrorMessage = "Answer cannot be empty")]
    [StringLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters")]
    public string AnswerText { get; set; } = string.Empty;
}
