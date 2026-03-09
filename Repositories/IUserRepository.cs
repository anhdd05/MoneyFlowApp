using BusinessObjects;

namespace Repositories
{
    public interface IUserRepository
    {
        User? Login(string email, string password);
    }
}
