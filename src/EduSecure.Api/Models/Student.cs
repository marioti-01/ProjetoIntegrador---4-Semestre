using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [MaxLength(30)] public string RegistrationNumber { get; set; } = string.Empty;
    [MaxLength(120)] public string Course { get; set; } = string.Empty;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
