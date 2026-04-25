namespace AlumniManagementSystem.Shared;
public class SubmitSurveyDto
{
    [Required(ErrorMessage = "Survey ID is required")]
    public Guid SurveyId { get; set; }

    [Required(ErrorMessage = "Answers are required")]
    [MinLength(1, ErrorMessage = "At least one answer is required")]
    public List<SurveyAnswerDto> Answers { get; set; } = [];
}
