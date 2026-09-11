using EduSecure.Api.DTOs;
namespace EduSecure.Api.Services;
public interface IAuthService
{
    Task<(LoginResponse? Response, int StatusCode, string? Error)> LoginAsync(LoginRequest request, string ipAddress);
}
