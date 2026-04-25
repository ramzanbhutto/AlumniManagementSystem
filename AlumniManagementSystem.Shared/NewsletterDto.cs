namespace AlumniManagementSystem.Shared;
public class NewsletterDto{
  public Guid NewsletterId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string TargetRole { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public DateTime? SentAt { get; set; }
  public int RecipientCount { get; set; }
  public int ReadCount { get; set; }
}
