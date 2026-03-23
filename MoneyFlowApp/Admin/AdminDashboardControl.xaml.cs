using Service;
using System.Windows;
using System.Windows.Controls;

namespace MoneyFlowApp.Admin
{
    public partial class AdminDashboardControl : UserControl
    {
        private readonly AdminService adminService = new AdminService();

        public AdminDashboardControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            var (total, active, transactions) = adminService.GetSystemMetrics();
            TxtTotalUsers.Text = total.ToString();
            TxtActiveUsers.Text = active.ToString();
            TxtTotalTransactions.Text = transactions.ToString();

            var stats = adminService.GetRegistrationStats(30);
            int max = stats.Count > 0 ? stats.Max(x => x.Count) : 1;
            var items = stats.Select(s => new
            {
                Label = s.Date.ToString("dd/MM"),
                Height = Math.Max(4, (double)s.Count / max * 120)
            }).ToList();
            ChartBars.ItemsSource = items;
        }
    }
}
