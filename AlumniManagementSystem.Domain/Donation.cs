using AlumniManagementSystem.Domain.Enums;

namespace AlumniManagementSystem.Domain;

public class Donation
{
    public Guid DonationId { get; set; }
    public Guid AlumniId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "PKR";
    public DateTime DonationDate { get; set; } = DateTime.UtcNow;
    public string? Campaign { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? TransactionRef { get; set; }
    public bool IsAnonymous { get; set; } = false;
    public string? Message { get; set; }

    public Alumni Alumni { get; set; } = null!;
}
