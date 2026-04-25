namespace AlumniManagementSystem.Shared;
public class CreateEventDto
{
    [Required(ErrorMessage = "Event title is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Event date is required")]
    public DateTime EventDate { get; set; }

    [StringLength(200, ErrorMessage = "Venue cannot exceed 200 characters")]
    public string? Venue { get; set; }

    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10000")]
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Event type is required")]
    [RegularExpression(@"^(Reunion|Seminar|CareerFair|Social|Workshop)$",
        ErrorMessage = "Event type must be: Reunion, Seminar, CareerFair, Social, or Workshop")]
    public string EventType { get; set; } = string.Empty;

    public DateTime? RegistrationDeadline { get; set; }
}
