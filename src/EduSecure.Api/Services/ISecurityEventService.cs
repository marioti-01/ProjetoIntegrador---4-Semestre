namespace EduSecure.Api.Services;
public interface ISecurityEventService
{
    Task RecordAsync(string eventType, string? email, string? ip, bool success, string? details = null);
    Task<int> CountRecentFailuresAsync(string email, TimeSpan window);
}
