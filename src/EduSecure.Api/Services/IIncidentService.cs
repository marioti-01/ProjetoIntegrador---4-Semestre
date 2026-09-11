using EduSecure.Api.Models;
namespace EduSecure.Api.Services;
public interface IIncidentService
{
    Task<Incident> CreateAsync(string type, string severity, string description, string source);
    Task ResolveAsync(Guid id);
}
