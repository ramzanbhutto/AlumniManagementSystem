namespace AlumniManagementSystem.Shared;
public class EventRsvpDto{
  public Guid RsvpId { get; set; }
  public Guid EventId { get; set; }
  public string EventTitle { get; set; } = string.Empty;
  public string AlumniName { get; set; } = string.Empty;
  public string AlumniRoll { get; set; } = string.Empty;
  public DateTime RsvpDate { get; set; }
  public string Status { get; set; } = string.Empty;
  public bool CheckedIn { get; set; }
}
