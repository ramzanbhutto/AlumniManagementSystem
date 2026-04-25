namespace AlumniManagementSystem.Shared;
public class JobPostingDto{
  public Guid JobId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string? Location { get; set; }
  public string JobType { get; set; } = string.Empty;
  public string? SalaryRange { get; set; }
  public string? ApplicationUrl { get; set; }
  public DateOnly? Deadline { get; set; }
  public DateTime PostedAt { get; set; }
  public bool IsActive { get; set; }
  public string PostedByName { get; set; } = string.Empty;
  public string? PostedByRollNumber { get; set; }
  public string? PostedByDepartment { get; set; }
}
