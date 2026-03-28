using BusinessObject.Models;
using MoneyFlowApp.Admin;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System;

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
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            try
            {
                // --- CHỨC NĂNG VALIDATE CHUNG ---

                // 1. Kiểm tra trống Email
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Focus();
                    throw new Exception("Vui lòng nhập Email!");
                }

                // 2. Kiểm tra định dạng Email
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    txtEmail.Focus();
                    throw new Exception("Định dạng Email không hợp lệ (ví dụ: abc@gmail.com)!");
                }

                // 3. Kiểm tra trống Mật khẩu
                if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    txtPassword.Focus();
                    throw new Exception("Vui lòng nhập Mật khẩu!");
                }

                // --- XỬ LÝ THEO TỪNG CHẾ ĐỘ (MODE) ---

                if (mode == "ĐĂNG NHẬP")
                {
                    var user = _userService.Login(txtEmail.Text, txtPassword.Password);

                    if (user != null)
                    {
                        string displayName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Email;

                        if (user.Role != null && user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show($"Đăng nhập thành công! Chào Admin: {displayName}.", "Thông báo");
                            AdminWindow adminWin = new AdminWindow();
                            adminWin.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Đăng nhập thành công! Chào mừng {displayName} quay trở lại.", "Thông báo");
                            AuthWindow auth = new AuthWindow(user.UserId);
                            auth.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        throw new Exception("Email hoặc mật khẩu không chính xác!");
                    }
                }
                else if (mode == "XÁC NHẬN ĐĂNG KÝ")
                {
                    // Kiểm tra thêm Username khi Đăng ký
                    if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    {
                        txtUsername.Focus();
                        throw new Exception("Vui lòng nhập Họ tên (Username) để đăng ký!");
                    }

                    _userService.Register(txtUsername.Text, txtEmail.Text, txtPassword.Password);
                    MessageBox.Show("Đăng ký thành công! Hãy đăng nhập lại bằng tài khoản mới.", "Thành công");
                    btnBackToLogin_Click(null, null);
                }
                else if (mode == "ĐẶT LẠI MẬT KHẨU")
                {
                    if (string.IsNullOrEmpty(txtToken.Text))
                    {
                        txtToken.Focus();
                        throw new Exception("Vui lòng nhập mã Token (6 số) đã gửi về Email!");
                    }

                    _userService.ResetPassword(txtEmail.Text, txtToken.Text, txtPassword.Password);
                    MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.", "Thông báo");
                    btnBackToLogin_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                txtMessage.Foreground = Brushes.Red;
            }
        }

        private async void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate Email trước khi gửi mã
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Focus();
                    throw new Exception("Nhập Email của bạn trước khi bấm Quên mật khẩu!");
                }

                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                {
                    txtEmail.Focus();
                    throw new Exception("Vui lòng nhập đúng định dạng Email để nhận mã xác nhận!");
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