using AlumniManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlumniManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase{
    private readonly ApplicationDbContext _db;
    public UsersController(ApplicationDbContext db) => _db = db;

    // GET: api/users/by-roll/24P-3051
    [HttpGet("by-roll/{rollNumber}")]
    public async Task<ActionResult> GetByRoll(string rollNumber){
        var alumni = await _db.Alumnis
            .Where(a => a.RollNumber == rollNumber)
            .Select(a => new { a.UserId, a.FirstName, a.LastName, a.RollNumber })
            .FirstOrDefaultAsync();

        if(alumni is null) return NotFound("No alumni found with that roll number.");
        return Ok(alumni);
    }
}

