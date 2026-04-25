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
}
