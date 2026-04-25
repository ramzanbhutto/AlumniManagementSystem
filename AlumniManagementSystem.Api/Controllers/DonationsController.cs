using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using AlumniManagementSystem.Infrastructure;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DonationsController : ControllerBase{
  private readonly DonationService _svc;
  private readonly ApplicationDbContext _ctx;
 
  public DonationsController(DonationService svc, ApplicationDbContext ctx){
    _svc = svc;
    _ctx = ctx;
  }
 
  [HttpGet]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> GetAll(){
    var donations = await _ctx.Donations
        .Include(d => d.Alumni)
        .OrderByDescending(d => d.DonationDate)
        .ToListAsync();
    return Ok(donations.Select(MapDonation));
  }
 
  [HttpGet("my")]
  public async Task<ActionResult> GetMy(){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var alumni = await _ctx.Alumnis.FirstOrDefaultAsync(a => a.UserId == userId);
    if(alumni is null) return NotFound("Alumni profile not found.");
    var donations = await _ctx.Donations
        .Where(d => d.AlumniId == alumni.AlumniId)
        .OrderByDescending(d => d.DonationDate)
        .ToListAsync();
    return Ok(donations.Select(MapDonation));
  }
 
  [HttpPost]
  public async Task<ActionResult> Create(CreateDonationDto dto){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var alumni = await _ctx.Alumnis.FirstOrDefaultAsync(a => a.UserId == userId);
    if(alumni is null) return NotFound("Alumni profile not found.");
 
    PaymentMethod? method = null;
    if(!string.IsNullOrEmpty(dto.PaymentMethod) &&
        Enum.TryParse<PaymentMethod>(dto.PaymentMethod, out var parsed))
      method = parsed;
 
    var donation = new Donation
      {
        DonationId = Guid.NewGuid(),
        AlumniId = alumni.AlumniId,
        Amount = dto.Amount,
        Currency = dto.Currency,
        Campaign = dto.Campaign,
        PaymentMethod = method,
        IsAnonymous = dto.IsAnonymous,
        Message = dto.Message,
      };
    await _svc.CreateAsync(donation);
    return Ok(MapDonation(donation));
  }
 
  private static DonationDto MapDonation(Donation d) => new()
    {
      DonationId = d.DonationId,
      Amount = d.Amount,
      Currency = d.Currency,
      DonationDate = d.DonationDate,
      Campaign = d.Campaign,
      PaymentMethod = d.PaymentMethod?.ToString(),
      IsAnonymous = d.IsAnonymous,
      Message = d.Message,
      AlumniName = d.IsAnonymous ? "Anonymous" : (d.Alumni is null ? null : $"{d.Alumni.FirstName} {d.Alumni.LastName}"),
      AlumniRoll = d.IsAnonymous ? null : d.Alumni?.RollNumber,
    };
}
