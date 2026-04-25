using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class EventRSVP
{
    public Guid RsvpId { get; set; }
    public Guid EventId { get; set; }
    public Guid AlumniId { get; set; }
    public DateTime RsvpDate { get; set; } = DateTime.UtcNow;
    public RsvpStatus Status { get; set; } = RsvpStatus.Attending;
    public bool CheckedIn { get; set; } = false;
    public DateTime? CheckedInAt { get; set; }

    public Event Event { get; set; } = null!;
    public Alumni Alumni { get; set; } = null!;
}
