using AlumniManagementSystem.Domain.Enums;
namespace AlumniManagementSystem.Domain;

public class Alumni
{
    public Guid AlumniId { get; set; }
    public Guid UserId { get; set; }          // FK -> User
    public string RollNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; // kept for quick lookup
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ProfilePicUrl { get; set; }
    public Guid ProgramId { get; set; }          // FK -> Program
    public int? BatchYear { get; set; }
    public int? GraduationYear { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? CurrentCompany { get; set; }
    public string? LinkedInUrl { get; set; }
    public bool IsProfileComplete { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public AcademicProgram Program { get; set; } = null!;
    public ICollection<EventRSVP> RSVPs { get; set; } = [];
    public ICollection<JobPosting> JobPostings { get; set; } = [];
    public ICollection<Donation> Donations { get; set; } = [];
    public ICollection<SurveyResponse> Responses { get; set; } = [];
    public ICollection<Contact> Contacts { get; set; } = [];
}
