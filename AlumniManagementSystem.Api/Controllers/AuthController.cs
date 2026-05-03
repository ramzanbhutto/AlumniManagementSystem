using AlumniManagementSystem.Application.Interfaces;
using AlumniManagementSystem.Application.Services;
using AlumniManagementSystem.Domain;
using AlumniManagementSystem.Domain.Enums;
using AlumniManagementSystem.Shared;
using AlumniManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
 
namespace AlumniManagementSystem.Api.Controllers;

[EnableRateLimiting("AuthPolicy")]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase{
  private readonly ApplicationDbContext _ctx; // only for user lookup during auth
  private readonly IAuthService _auth;
  private readonly AlumniService _alumniSvc;
 
  public AuthController(ApplicationDbContext ctx, IAuthService auth, AlumniService alumniSvc){
    _ctx = ctx;
    _auth = auth;
    _alumniSvc = alumniSvc;
  }
 
  // POST: api/auth/register
  [HttpPost("register")]
  public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto){
    if(_ctx.Users.Any(u => u.Email == dto.Email)) return Conflict("Email already registered.");
 
    var user = new User
      {
        UserId= Guid.NewGuid(),
        Email= dto.Email,
        PasswordHash= _auth.HashPassword(dto.Password),
        Role= UserRole.Alumni,
      };
    await _ctx.Users.AddAsync(user);
 
    var programId = dto.ProgramId != Guid.Empty && _ctx.Programs.Any(p => p.ProgramId == dto.ProgramId) ? dto.ProgramId : _ctx.Programs.Select(p => p.ProgramId).FirstOrDefault();

    if(programId == Guid.Empty)
      return BadRequest("No academic programs exist. Ask admin to create one first.");

    var alumni = new Alumni
    {
      AlumniId= Guid.NewGuid(),
      UserId= user.UserId,
      RollNumber= dto.RollNumber,
      FirstName= dto.FirstName,
      LastName= dto.LastName,
      Email= dto.Email,
      ProgramId= programId,
      GraduationYear= dto.GraduationYear,
    };
    await _ctx.Alumnis.AddAsync(alumni);
    await _ctx.SaveChangesAsync();
 
    return Ok(new AuthResponseDto
      {
        Token= _auth.GenerateJwtToken(user),
        Role= user.Role.ToString(),
        UserId= user.UserId,
        AlumniId= alumni.AlumniId,
        FullName= $"{alumni.FirstName} {alumni.LastName}",
      });
  }
 
  // POST: api/auth/login
  [HttpPost("login")]
  public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto){
    var user = _ctx.Users.FirstOrDefault(u => u.Email == dto.Email);
    if(user is null || !_auth.VerifyPassword(dto.Password, user.PasswordHash)) return Unauthorized("Invalid credentials.");
 
    user.LastLoginAt = DateTime.UtcNow;
    await _ctx.SaveChangesAsync();
 
    var alumni = await _alumniSvc.GetByUserIdAsync(user.UserId) ;
 
    return Ok(new AuthResponseDto
      {
        Token= _auth.GenerateJwtToken(user),
        Role= user.Role.ToString(),
        UserId= user.UserId,
        AlumniId= alumni?.AlumniId ?? Guid.Empty,
        FullName= alumni is null ? user.Email : $"{alumni.FirstName} {alumni.LastName}",
      });
  }
}
