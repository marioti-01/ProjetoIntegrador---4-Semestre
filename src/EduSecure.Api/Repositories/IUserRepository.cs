using EduSecure.Api.Models;
namespace EduSecure.Api.Repositories;
public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task<List<User>> ListAsync();
    Task SaveChangesAsync();
}
