using AlumniManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
 
namespace AlumniManagementSystem.Api.Controllers;

[EnableRateLimiting("ReportsPolicy")] 
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ReportsController : ControllerBase{
  private readonly ApplicationDbContext _ctx;
  public ReportsController(ApplicationDbContext ctx) => _ctx = ctx;
 
  // GET: api/reports/donations?year=2024
  [HttpGet("donations")]
  public async Task<ActionResult> DonationsReport([FromQuery] int? year){
    var query = _ctx.Donations.Include(d => d.Alumni).AsQueryable();
    if(year.HasValue)
      query = query.Where(d => d.DonationDate.Year == year.Value);
 
    var byCampaign = await query
        .GroupBy(d => d.Campaign ?? "General")
        .Select(g => new
          {
            campaign= g.Key,
            count= g.Count(),
            total= g.Sum(d => d.Amount),
            average= g.Average(d => d.Amount),
            largest= g.Max(d => d.Amount),
          })
        .OrderByDescending(x => x.total)
        .ToListAsync();
 
    var topDonors = await query
        .GroupBy(d => new { d.AlumniId, d.Alumni.FirstName, d.Alumni.LastName, d.IsAnonymous })
        .Select(g => new
          {
            name= g.Key.IsAnonymous ? "Anonymous" : $"{g.Key.FirstName} {g.Key.LastName}",
            total= g.Sum(d => d.Amount),
            count= g.Count(),
          })
        .OrderByDescending(x => x.total)
        .Take(10)
        .ToListAsync();
 
    var monthly = await query
        .GroupBy(d => new { d.DonationDate.Year, d.DonationDate.Month })
        .Select(g => new
          {
            year= g.Key.Year,
            month= g.Key.Month,
            total= g.Sum(d => d.Amount),
            count= g.Count(),
          })
        .OrderBy(x => x.year).ThenBy(x => x.month)
        .ToListAsync();
 
    return Ok(new { byCampaign, topDonors, monthly, grandTotal= await query.SumAsync(d => (decimal?)d.Amount) ?? 0 });
  }
 
  // GET: api/reports/events
  [HttpGet("events")]
  public async Task<ActionResult> EventsReport(){
    var data = await _ctx.Events
        .Include(e => e.RSVPs)
        .Select(e => new
          {
            e.EventId,
            e.Title,
            e.EventType,
            e.EventDate,
            e.Capacity,
            totalRsvp= e.RSVPs.Count,
            attending= e.RSVPs.Count(r => r.Status == Domain.Enums.RsvpStatus.Attending),
            checkedIn= e.RSVPs.Count(r => r.CheckedIn),
            occupancyPct= e.Capacity == null ? (double?)null : Math.Round((double)e.RSVPs.Count(r => r.CheckedIn) / e.Capacity.Value * 100, 1),
          })
        .OrderByDescending(x => x.EventDate)
        .ToListAsync();
    return Ok(data);
  }
 
  // GET: api/reports/alumni-employment
  [HttpGet("alumni-employment")]
  public async Task<ActionResult> AlumniEmploymentReport(){
    var employed = await _ctx.Alumnis
        .CountAsync(a => a.CurrentCompany != null);
    var total = await _ctx.Alumnis.CountAsync();
 
    var byCompany = await _ctx.Alumnis
        .Where(a => a.CurrentCompany != null)
        .GroupBy(a => a.CurrentCompany!)
        .Select(g => new { company= g.Key, count= g.Count() })
        .OrderByDescending(x => x.count)
        .Take(10)
        .ToListAsync();
 
    var byJobTitle = await _ctx.Alumnis
        .Where(a => a.CurrentJobTitle != null)
        .GroupBy(a => a.CurrentJobTitle!)
        .Select(g => new { title= g.Key, count= g.Count() })
        .OrderByDescending(x => x.count)
        .Take(10)
        .ToListAsync();
 
    return Ok(new
      {
        total, employed,
        unemployedOrUnknown= total - employed,
        employmentRate= Math.Round((double)employed / total * 100, 1),
        byCompany, byJobTitle,
      });
  }
 
  // GET: api/reports/surveys
  [HttpGet("surveys")]
  public async Task<ActionResult> SurveysReport(){
    var totalAlumni = await _ctx.Alumnis.CountAsync();
    var data = await _ctx.Surveys
        .Include(s => s.Responses)
        .Select(s => new
          {
            s.SurveyId,
            s.Title,
            s.Status,
            responses= s.Responses.Count,
            responseRate= totalAlumni == 0 ? 0 : Math.Round((double)s.Responses.Count / totalAlumni * 100, 1),
          })
        .ToListAsync();
    return Ok(data);
  }
}
