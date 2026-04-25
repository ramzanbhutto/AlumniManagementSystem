namespace AlumniManagementSystem.Shared;
public class DonationDto{
  public Guid DonationId  { get; set; }
  public decimal Amount { get; set; }
  public string Currency  { get; set; } = "PKR";
  public DateTime DonationDate { get; set; }
  public string? Campaign { get; set; }
  public string? PaymentMethod { get; set; }
  public bool IsAnonymous  { get; set; }
  public string? Message  { get; set; }
  // hidden if Anonymous
  public string? AlumniName { get; set; }
  public string? AlumniRoll { get; set; }
}
