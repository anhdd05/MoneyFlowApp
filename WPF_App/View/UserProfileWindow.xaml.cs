using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPF_App.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserProfileWindow : Window
    {
        private readonly IUserService _userService = new UserService();

        public UserProfileWindow()
        {
            InitializeComponent();
            LoadProfile();
            ShowViewMode(); // Mặc định mở ra là View mode
        }

        // ══ LOAD THÔNG TIN ══════════════════════════
        private void LoadProfile()
        {
            var user = SessionManager.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Phiên đăng nhập hết hạn!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                GoToLogin();
                return;
            }

            lblName.Text = user.UserName;
            lblEmail.Text = user.Email;
            lblRole.Text = user.Role == "admin" ? "Quản trị viên" : "Người dùng";
            lblCreatedAt.Text = user.CreatedAt?.ToString("dd/MM/yyyy") ?? "N/A";
        }

        // ══ CHUYỂN MODE ═════════════════════════════
        private void ShowViewMode()
        {
            pnlView.Visibility = Visibility.Visible;
            pnlEdit.Visibility = Visibility.Collapsed;
            lblTitle.Text = "Hồ sơ cá nhân";
        }

        private void ShowEditMode()
        {
            pnlView.Visibility = Visibility.Collapsed;
            pnlEdit.Visibility = Visibility.Visible;
            lblTitle.Text = "Chỉnh sửa hồ sơ";

            // Điền tên hiện tại vào ô nhập
            txtName.Text = SessionManager.CurrentUser!.UserName;
        }

        // ══ SỰ KIỆN NÚT ════════════════════════════
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
            => ShowEditMode();

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => ShowViewMode();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var (success, message) = _userService.UpdateProfile(
                txtName.Text,
                SessionManager.CurrentUser!.UserId
            );

            if (!success)
            {
                MessageBox.Show(message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cập nhật lại session và View
            SessionManager.CurrentUser!.UserName = txtName.Text.Trim();
            lblName.Text = txtName.Text.Trim();

            MessageBox.Show(message, "Thành công",
                MessageBoxButton.OK, MessageBoxImage.Information);

            ShowViewMode(); // Quay về View sau khi lưu
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất không?",
                "Xác nhận", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            SessionManager.Logout();
            GoToLogin();
        }

        private void GoToLogin()
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
