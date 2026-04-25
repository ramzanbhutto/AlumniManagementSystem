namespace AlumniManagementSystem.Domain;

public class SurveyResponse
{
    public Guid ResponseId { get; set; }
    public Guid SurveyId { get; set; }
    public Guid AlumniId { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public Survey Survey { get; set; } = null!;
    public Alumni Alumni { get; set; } = null!;
    public ICollection<SurveyAnswer> Answers { get; set; } = [];
}
