namespace AlumniManagementSystem.Shared;
public class MessageDto{
  public Guid MessageId { get; set; }
  public Guid SenderId { get; set; }
  public string SenderName { get; set; } = string.Empty;
  public string SenderEmail { get; set; } = string.Empty;
  public Guid ReceiverId { get; set; }
  public string ReceiverName { get; set; } = string.Empty;
  public string? Subject { get; set; }
  public string Body { get; set; } = string.Empty;
  public DateTime SentAt { get; set; }
  public bool IsRead { get; set; }
}
