using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyFlowApp
{
    public static class SessionManager
    {
        // Lưu user đang đăng nhập
        public static User? CurrentUser { get; set; }

        // Kiểm tra đã đăng nhập chưa
        public static bool IsLoggedIn => CurrentUser != null;

        // Kiểm tra có phải admin không
        public static bool IsAdmin => CurrentUser?.Role == "admin";

        // Đăng xuất → xóa session
        public static void Logout() => CurrentUser = null;
    }
}
