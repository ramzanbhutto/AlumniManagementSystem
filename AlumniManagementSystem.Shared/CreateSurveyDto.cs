namespace AlumniManagementSystem.Shared;

public class CreateSurveyDto{
    [Required(ErrorMessage = "Survey title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be 3–200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one question is required")]
    public List<CreateSurveyQuestionDto> Questions { get; set; } = [];
}

public class CreateSurveyQuestionDto{
    [Required(ErrorMessage = "Question text is required")]
    public string QuestionText { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(Text|MCQ|Rating)$", ErrorMessage = "Type must be Text, MCQ, or Rating")]
    public string QuestionType { get; set; } = "Text";

    public int OrderIndex { get; set; }
}
