using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class Discipline
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(30)] public string Code { get; set; } = string.Empty;
    [MaxLength(120)] public string Name { get; set; } = string.Empty;
    public ICollection<ClassRoom> Classes { get; set; } = new List<ClassRoom>();
}
