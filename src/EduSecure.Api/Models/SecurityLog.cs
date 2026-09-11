using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.Models;
public class SecurityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(80)] public string EventType { get; set; } = string.Empty;
    [MaxLength(150)] public string? UserEmail { get; set; }
    [MaxLength(80)] public string? IpAddress { get; set; }
    public bool Success { get; set; }
    [MaxLength(500)] public string? Details { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
