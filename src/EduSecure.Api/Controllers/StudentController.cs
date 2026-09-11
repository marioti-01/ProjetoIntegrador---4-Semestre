using System.Security.Claims;
using EduSecure.Api.Data;
using EduSecure.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/alunos"), Authorize(Roles = Roles.Student)]
public class StudentController(EduSecureContext db) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var student = await db.Students.AsNoTracking().Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);
        return student is null ? NotFound() : Ok(new { student.User.Name, student.User.Email, student.RegistrationNumber, student.Course });
    }

    [HttpGet("me/notas")]
    public async Task<IActionResult> Grades()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var items = await db.Enrollments.AsNoTracking()
            .Where(x => x.Student.UserId == userId)
            .Select(x => new { x.Id, Disciplina = x.ClassRoom.Discipline.Name, Codigo = x.ClassRoom.Discipline.Code, x.ClassRoom.Semester, Nota = x.Grade != null ? x.Grade.Value : (decimal?)null })
            .ToListAsync();
        return Ok(items);
    }
}
