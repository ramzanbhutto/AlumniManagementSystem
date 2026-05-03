using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
 
namespace AlumniManagementSystem.Api.Controllers;
 
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NewsletterController : ControllerBase{
  private readonly INewsletterRepository _repo;
  public NewsletterController(INewsletterRepository repo) => _repo = repo;
 
  [HttpGet]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> GetAll(){
    var newsletters = await _repo.GetAllAsync();
    return Ok(newsletters.Select(n => new NewsletterDto
      {
        NewsletterId = n.NewsletterId,
        Title = n.Title,
        Content = n.Content,
        Status = n.Status.ToString(),
        TargetRole = n.TargetRole,
        CreatedAt = n.CreatedAt,
        SentAt = n.SentAt,
        RecipientCount = n.Recipients?.Count ?? 0,
        ReadCount = n.Recipients?.Count(r => r.IsRead) ?? 0,
      }));
  }
 
  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Create([FromBody] CreateNewsletterDto dto){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var newsletter = new Newsletter
      {
        NewsletterId = Guid.NewGuid(),
        Title = dto.Title,
        Content = dto.Content,
        CreatedBy = userId,
        TargetRole = dto.TargetRole ?? "All",
      };
    await _repo.AddAsync(newsletter);
    await _repo.SaveChangesAsync();
    return Ok(new NewsletterDto
      {
        NewsletterId = newsletter.NewsletterId,
        Title = newsletter.Title,
        Content = newsletter.Content,
        Status = newsletter.Status.ToString(),
        TargetRole = newsletter.TargetRole,
        CreatedAt = newsletter.CreatedAt,
      });
  }

  // Alumni/Guest: see newsletters sent to them
  [HttpGet("my")]
  public async Task<ActionResult> GetMy(){
    var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Guest";
    var all = await _repo.GetAllAsync();
    var filtered = all
      .Where(n => n.Status == Domain.Enums.NewsletterStatus.Sent &&
                  (n.TargetRole == "All" || n.TargetRole == role))
      .Select(n => new NewsletterDto{
        NewsletterId = n.NewsletterId,
        Title = n.Title,
        Content = n.Content,
        Status = n.Status.ToString(),
        TargetRole = n.TargetRole,
        CreatedAt = n.CreatedAt,
        SentAt = n.SentAt,
        RecipientCount = n.Recipients?.Count ?? 0,
        ReadCount = n.Recipients?.Count(r => r.IsRead) ?? 0,
      });
    return Ok(filtered);
  }

  // Admin: mark a newsletter as Sent
  [HttpPost("{id:guid}/send")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> Send(Guid id){
    var n = await _repo.GetByIdAsync(id);
    if(n == null) return NotFound();
    n.Status = Domain.Enums.NewsletterStatus.Sent;
    n.SentAt = DateTime.UtcNow;
    await _repo.SaveChangesAsync();
    return Ok();
  }
}
