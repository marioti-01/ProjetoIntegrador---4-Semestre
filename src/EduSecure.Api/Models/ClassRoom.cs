using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class ClassRoom
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DisciplineId { get; set; }
    public Discipline Discipline { get; set; } = null!;
    public Guid ProfessorId { get; set; }
    public Professor Professor { get; set; } = null!;
    [MaxLength(30)] public string Semester { get; set; } = "2026.2";
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
