using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduSecure.Api.DTOs;
using EduSecure.Api.Metrics;
using EduSecure.Api.Repositories;
using EduSecure.Api.Utils;
using Microsoft.IdentityModel.Tokens;

namespace EduSecure.Api.Services;

public class AuthService(IUserRepository users, ISecurityEventService securityEvents, IConfiguration configuration) : IAuthService
{
    public async Task<(LoginResponse? Response, int StatusCode, string? Error)> LoginAsync(LoginRequest request, string ipAddress)
    {
        var user = await users.FindByEmailAsync(request.Email);
        if (user is null || !user.Active)
        {
            EduMetrics.LoginAttempts.WithLabels("failed").Inc();
            await securityEvents.RecordAsync("LOGIN_FAILED", request.Email, ipAddress, false, "Usuário inexistente ou inativo.");
            return (null, 401, "Email ou senha inválidos.");
        }

        if (user.LockedUntilUtc is not null && user.LockedUntilUtc > DateTime.UtcNow)
        {
            EduMetrics.LoginAttempts.WithLabels("blocked").Inc();
            await securityEvents.RecordAsync("LOGIN_BLOCKED", user.Email, ipAddress, false, "Conta temporariamente bloqueada.");
            return (null, 423, $"Conta bloqueada temporariamente até {user.LockedUntilUtc:O}.");
        }

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            EduMetrics.LoginAttempts.WithLabels("failed").Inc();
            await securityEvents.RecordAsync("LOGIN_FAILED", user.Email, ipAddress, false, "Senha inválida.");

            var threshold = configuration.GetValue("Security:MaxFailedAttempts", 5);
            var windowSeconds = configuration.GetValue("Security:WindowSeconds", 60);
            var failedCount = await securityEvents.CountRecentFailuresAsync(user.Email, TimeSpan.FromSeconds(windowSeconds));
            if (failedCount >= threshold)
            {
                var lockoutSeconds = configuration.GetValue("Security:LockoutSeconds", 60);
                user.LockedUntilUtc = DateTime.UtcNow.AddSeconds(lockoutSeconds);
                await users.SaveChangesAsync();
                EduMetrics.SecurityEvents.WithLabels("ACCOUNT_LOCKED").Inc();
                await securityEvents.RecordAsync("ACCOUNT_LOCKED", user.Email, ipAddress, false, $"Bloqueio após {failedCount} falhas recentes.");
            }

            return (null, 401, "Email ou senha inválidos.");
        }

        user.LockedUntilUtc = null;
        await users.SaveChangesAsync();
        EduMetrics.LoginAttempts.WithLabels("success").Inc();
        await securityEvents.RecordAsync("LOGIN_SUCCESS", user.Email, ipAddress, true, $"Perfil: {user.Role}");

        var expires = DateTime.UtcNow.AddHours(4);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"], audience: configuration["Jwt:Audience"],
            claims: claims, expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (new LoginResponse(tokenString, expires, new { user.Id, user.Name, user.Email, user.Role }), 200, null);
    }
}
