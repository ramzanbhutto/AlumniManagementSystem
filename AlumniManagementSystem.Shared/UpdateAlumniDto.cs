namespace AlumniManagementSystem.Shared;
public class UpdateAlumniDto{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? RollNumber { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? DateOfBirth { get; set; }
    public int? BatchYear { get; set; }
    public int? GraduationYear { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? CurrentJobTitle { get; set; }
    public string? CurrentCompany { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ProfilePicUrl { get; set; }
}
