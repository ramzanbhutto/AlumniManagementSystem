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
        RecipientCount = n.Recipients.Count,
        ReadCount = n.Recipients.Count(r => r.IsRead),
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
}
