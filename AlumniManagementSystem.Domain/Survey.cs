using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class Survey
{
    public Guid SurveyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }
    public SurveyStatus Status { get; set; } = SurveyStatus.Active;

    public User Creator { get; set; } = null!;
    public ICollection<SurveyQuestion> Questions { get; set; } = [];
    public ICollection<SurveyResponse> Responses { get; set; } = [];
}
