using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class Event
{
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public string? Venue { get; set; }
    public int? Capacity { get; set; }
    public DateTime? RegistrationDeadline { get; set; }
    public EventType EventType { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Upcoming;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Creator { get; set; } = null!;
    public ICollection<EventRSVP> RSVPs { get; set; } = [];
}
