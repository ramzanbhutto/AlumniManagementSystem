namespace AlumniManagementSystem.Shared;
public class CreateProgramDto{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int DurationYears { get; set; } = 4;
    public Guid DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
}
