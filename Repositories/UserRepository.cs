using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

using DataAccessLayer;
namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private static UserRepository? _instance;
        public static UserRepository Instance
            => _instance ??= new UserRepository();
        public UserRepository() { }
        public User? GetById(int id)
        {
            using var ctx = new AppDbContext();
            return ctx.Users.Find(id);
        }
        public User? GetByEmail(string email)
        {
            using var db = new AppDbContext();
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? Login(string email, string password)
        {
            using var db = new AppDbContext();
            return db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            throw new NotImplementedException();
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
}