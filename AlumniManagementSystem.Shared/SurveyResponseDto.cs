namespace AlumniManagementSystem.Shared;

public class SurveyResponseDto {
    public Guid ResponseId { get; set; }
    public string AlumniName { get; set; } = string.Empty;
    public string AlumniRoll { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public List<SurveyAnswerResultDto> Answers { get; set; } = [];
}

public class SurveyAnswerResultDto {
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
}
