using BusinessObject.Models;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlowApp.Admin
{
    public partial class AdminUserControl : UserControl
    {
        private readonly AdminService adminService = new AdminService();

        public AdminUserControl()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadUsers();
        }

        public void LoadUsers()
        {
            string? search = string.IsNullOrWhiteSpace(TxtSearch.Text) ? null : TxtSearch.Text.Trim();
            string? status = CboStatus.SelectedIndex switch
            {
                1 => "active",
                2 => "banned",
                3 => "unverified",
                _ => null
            };

            var list = adminService.GetUsers(search, status);
            UserGrid.ItemsSource = list.Select(u => new
            {
                u.UserId,
                u.UserName,
                u.Email,
                u.Role,
                StatusLabel = u.IsBanned ? "Bị khóa" : (u.LastLoginAt == null ? "Chưa xác minh" : "Active"),
                CreatedAtStr = u.CreatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                LastLoginStr = u.LastLoginAt?.ToString("dd/MM/yyyy HH:mm") ?? "-"
            }).ToList();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) => LoadUsers();
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadUsers();

        private void BtnBan_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null) { MessageBox.Show("Chọn người dùng cần khóa."); return; }
            dynamic sel = UserGrid.SelectedItem;
            int id = (int)sel.UserId;
            if (MessageBox.Show($"Khóa tài khoản của {sel.Email}?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                adminService.BanUser(id);
                LoadUsers();
            }
        }

        private void BtnUnban_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null) { MessageBox.Show("Chọn người dùng cần mở khóa."); return; }
            dynamic sel = UserGrid.SelectedItem;
            int id = (int)sel.UserId;
            adminService.UnbanUser(id);
            LoadUsers();
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem == null) { MessageBox.Show("Chọn người dùng để xem chi tiết."); return; }
            dynamic sel = UserGrid.SelectedItem;
            int id = (int)sel.UserId;
            var users = adminService.GetUsers(null, null);
            var u = users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return;

            string msg = $"--- Thông tin Metadata (Admin) ---\n\n" +
                $"ID: {u.UserId}\n" +
                $"Họ tên: {u.UserName}\n" +
                $"Email: {u.Email}\n" +
                $"Role: {u.Role}\n" +
                $"Ngày tạo: {u.CreatedAt:dd/MM/yyyy HH:mm}\n" +
                $"Lần đăng nhập cuối: {(u.LastLoginAt.HasValue ? u.LastLoginAt.Value.ToString("dd/MM/yyyy HH:mm") : "Chưa đăng nhập")}\n" +
                $"Trạng thái: {(u.IsBanned ? "Bị khóa" : "Hoạt động")}\n\n" +
                "(Không hiển thị số dư hay chi tiết giao dịch - Data Masking)";
            MessageBox.Show(msg, "Chi tiết người dùng");
        }
    }
}
