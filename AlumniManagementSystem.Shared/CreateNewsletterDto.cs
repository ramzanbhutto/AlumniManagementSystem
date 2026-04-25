namespace AlumniManagementSystem.Shared;
public class CreateNewsletterDto
{
    [Required(ErrorMessage = "Newsletter title is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Newsletter content is required")]
    [MinLength(10, ErrorMessage = "Content must be at least 10 characters")]
    public string Content { get; set; } = string.Empty;

    [RegularExpression(@"^(All|Alumni|Guest)$",
        ErrorMessage = "Target role must be: All, Alumni, or Guest")]
    public string? TargetRole { get; set; } = "All";
}
