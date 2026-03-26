using BusinessObjects;
using DataAccessLayer;

namespace Repositories;

public class UserRepository : IUserRepository
{
    public User? GetByEmail(string email)
    {
        using var db = new AppDbContext();
        return db.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? Login(string email, string password)
    {
        using var db = new AppDbContext();
        return db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }

    public void Add(User user)
    {
        using var db = new AppDbContext();
        db.Users.Add(user);
        db.SaveChanges();
    }

    public void Update(User user)
    {
        using var db = new AppDbContext();
        db.Users.Update(user);
        db.SaveChanges();
    }
    public List<User> GetAll()
    {
        using var db = new AppDbContext(); // Lưu ý: Chỗ này dùng đúng tên DbContext của nhóm ông
        return db.Users.ToList();
    }
    public User? GetByResetToken(string token)
    {
        using var db = new AppDbContext();
        return db.Users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);
    }
}