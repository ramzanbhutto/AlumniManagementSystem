using AlumniManagementSystem.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlumniManagementSystem.Infrastructure;

namespace AlumniManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProgramsController : ControllerBase{
    private readonly ApplicationDbContext _db;
    public ProgramsController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<ProgramDto>>> GetAll(){
        var programs = await _db.Programs
            .Select(p => new ProgramDto
            {
                ProgramId = p.ProgramId,
                Name = p.Name,
                Type = p.Type
            })
            .ToListAsync();
        return Ok(programs);
    }
}
