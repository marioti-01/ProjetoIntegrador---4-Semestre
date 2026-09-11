using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.DTOs;
public record IncidentWebhookRequest(
    [Required] string Type,
    [Required] string Severity,
    [Required] string Description,
    string Source = "Alertmanager");
public record UpdateGradeRequest([Range(0, 10)] decimal Value);
