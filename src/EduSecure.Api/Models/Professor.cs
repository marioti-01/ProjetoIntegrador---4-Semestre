using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class Professor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    [MaxLength(30)] public string EmployeeNumber { get; set; } = string.Empty;
    public ICollection<ClassRoom> Classes { get; set; } = new List<ClassRoom>();
}
