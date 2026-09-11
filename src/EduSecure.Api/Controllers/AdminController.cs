using EduSecure.Api.Data;
using EduSecure.Api.Models;
using EduSecure.Api.Repositories;
using EduSecure.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/admin"), Authorize(Roles = Roles.Administrator)]
public class AdminController(EduSecureContext db, IUserRepository users, IIncidentService incidents) : ControllerBase
{
    [HttpGet("usuarios")]
    public async Task<IActionResult> Users() => Ok((await users.ListAsync()).Select(x => new { x.Id, x.Name, x.Email, x.Role, x.Active, x.LockedUntilUtc, x.CreatedAtUtc }));

    [HttpPost("usuarios/{id:guid}/desbloquear")]
    public async Task<IActionResult> Unlock(Guid id)
    {
        var user = await users.FindByIdAsync(id);
        if (user is null) return NotFound();
        user.LockedUntilUtc = null;
        await users.SaveChangesAsync();
        return Ok(new { message = "Usuário desbloqueado." });
    }

    [HttpGet("logs")]
    public async Task<IActionResult> Logs([FromQuery] int limit = 100) => Ok(await db.SecurityLogs.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).Take(Math.Clamp(limit, 1, 500)).ToListAsync());

    [HttpGet("incidentes")]
    public async Task<IActionResult> Incidents() => Ok(await db.Incidents.AsNoTracking().OrderByDescending(x => x.DetectedAtUtc).ToListAsync());

    [HttpGet("incidentes/{id:guid}")]
    public async Task<IActionResult> Incident(Guid id)
    {
        var incident = await db.Incidents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpPost("incidentes/{id:guid}/resolver")]
    public async Task<IActionResult> Resolve(Guid id)
    {
        try { await incidents.ResolveAsync(id); return Ok(new { message = "Incidente resolvido." }); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet("resumo")]
    public async Task<IActionResult> Summary() => Ok(new
    {
        Usuarios = await db.Users.CountAsync(),
        LoginsFalhosUltimos15Min = await db.SecurityLogs.CountAsync(x => x.EventType == "LOGIN_FAILED" && x.CreatedAtUtc >= DateTime.UtcNow.AddMinutes(-15)),
        IncidentesAbertos = await db.Incidents.CountAsync(x => x.Status != "Resolvido"),
        EventosSegurancaHoje = await db.SecurityLogs.CountAsync(x => x.CreatedAtUtc >= DateTime.UtcNow.Date)
    });
}
