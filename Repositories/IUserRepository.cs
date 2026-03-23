using BusinessObjects;

namespace Repositories;

public interface IUserRepository
{
    User? Login(string email, string password);
    User? GetByEmail(string email);
    void Add(User user);
    void Update(User user);
    User? GetByResetToken(string token);
}