using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static UserRepository? _instance;
        public static UserRepository Instance
            => _instance ??= new UserRepository();
        private UserRepository() { }
        public User? GetById(int id)
        {
            using var ctx = new ExpenseManagementContext();
            return ctx.Users.Find(id);
        }

        public void Update(User user)
        {
            using var ctx = new ExpenseManagementContext();
            ctx.Users.Update(user);
            ctx.SaveChanges();
        }
    }
}
