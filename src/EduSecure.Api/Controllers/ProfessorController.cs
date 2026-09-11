using System.Security.Claims;
using EduSecure.Api.Data;
using EduSecure.Api.DTOs;
using EduSecure.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/professores"), Authorize(Roles = Roles.Professor)]
public class ProfessorController(EduSecureContext db) : ControllerBase
{
    [HttpGet("me/turmas")]
    public async Task<IActionResult> Classes()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var classes = await db.Classes.AsNoTracking().Where(x => x.Professor.UserId == userId)
            .Select(x => new { x.Id, Disciplina = x.Discipline.Name, x.Discipline.Code, x.Semester, Alunos = x.Enrollments.Count }).ToListAsync();
        return Ok(classes);
    }

    [HttpGet("me/alunos")]
    public async Task<IActionResult> Students()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var rows = await db.Enrollments.AsNoTracking().Where(x => x.ClassRoom.Professor.UserId == userId)
            .Select(x => new { EnrollmentId = x.Id, Aluno = x.Student.User.Name, RA = x.Student.RegistrationNumber, Disciplina = x.ClassRoom.Discipline.Name, Nota = x.Grade != null ? x.Grade.Value : (decimal?)null }).ToListAsync();
        return Ok(rows);
    }

    [HttpPut("notas/{enrollmentId:guid}")]
    public async Task<IActionResult> UpdateGrade(Guid enrollmentId, UpdateGradeRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var enrollment = await db.Enrollments.Include(x => x.Grade).Include(x => x.ClassRoom).ThenInclude(x => x.Professor)
            .FirstOrDefaultAsync(x => x.Id == enrollmentId && x.ClassRoom.Professor.UserId == userId);
        if (enrollment is null) return NotFound();
        if (enrollment.Grade is null) enrollment.Grade = new Grade { EnrollmentId = enrollment.Id, Value = request.Value };
        else { enrollment.Grade.Value = request.Value; enrollment.Grade.UpdatedAtUtc = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        return Ok(new { message = "Nota atualizada.", request.Value });
    }
}
