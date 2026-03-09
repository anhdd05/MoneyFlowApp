using BusinessObjects;
using Repositories;

namespace Services;

public class UserService
{
    private readonly IUserRepository userRepo = new UserRepository();

    public User? Login(string email, string password) => userRepo.Login(email, password);
}