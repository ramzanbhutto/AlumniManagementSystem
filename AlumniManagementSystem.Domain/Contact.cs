using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class Contact
{
    public Guid ContactId { get; set; }
    public Guid AlumniId { get; set; }
    public ContactType Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;

    public Alumni Alumni { get; set; } = null!;
}
