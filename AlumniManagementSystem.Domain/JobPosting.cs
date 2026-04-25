using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class JobPosting
{
    public Guid JobId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public JobType JobType { get; set; }
    public string? SalaryRange { get; set; }
    public string? ApplicationUrl { get; set; }
    public DateOnly? Deadline { get; set; }
    public Guid PostedBy { get; set; }
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public Alumni PostedByAlumni { get; set; } = null!;
}
