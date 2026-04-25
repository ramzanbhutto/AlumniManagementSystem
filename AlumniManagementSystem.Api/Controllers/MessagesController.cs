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
public class MessagesController : ControllerBase{
  private readonly IMessageRepository _repo;
  public MessagesController(IMessageRepository repo) => _repo = repo;
 
  [HttpGet("inbox")]
  public async Task<ActionResult> GetInbox(){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var msgs = await _repo.GetInboxAsync(userId);
    return Ok(msgs.Select(MapMessage));
  }
 
  [HttpGet("sent")]
  public async Task<ActionResult> GetSent(){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var msgs = await _repo.GetSentAsync(userId);
    return Ok(msgs.Select(MapMessage));
  }
 
  [HttpPost]
  public async Task<ActionResult> Send(SendMessageDto dto){
    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var msg = new Message
      {
        MessageId = Guid.NewGuid(),
        SenderId = userId,
        ReceiverId = dto.ReceiverId,
        Subject = dto.Subject,
        Body = dto.Body,
      };
    await _repo.AddAsync(msg);
    await _repo.SaveChangesAsync();
    return Ok(MapMessage(msg));
  }
 
  private static MessageDto MapMessage(Message m) => new()
    {
      MessageId = m.MessageId,
      SenderId = m.SenderId,
      SenderName = m.Sender?.AlumniProfile is null
          ? m.Sender?.Email ?? ""
          : $"{m.Sender.AlumniProfile.FirstName} {m.Sender.AlumniProfile.LastName}",
      SenderEmail = m.Sender?.Email ?? "",
      ReceiverId = m.ReceiverId,
      ReceiverName = m.Receiver?.AlumniProfile is null
          ? m.Receiver?.Email ?? ""
          : $"{m.Receiver.AlumniProfile.FirstName} {m.Receiver.AlumniProfile.LastName}",
      Subject = m.Subject,
      Body = m.Body,
      SentAt = m.SentAt,
      IsRead = m.IsRead,
    };
}
