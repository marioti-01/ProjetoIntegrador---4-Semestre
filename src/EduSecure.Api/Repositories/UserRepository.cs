using EduSecure.Api.Data;
using EduSecure.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace EduSecure.Api.Repositories;
public class UserRepository(EduSecureContext db) : IUserRepository
{
    public Task<User?> FindByEmailAsync(string email) => db.Users
        .Include(x => x.Student).Include(x => x.Professor)
        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    public Task<User?> FindByIdAsync(Guid id) => db.Users
        .Include(x => x.Student).Include(x => x.Professor)
        .FirstOrDefaultAsync(x => x.Id == id);
    public Task<List<User>> ListAsync() => db.Users.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
