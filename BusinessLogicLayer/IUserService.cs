
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
namespace Services
{
    public interface IUserService
    {
        User? GetById(int id);
        (bool success, string message) UpdateProfile(string newName, int userId);
    }
}
