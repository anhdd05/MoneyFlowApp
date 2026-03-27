using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        User? Login(string email, string password);
        User? GetById(int id);
        void Update(User user);
    }
}
