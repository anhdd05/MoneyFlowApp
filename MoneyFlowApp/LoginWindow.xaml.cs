using BusinessObjects;
using Services;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Data.SqlClient;

namespace MoneyFlowApp;

public partial class LoginWindow : Window
{
    private readonly UserService userService = new UserService();
    private readonly AdminService adminService = new AdminService();

    public User? LoggedInUser { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
        TxtEmail.Text = "admin@app.com"; // demo prefill
    }

    private async void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        TxtError.Visibility = Visibility.Collapsed;
        string email = TxtEmail.Text?.Trim() ?? "";
        string password = TxtPassword.Password ?? "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            TxtError.Text = "Vui lòng nhập email và mật khẩu.";
            TxtError.Visibility = Visibility.Visible;
            return;
        }

        BtnLogin.IsEnabled = false;
        BtnCancel.IsEnabled = false;
        BtnLogin.Content = "Đang xử lý...";

        try
        {
            User? user = await Task.Run(() => userService.Login(email, password));

            await Dispatcher.InvokeAsync(() =>
            {
                if (user == null)
                {
                    TxtError.Text = "Email hoặc mật khẩu không đúng.";
                    TxtError.Visibility = Visibility.Visible;
                    ResetButtons();
                    return;
                }

                if (user.IsBanned)
                {
                    TxtError.Text = "Tài khoản đã bị khóa. Liên hệ quản trị viên.";
                    TxtError.Visibility = Visibility.Visible;
                    ResetButtons();
                    return;
                }

                Task.Run(() => adminService.UpdateLastLogin(user.UserId));
                user.LastLoginAt = DateTime.Now;
                LoggedInUser = user;
                DialogResult = true;
                Close();
            });
        }
        catch (SqlException ex)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                TxtError.Text = "Lỗi kết nối CSDL. Kiểm tra SQL Server đã chạy, database đã tạo chưa.\n" + ex.Message;
                TxtError.Visibility = Visibility.Visible;
                ResetButtons();
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                TxtError.Text = "Lỗi: " + ex.Message;
                TxtError.Visibility = Visibility.Visible;
                ResetButtons();
            });
        }
    }

    private void ResetButtons()
    {
        BtnLogin.IsEnabled = true;
        BtnCancel.IsEnabled = true;
        BtnLogin.Content = "Đăng nhập";
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
