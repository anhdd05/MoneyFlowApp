using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo = UserRepository.Instance;

        // ── Lấy thông tin user theo ID ────────────────────────────
        public User? GetById(int id) => _repo.GetById(id);

        // ── Cập nhật hồ sơ cá nhân ───────────────────────────────
        public (bool success, string message) UpdateProfile(string newName, int userId)
        {
            // Validate tên
            if (string.IsNullOrWhiteSpace(newName))
                return (false, "Tên không được để trống!");

            if (newName.Trim().Length > 50)
                return (false, "Tên không được quá 50 ký tự!");

            // Lấy user từ DB
            var user = _repo.GetById(userId);
            if (user == null)
                return (false, "Không tìm thấy người dùng!");

            // Cập nhật tên
            user.UserName = newName.Trim();

            _repo.Update(user);
            return (true, "Cập nhật thành công!");
        }
    }
}
