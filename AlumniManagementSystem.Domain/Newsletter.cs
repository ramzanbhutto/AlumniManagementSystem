using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class Newsletter
{
    public Guid NewsletterId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public NewsletterStatus Status { get; set; } = NewsletterStatus.Draft;
    public string TargetRole { get; set; } = "All"; // All / Alumni / Guest

    public User Creator { get; set; } = null!;
    public ICollection<NewsletterRecipient> Recipients { get; set; } = [];
}
