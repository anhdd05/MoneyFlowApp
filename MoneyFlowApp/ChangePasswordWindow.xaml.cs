using System;
using System.Windows;
using Services;

namespace MoneyFlowApp
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly string _userEmail;
        private readonly UserService _userService = new UserService();

        public ChangePasswordWindow(string email)
        {
            InitializeComponent();
            _userEmail = email;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtNewPass.Password != txtConfirmPass.Password)
                {
                    txtMsg.Text = "Mật khẩu xác nhận không khớp!";
                    return;
                }
                _userService.ChangePassword(_userEmail, txtOldPass.Password, txtNewPass.Password);
                MessageBox.Show("Đổi mật khẩu thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                txtMsg.Text = ex.Message;
            }
        }
    }
}