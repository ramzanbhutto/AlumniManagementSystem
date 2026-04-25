namespace AlumniManagementSystem.Domain;

public class Department
{
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // CS, SE, EE
    public string? Description { get; set; }
    public string? HodName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AcademicProgram> Programs { get; set; } = [];
}
