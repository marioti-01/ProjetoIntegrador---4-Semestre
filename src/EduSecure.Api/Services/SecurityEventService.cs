using EduSecure.Api.Data;
using EduSecure.Api.Metrics;
using EduSecure.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Services;
public class SecurityEventService(EduSecureContext db) : ISecurityEventService
{
    public async Task RecordAsync(string eventType, string? email, string? ip, bool success, string? details = null)
    {
        db.SecurityLogs.Add(new SecurityLog { EventType = eventType, UserEmail = email, IpAddress = ip, Success = success, Details = details });
        EduMetrics.SecurityEvents.WithLabels(eventType).Inc();
        await db.SaveChangesAsync();
    }
    public Task<int> CountRecentFailuresAsync(string email, TimeSpan window)
    {
        var since = DateTime.UtcNow.Subtract(window);
        return db.SecurityLogs.CountAsync(x => x.UserEmail == email && x.EventType == "LOGIN_FAILED" && x.CreatedAtUtc >= since);
    }
}
