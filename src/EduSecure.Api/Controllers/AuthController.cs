using System.Security.Claims;
using EduSecure.Api.DTOs;
using EduSecure.Api.Repositories;
using EduSecure.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EduSecure.Api.Controllers;
[ApiController, Route("api/auth")]
public class AuthController(IAuthService auth, IUserRepository users) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var forwarded = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        var ip = !string.IsNullOrWhiteSpace(forwarded) ? forwarded.Split(',')[0].Trim() : (HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
        var result = await auth.LoginAsync(request, ip);
        return result.StatusCode == 200 ? Ok(result.Response) : StatusCode(result.StatusCode, new { message = result.Error });
    }

    [Authorize, HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)) return Unauthorized();
        var user = await users.FindByIdAsync(id);
        return user is null ? NotFound() : Ok(new { user.Id, user.Name, user.Email, user.Role, user.Active, user.LockedUntilUtc });
    }
}
