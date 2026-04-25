using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize] // all endpoints require JWT
public class AlumniController : ControllerBase{
  private readonly AlumniService _svc;
  public AlumniController(AlumniService svc) => _svc = svc;
 
  // GET: api/alumni
  [HttpGet]
  public async Task<ActionResult<List<AlumniDto>>> GetAll(){
    var list = await _svc.GetAllAsync();
    return Ok(list.Select(Map));
  }
 
  // GET: api/alumni/{rollNumber}
  [HttpGet("{rollNumber}")]
  public async Task<ActionResult<AlumniDto>> GetByRollNumber(string rollNumber){
    var a = await _svc.GetByRollNumberAsync(rollNumber);
    return a is null ? NotFound() : Ok(Map(a));
  }
 
  // POST: api/alumni
  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<AlumniDto>> Create(CreateAlumniDto dto){
    var a = new Alumni
      {
        AlumniId= Guid.NewGuid(),
        RollNumber= dto.RollNumber,
        FirstName= dto.FirstName,
        LastName= dto.LastName,
        Email= dto.Email,
        GraduationYear= dto.GraduationYear,
        ProgramId= Guid.Empty, // set properly via RegisterDto in auth flow
      };
    await _svc.CreateAsync(a);
    return Ok(Map(a));
  }
 
  // PUT: api/alumni/{id}
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, CreateAlumniDto dto){
    var a = await _svc.GetByIdAsync(id);
    if(a is null) return NotFound();
 
    a.RollNumber= dto.RollNumber;
    a.FirstName= dto.FirstName;
    a.LastName= dto.LastName;
    a.Email= dto.Email;
    a.GraduationYear= dto.GraduationYear;
 
    await _svc.UpdateAsync(a);
    return NoContent();
  }
 
  // DELETE: api/alumni/{id}
  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Delete(Guid id){
    var a = await _svc.GetByIdAsync(id);
    if(a is null) return NotFound();
    await _svc.DeleteAsync(a);
    return NoContent();
  }
 
  // private mapper (stays in controller — thin mapping only)
  private static AlumniDto Map(Alumni a) => new()
    {
      Id = a.AlumniId,
      RollNumber = a.RollNumber,
      FirstName = a.FirstName,
      LastName = a.LastName,
      Email = a.Email,
      Phone = a.Phone,
      Gender = a.Gender?.ToString(),
      City = a.City,
      Country = a.Country,
      BatchYear = a.BatchYear,
      GraduationYear = a.GraduationYear,
      CurrentJobTitle = a.CurrentJobTitle,
      CurrentCompany = a.CurrentCompany,
      LinkedInUrl = a.LinkedInUrl,
      IsProfileComplete = a.IsProfileComplete,
      ProgramName = a.Program?.Name,
      ProgramType = a.Program?.Type,
      DepartmentName = a.Program?.Department?.Name,
      DepartmentCode = a.Program?.Department?.Code,
    };
}
