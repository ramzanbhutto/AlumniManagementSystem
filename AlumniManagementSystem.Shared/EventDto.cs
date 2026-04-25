namespace AlumniManagementSystem.Shared;
public class EventDto{
  public Guid EventId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public DateTime EventDate { get; set; }
  public string? Venue { get; set; }
  public int? Capacity { get; set; }
  public DateTime? RegistrationDeadline { get; set; }
  public string EventType { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public int RsvpCount { get; set; }
  public int AttendingCount { get; set; }
  public int CheckedInCount { get; set; }
}
