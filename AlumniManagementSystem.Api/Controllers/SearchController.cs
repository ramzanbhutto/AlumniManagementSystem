using AlumniManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SearchController : ControllerBase{
  private readonly ApplicationDbContext _ctx;
  public SearchController(ApplicationDbContext ctx) => _ctx = ctx;
 
  // GET: api/search/alumni?name=Ali&dept=SE&gradYear=2023&city=Peshawar
  [HttpGet("alumni")]
  public async Task<ActionResult> SearchAlumni(
    [FromQuery] string? name,
    [FromQuery] string? dept,
    [FromQuery] int? gradYear,
    [FromQuery] string? city,
    [FromQuery] string? company,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20){
    var query = _ctx.Alumnis
        .Include(a => a.Program).ThenInclude(p => p.Department)
        .Include(a => a.User)
        .AsQueryable();
 
    if(!string.IsNullOrWhiteSpace(name))
      query = query.Where(a =>
          (a.FirstName + " " + a.LastName).Contains(name) ||
          a.RollNumber.Contains(name));
 
    if(!string.IsNullOrWhiteSpace(dept))
      query = query.Where(a =>
          a.Program.Department.Code == dept ||
          a.Program.Department.Name.Contains(dept));
 
    if(gradYear.HasValue)
      query = query.Where(a => a.GraduationYear == gradYear.Value);
 
    if(!string.IsNullOrWhiteSpace(city))
      query = query.Where(a => a.City != null && a.City.Contains(city));
 
    if(!string.IsNullOrWhiteSpace(company))
      query = query.Where(a => a.CurrentCompany != null &&
          a.CurrentCompany.Contains(company));
 
    var total= await query.CountAsync();
    var items= await query
        .OrderBy(a => a.LastName)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(a => new
          {
            a.AlumniId,
            a.RollNumber,
            fullName= $"{a.FirstName} {a.LastName}",
            a.Email,
            a.City,
            a.GraduationYear,
            a.CurrentJobTitle,
            a.CurrentCompany,
            program= a.Program.Name,
            department= a.Program.Department.Name,
          })
        .ToListAsync();
 
    return Ok(new { total, page, pageSize, items });
  }
 
  // GET: api/search/events?title=Reunion&type=Reunion&status=Upcoming
  [HttpGet("events")]
  public async Task<ActionResult> SearchEvents(
    [FromQuery] string? title,
    [FromQuery] string? type,
    [FromQuery] string? status){
    var query = _ctx.Events.AsQueryable();
 
    if(!string.IsNullOrWhiteSpace(title))
      query = query.Where(e => e.Title.Contains(title));
 
    if(!string.IsNullOrWhiteSpace(type) &&
        Enum.TryParse<Domain.Enums.EventType>(type, out var eType))
      query = query.Where(e => e.EventType == eType);
 
    if(!string.IsNullOrWhiteSpace(status) &&
        Enum.TryParse<Domain.Enums.EventStatus>(status, out var eStatus))
      query = query.Where(e => e.Status == eStatus);
 
    var items = await query
        .OrderBy(e => e.EventDate)
        .Select(e => new
          {
            e.EventId, e.Title, e.EventDate, e.Venue,
            e.Capacity, e.EventType, e.Status,
          })
        .ToListAsync();
 
    return Ok(items);
  }
 
  // GET: api/search/jobs?keyword=dotnet&type=Remote
  [HttpGet("jobs")]
  public async Task<ActionResult> SearchJobs(
    [FromQuery] string? keyword,
    [FromQuery] string? type,
    [FromQuery] string? location){
    var query = _ctx.JobPostings
        .Include(j => j.PostedByAlumni)
        .Where(j => j.IsActive)
        .AsQueryable();
 
    if(!string.IsNullOrWhiteSpace(keyword))
      query = query.Where(j =>
          j.Title.Contains(keyword) ||
          j.Company.Contains(keyword) ||
          j.Description.Contains(keyword));
 
    if(!string.IsNullOrWhiteSpace(type) &&
        Enum.TryParse<Domain.Enums.JobType>(type, out var jType))
      query = query.Where(j => j.JobType == jType);
 
    if(!string.IsNullOrWhiteSpace(location))
      query = query.Where(j => j.Location != null && j.Location.Contains(location));
 
    var items = await query
        .OrderByDescending(j => j.PostedAt)
        .Select(j => new
          {
            j.JobId, j.Title, j.Company, j.Location,
            j.JobType, j.SalaryRange, j.Deadline, j.PostedAt,
            postedBy= $"{j.PostedByAlumni.FirstName} {j.PostedByAlumni.LastName}",
          })
        .ToListAsync();
 
    return Ok(items);
  }
}
