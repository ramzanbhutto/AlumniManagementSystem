namespace AlumniManagementSystem.Shared;
public class CreateJobPostingDto
{
    [Required(ErrorMessage = "Job title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 200 characters")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [StringLength(5000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 5000 characters")]
    public string Description { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Job type is required")]
    [RegularExpression(@"^(FullTime|PartTime|Remote|Contract)$",
        ErrorMessage = "Job type must be: FullTime, PartTime, Remote, or Contract")]
    public string JobType { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Salary range cannot exceed 100 characters")]
    public string? SalaryRange { get; set; }

    [Url(ErrorMessage = "Application URL must be a valid URL")]
    [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
    public string? ApplicationUrl { get; set; }

    public DateOnly? Deadline { get; set; }
}
