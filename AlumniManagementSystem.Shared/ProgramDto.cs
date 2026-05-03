namespace AlumniManagementSystem.Shared;
public class ProgramDto{
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public int? DurationYears { get; set; }
    public bool IsActive { get; set; }
    public string? DepartmentName { get; set; }
    public string? DepartmentCode { get; set; }
    public int AlumniCount { get; set; }
}
