namespace AlumniManagementSystem.Domain;

public class NewsletterRecipient
{
    public Guid RecipientId { get; set; }
    public Guid NewsletterId { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public bool IsRead { get; set; } = false;

    public Newsletter Newsletter { get; set; } = null!;
    public User User { get; set; } = null!;
}
