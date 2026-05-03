using AlumniManagementSystem.Infrastructure;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentsController : ControllerBase{
  private readonly ApplicationDbContext _ctx;
  public DepartmentsController(ApplicationDbContext ctx) => _ctx = ctx;
 
  [HttpGet]
  public async Task<ActionResult> GetAll(){
    var depts = await _ctx.Departments
        .Include(d => d.Programs)
            .ThenInclude(p => p.Alumnis)
        .ToListAsync();
    return Ok(depts.Select(d => new DepartmentDto
      {
        DepartmentId = d.DepartmentId,
        Name = d.Name,
        Code = d.Code,
        Description = d.Description,
        HodName = d.HodName,
        ProgramCount = d.Programs.Count,
        AlumniCount = d.Programs.Sum(p => p.Alumnis.Count),
      }));
  }
 
  [HttpGet("{id:guid}/programs")]
  public async Task<ActionResult> GetPrograms(Guid id){
    var programs = await _ctx.Programs
        .Where(p => p.DepartmentId == id)
        .Include(p => p.Department)
        .Include(p => p.Alumnis)
        .ToListAsync();
    return Ok(programs.Select(p => new ProgramDto
      {
        ProgramId = p.ProgramId,
        Name = p.Name,
        Type = p.Type,
        DurationYears = p.DurationYears,
        IsActive = p.IsActive,
        DepartmentName = p.Department.Name,
        DepartmentCode = p.Department.Code,
        AlumniCount = p.Alumnis.Count,
      }));
  }

   [HttpPost("{id:guid}/programs")]
   [Authorize(Roles = "Admin")]
   public async Task<ActionResult> AddProgram(Guid id, CreateProgramDto dto){
     var dept = await _ctx.Departments.FindAsync(id);
     if(dept is null) return NotFound("Department not found.");

     var program = new AlumniManagementSystem.Domain.AcademicProgram
     {
        ProgramId     = Guid.NewGuid(),
        Name          = dto.Name,
        Type          = dto.Type,
        DurationYears = dto.DurationYears,
        DepartmentId  = id,
        IsActive      = dto.IsActive,
     };

     _ctx.Programs.Add(program);
     await _ctx.SaveChangesAsync();

     return Ok(new ProgramDto
     {
        ProgramId      = program.ProgramId,
        Name           = program.Name,
        Type           = program.Type,
        DurationYears  = program.DurationYears,
        IsActive       = program.IsActive,
        DepartmentName = dept.Name,
        DepartmentCode = dept.Code,
        AlumniCount    = 0,
     });
   }
}
