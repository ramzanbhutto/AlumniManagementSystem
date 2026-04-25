namespace AlumniManagementSystem.Domain;

public class AcademicProgram
{
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = string.Empty; // BS Software Engineering
    public string Type { get; set; } = string.Empty; // BS / MS / PhD
    public int DurationYears { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;

    public Department Department { get; set; } = null!;
    public ICollection<Alumni> Alumnis { get; set; } = [];
}
