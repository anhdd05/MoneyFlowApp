using System;
using System.Windows;
using System.Windows.Media;
using System.Text.RegularExpressions; // Thư viện cần thiết để dùng Regex
using Services;
using BusinessObjects;

namespace MoneyFlowApp
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService = new UserService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = "";
            string mode = btnMainAction.Content.ToString();

            try
            {
                // 1. Kiểm tra trống (Cho tất cả các chế độ)
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
                {
                    throw new Exception("Vui lòng nhập đầy đủ Email và Mật khẩu!");
                }

                // 2. Kiểm tra định dạng Email bằng Regex
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    throw new Exception("Định dạng Email không hợp lệ (ví dụ: abc@gmail.com)!");
                }

                if (mode == "ĐĂNG NHẬP")
                {
                    var user = _userService.Login(txtEmail.Text, txtPassword.Password);

                    MessageBox.Show($"Đăng nhập thành công! Chào {user.Email}", "Thông báo");

                    ChangePasswordWindow changeWin = new ChangePasswordWindow(user.Email);
                    this.Hide();

                    changeWin.ShowDialog();

                    this.Show();
                    txtPassword.Password = "";
                    txtMessage.Text = "Vui lòng đăng nhập lại với mật khẩu mới.";
                }
                else if (mode == "XÁC NHẬN ĐĂNG KÝ")
                {
                   
                    _userService.Register(txtUsername.Text, txtEmail.Text, txtPassword.Password);

                    MessageBox.Show("Đăng ký thành công! Hãy đăng nhập lại bằng tài khoản mới.", "Thành công");
                    btnBackToLogin_Click(null, null);
                }
                else if (mode == "ĐẶT LẠI MẬT KHẨU")
                {
                    if (string.IsNullOrEmpty(txtToken.Text)) throw new Exception("Vui lòng nhập mã Token từ Email!");

                    _userService.ResetPassword(txtEmail.Text, txtToken.Text, txtPassword.Password);

                    MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.", "Thông báo");
                    btnBackToLogin_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                // Tô đỏ thông báo lỗi cho dễ nhìn
                txtMessage.Foreground = Brushes.Red;
            }
        }

        private async void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    txtMessage.Text = "Nhập Email trước khi bấm Quên mật khẩu!";
                    txtMessage.Foreground = Brushes.OrangeRed;
                    return;
                }

                // Kiểm tra định dạng email cả khi bấm quên mật khẩu
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    throw new Exception("Vui lòng nhập đúng định dạng Email để nhận mã!");
                }

                txtTitle.Text = "Khôi phục mật khẩu";
                panelToken.Visibility = Visibility.Visible;
                panelUsername.Visibility = Visibility.Collapsed;
                lblPassword.Text = "Mật khẩu mới";
                btnMainAction.Content = "ĐẶT LẠI MẬT KHẨU";
                btnMainAction.Background = new SolidColorBrush(Color.FromRgb(230, 126, 34));
                btnBackToLogin.Visibility = Visibility.Visible;

                await _userService.ForgotPassword(txtEmail.Text);
                MessageBox.Show("Mã xác nhận (6 số) đã được gửi vào Email của bạn!", "Thông báo");
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                txtMessage.Foreground = Brushes.Red;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = "Tạo tài khoản mới";
            panelUsername.Visibility = Visibility.Visible;
            panelToken.Visibility = Visibility.Collapsed;
            lblPassword.Text = "Mật khẩu";
            btnMainAction.Content = "XÁC NHẬN ĐĂNG KÝ";
            btnMainAction.Background = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            btnBackToLogin.Visibility = Visibility.Visible;
            txtMessage.Text = "";
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = "Hệ thống quản lý tài chính";
            panelUsername.Visibility = Visibility.Collapsed;
            panelToken.Visibility = Visibility.Collapsed;
            lblPassword.Text = "Mật khẩu";
            btnMainAction.Content = "ĐĂNG NHẬP";
            btnMainAction.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            btnBackToLogin.Visibility = Visibility.Collapsed;
            txtMessage.Text = "";
        }
    }
}