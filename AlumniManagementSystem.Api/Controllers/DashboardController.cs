using AlumniManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;

namespace AlumniManagementSystem.Api.Controllers;

[EnableRateLimiting("ReportsPolicy")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase{
  private readonly ApplicationDbContext _ctx;
  public DashboardController(ApplicationDbContext ctx) => _ctx = ctx;
 
  // GET: api/dashboard/stats
  // Returns all counts/aggregates for the dashboard in one shot
  [HttpGet("stats")]
  public async Task<ActionResult> GetStats(){
    var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    var stats = new
      {
        totalAlumni= await _ctx.Alumnis.CountAsync(),
        profilesComplete= await _ctx.Alumnis.CountAsync(a => a.IsProfileComplete),
        upcomingEvents= await _ctx.Events.CountAsync(e => e.Status == Domain.Enums.EventStatus.Upcoming),
        activeJobs= await _ctx.JobPostings.CountAsync(j => j.IsActive),
        totalDonations= await _ctx.Donations.SumAsync(d => (decimal?)d.Amount) ?? 0,
        totalMessages    = await _ctx.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsDeleted),
        unreadMessages= await _ctx.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsRead && !m.IsDeleted),
        activeSurveys= await _ctx.Surveys.CountAsync(s => s.Status == Domain.Enums.SurveyStatus.Active),
        totalDepartments= await _ctx.Departments.CountAsync(),
        activePrograms= await _ctx.Programs.CountAsync(p => p.IsActive),
      };
     return Ok(stats);
    }
 
  // GET: api/dashboard/alumni-by-year
  // For chart: alumni count grouped by graduation year
  [HttpGet("alumni-by-year")]
  public async Task<ActionResult> AlumniByYear(){
    var data = await _ctx.Alumnis
        .Where(a => a.GraduationYear != null)
        .GroupBy(a => a.GraduationYear)
        .Select(g => new
            {
              year= g.Key,
              count= g.Count(),
              profilesComplete= g.Count(a => a.IsProfileComplete),
            })
        .OrderBy(x => x.year)
        .ToListAsync();
    return Ok(data);
  }
 
  // GET: api/dashboard/alumni-by-department
  [HttpGet("alumni-by-department")]
  public async Task<ActionResult> AlumniByDepartment(){
    var data = await _ctx.Alumnis
        .Include(a => a.Program).ThenInclude(p => p.Department)
        .GroupBy(a => a.Program.Department.Name)
        .Select(g => new { department = g.Key, count = g.Count() })
        .OrderByDescending(x => x.count)
        .ToListAsync();
    return Ok(data);
  }
 
  // GET: api/dashboard/donation-by-campaign
  [HttpGet("donation-by-campaign")]
  public async Task<ActionResult> DonationByCampaign(){
    var data = await _ctx.Donations
        .GroupBy(d => d.Campaign ?? "General")
        .Select(g => new
            {
              campaign= g.Key,
              count= g.Count(),
              totalAmount= g.Sum(d => d.Amount),
              avgAmount= g.Average(d => d.Amount),
            })
        .OrderByDescending(x => x.totalAmount)
        .ToListAsync();
    return Ok(data);
  }
 
  // GET: api/dashboard/jobs-by-type
  [HttpGet("jobs-by-type")]
  public async Task<ActionResult> JobsByType(){
    var data = await _ctx.JobPostings
        .Where(j => j.IsActive)
        .GroupBy(j => j.JobType)
        .Select(g => new { type = g.Key.ToString(), count = g.Count() })
        .OrderByDescending(x => x.count)
        .ToListAsync();
    return Ok(data);
  }
 
  // GET: api/dashboard/recent-activity
  [HttpGet("recent-activity")]
  public async Task<ActionResult> RecentActivity(){
    var recentAlumni = await _ctx.Alumnis
        .OrderByDescending(a => a.CreatedAt)
        .Take(5)
        .Select(a => new { a.FirstName, a.LastName, a.RollNumber, a.CreatedAt })
        .ToListAsync();
 
    var recentDonations = await _ctx.Donations
        .Include(d => d.Alumni)
        .OrderByDescending(d => d.DonationDate)
        .Take(5)
        .Select(d => new
            {
              donor = d.IsAnonymous ? "Anonymous" : $"{d.Alumni.FirstName} {d.Alumni.LastName}",
              amount= d.Amount,
              campaign= d.Campaign,
              date= d.DonationDate,
            })
        .ToListAsync();
 
    var recentJobs = await _ctx.JobPostings
        .Include(j => j.PostedByAlumni)
        .OrderByDescending(j => j.PostedAt)
        .Take(5)
        .Select(j => new
            {
              j.Title,
              j.Company,
              j.JobType,
              postedBy= $"{j.PostedByAlumni.FirstName} {j.PostedByAlumni.LastName}",
            })
        .ToListAsync();
 
      return Ok(new { recentAlumni, recentDonations, recentJobs });
  }
}
