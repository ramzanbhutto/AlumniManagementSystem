namespace AlumniManagementSystem.Shared;
public class CreateDonationDto
{
    [Required(ErrorMessage = "Donation amount is required")]
    [Range(1, 10000000, ErrorMessage = "Amount must be between 1 and 10,000,000")]
    public decimal Amount { get; set; }

    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter code like PKR, USD")]
    public string Currency { get; set; } = "PKR";

    [StringLength(200, ErrorMessage = "Campaign name cannot exceed 200 characters")]
    public string? Campaign { get; set; }

    [RegularExpression(@"^(Card|BankTransfer|JazzCash|EasyPaisa|Other)$",
        ErrorMessage = "Payment method must be: Card, BankTransfer, JazzCash, EasyPaisa, or Other")]
    public string? PaymentMethod { get; set; }

    public bool IsAnonymous { get; set; } = false;

    [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
    public string? Message { get; set; }
}
