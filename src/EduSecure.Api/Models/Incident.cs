using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class Incident
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(80)] public string Type { get; set; } = string.Empty;
    [MaxLength(20)] public string Severity { get; set; } = "Media";
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    [MaxLength(30)] public string Status { get; set; } = "Aberto";
    [MaxLength(100)] public string Source { get; set; } = "EduSecure";
    public DateTime DetectedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAtUtc { get; set; }
}
