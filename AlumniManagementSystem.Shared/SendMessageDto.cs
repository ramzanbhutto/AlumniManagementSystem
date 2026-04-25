namespace AlumniManagementSystem.Shared;
public class SendMessageDto
{
    [Required(ErrorMessage = "Receiver is required")]
    public Guid ReceiverId { get; set; }

    [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Message body is required")]
    [StringLength(5000, MinimumLength = 1, ErrorMessage = "Message body cannot exceed 5000 characters")]
    public string Body { get; set; } = string.Empty;
}
