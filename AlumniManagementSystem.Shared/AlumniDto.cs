namespace AlumniManagementSystem.Shared;
public class AlumniDto{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Roll Number is required")]
    public string RollNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    public int? GraduationYear { get; set; }
    public string? Phone { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ProfilePicUrl { get; set; }
    public int? BatchYear { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? CurrentCompany { get; set; }
    public string? LinkedInUrl { get; set; }
    public bool IsProfileComplete { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid? ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public string? ProgramType { get; set; }
    public string? DepartmentName { get; set; }
    public string? DepartmentCode { get; set; }
    
    public string? LoginEmail { get; set; }
    public string? Role { get; set; }
}
