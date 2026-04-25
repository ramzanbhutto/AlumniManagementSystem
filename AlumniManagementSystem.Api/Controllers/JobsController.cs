using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JobsController : ControllerBase{
  private readonly JobService _svc;
  public JobsController(JobService svc) => _svc = svc;
 
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll(){
    var jobs = await _svc.GetAllActiveAsync();
    return Ok(jobs.Select(j => new JobPostingDto
      {
        JobId= j.JobId,
        Title= j.Title,
        Company= j.Company,
        Description= j.Description,
        Location= j.Location,
        JobType= j.JobType.ToString(),
        SalaryRange= j.SalaryRange,
        ApplicationUrl= j.ApplicationUrl,
        Deadline= j.Deadline,
        PostedAt= j.PostedAt,
        IsActive = j.IsActive,
        PostedByName= j.PostedByAlumni is null ? "" : $"{j.PostedByAlumni.FirstName} {j.PostedByAlumni.LastName}",
        PostedByRollNumber= j.PostedByAlumni?.RollNumber,
        PostedByDepartment= j.PostedByAlumni?.Program?.Department?.Name,
      }));
  }
 
  [HttpPost]
  public async Task<ActionResult> Create(CreateJobPostingDto dto){
    if(!Enum.TryParse<JobType>(dto.JobType, out var jobType)) return BadRequest("Invalid job type. Valid: FullTime, PartTime, Remote, Contract");
 
    // Get alumniId from JWT claims (you store it or look it up)
    var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    // pass userId as PostedBy — in production resolve to AlumniId via service
    var job = new JobPosting
      {
        JobId= Guid.NewGuid(),
        Title= dto.Title,
        Company= dto.Company,
        Description= dto.Description,
        Location= dto.Location,
        JobType= jobType,
        SalaryRange= dto.SalaryRange,
        ApplicationUrl= dto.ApplicationUrl,
        Deadline= dto.Deadline,
        PostedBy= userId, // resolve to AlumniId in real impl
      };
    await _svc.CreateAsync(job);
    return Ok(new { job.JobId });
  }
}
