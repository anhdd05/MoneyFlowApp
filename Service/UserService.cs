using BusinessObject.Models;
using Repositories;

namespace Service;

public class UserService : IUserService
{
    private readonly IUserRepository userRepo = new UserRepository();

    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public User? Login(string email, string password) => userRepo.Login(email, password);

    public (bool success, string message) UpdateProfile(string newName, int userId)
    {
        throw new NotImplementedException();
    }
}