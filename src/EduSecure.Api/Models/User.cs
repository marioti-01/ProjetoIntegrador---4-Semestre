using System.ComponentModel.DataAnnotations;

namespace EduSecure.Api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(150)] public string Email { get; set; } = string.Empty;
    [MaxLength(100)] public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(30)] public string Role { get; set; } = Roles.Student;
    public bool Active { get; set; } = true;
    public DateTime? LockedUntilUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Student? Student { get; set; }
    public Professor? Professor { get; set; }
}

public static class Roles
{
    public const string Student = "Aluno";
    public const string Professor = "Professor";
    public const string Administrator = "Administrador";
}
