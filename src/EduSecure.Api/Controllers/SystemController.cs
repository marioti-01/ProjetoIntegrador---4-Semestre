using Microsoft.AspNetCore.Mvc;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("info")]
    public IActionResult Info() => Ok(new { name = "EduSecure 360", environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production", utc = DateTime.UtcNow });
}
