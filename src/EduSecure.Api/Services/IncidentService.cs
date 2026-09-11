using EduSecure.Api.Data;
using EduSecure.Api.Metrics;
using EduSecure.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Services;
public class IncidentService(EduSecureContext db) : IIncidentService
{
    public async Task<Incident> CreateAsync(string type, string severity, string description, string source)
    {
        var incident = new Incident { Type = type, Severity = severity, Description = description, Source = source };
        db.Incidents.Add(incident);
        await db.SaveChangesAsync();
        EduMetrics.Incidents.WithLabels(type, severity).Inc();
        EduMetrics.ActiveIncidents.Set(await db.Incidents.CountAsync(x => x.Status != "Resolvido"));
        return incident;
    }
    public async Task ResolveAsync(Guid id)
    {
        var incident = await db.Incidents.FindAsync(id) ?? throw new KeyNotFoundException("Incidente não encontrado.");
        incident.Status = "Resolvido";
        incident.ResolvedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync();
        EduMetrics.ActiveIncidents.Set(await db.Incidents.CountAsync(x => x.Status != "Resolvido"));
    }
}
