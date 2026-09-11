using System.ComponentModel.DataAnnotations;
namespace EduSecure.Api.DTOs;
public record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
public record LoginResponse(string Token, DateTime ExpiresAtUtc, object User);
