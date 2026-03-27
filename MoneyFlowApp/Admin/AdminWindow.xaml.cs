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
    }
}
