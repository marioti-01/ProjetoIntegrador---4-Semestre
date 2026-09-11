using EduSecure.Api.Models;
using EduSecure.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace EduSecure.Api.Data;

public static class DatabaseBootstrap
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EduSecureContext>();

        for (var attempt = 1; attempt <= 10; attempt++)
        {
            try
            {
                await db.Database.EnsureCreatedAsync();
                break;
            }
            catch when (attempt < 10)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        if (await db.Users.AnyAsync()) return;

        var adminUser = new User { Name = "Administrador EduSecure", Email = "admin@edusecure.local", PasswordHash = PasswordHasher.Hash("Admin@123"), Role = Roles.Administrator };
        var professorUser = new User { Name = "Prof. Marina Costa", Email = "professor@edusecure.local", PasswordHash = PasswordHasher.Hash("Professor@123"), Role = Roles.Professor };
        var studentUser = new User { Name = "Carlos Silva", Email = "aluno@edusecure.local", PasswordHash = PasswordHasher.Hash("Aluno@123"), Role = Roles.Student };

        var professor = new Professor { User = professorUser, EmployeeNumber = "PROF-001" };
        var student = new Student { User = studentUser, RegistrationNumber = "RA-2026001", Course = "Segurança Cibernética" };
        var discipline = new Discipline { Code = "SEC401", Name = "Monitoramento de Infraestrutura e Sistemas" };
        var classRoom = new ClassRoom { Discipline = discipline, Professor = professor, Semester = "2026.2" };
        var enrollment = new Enrollment { Student = student, ClassRoom = classRoom };
        var grade = new Grade { Enrollment = enrollment, Value = 8.50m };

        db.AddRange(adminUser, professorUser, studentUser, professor, student, discipline, classRoom, enrollment, grade);
        await db.SaveChangesAsync();
    }
}
