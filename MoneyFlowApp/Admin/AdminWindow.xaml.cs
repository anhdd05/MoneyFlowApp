using System.Windows;
using System.Windows.Controls;

namespace MoneyFlowApp.Admin
{
    public partial class AdminWindow : Window
    {
            public AdminWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => RefreshAll();
        }

        private void AdminTabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            RefreshAll();
        }

        private void RefreshAll()
        {
            AdminDashboard?.LoadData();
            AdminCategory?.LoadAll();
            AdminUser?.LoadUsers();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SessionManager.Logout();
                var loginWin = new LoginWindow();
                loginWin.Show();
                this.Close();
            }
        }
    }
}
