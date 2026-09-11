using EduSecure.Api.DTOs;
using EduSecure.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/incidentes")]
public class IncidentWebhookController(IIncidentService incidents, IConfiguration config) : ControllerBase
{
    [HttpPost("webhook")]
    public async Task<IActionResult> Receive(IncidentWebhookRequest request, [FromHeader(Name = "X-EduSecure-Webhook-Secret")] string? secret)
    {
        if (secret != config["Security:WebhookSecret"]) return Unauthorized();
        var incident = await incidents.CreateAsync(request.Type, request.Severity, request.Description, request.Source);
        return Created($"/api/admin/incidentes/{incident.Id}", incident);
    }
}
