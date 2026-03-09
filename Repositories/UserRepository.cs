using BusinessObjects;
using DataAccessLayer;

namespace Repositories;

public class UserRepository : IUserRepository
{
    public User? Login(string email, string password)
    {
        using var db = new AppDbContext();
        return db.Users
            .FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}