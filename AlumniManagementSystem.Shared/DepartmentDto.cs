namespace AlumniManagementSystem.Shared;
public class DepartmentDto{
  public Guid DepartmentId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;
  public string? Description { get; set; }
  public string? HodName { get; set; }
  public int ProgramCount { get; set; }
  public int AlumniCount { get; set; }
}
