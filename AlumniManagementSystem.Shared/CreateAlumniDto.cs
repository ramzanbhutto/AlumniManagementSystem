namespace AlumniManagementSystem.Shared;
public class CreateAlumniDto
{
    [Required(ErrorMessage = "Roll Number is required")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Roll Number must be between 5 and 20 characters")]
    [RegularExpression(@"^\d{2}[A-Z]-\d{4}$", ErrorMessage = "Roll Number format must be like 24P-3051")]
    public string RollNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Range(1990, 2030, ErrorMessage = "Graduation year must be between 1990 and 2030")]
    public int? GraduationYear { get; set; }
}
