using MoneyFlowApp.View;
using Service;
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

namespace MoneyFlowApp.DashBoard
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private readonly int _currentUserId;
        private readonly TransactionService _transService;
        private readonly BudgetService _budgetService;
        public DashboardWindow(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            // Khởi tạo các Service của bạn
            _transService = new TransactionService();
            _budgetService = new BudgetService();

            LoadDashboardData();
        }
        private void LoadDashboardData()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            // 1. XỬ LÝ SỐ LIỆU TỔNG QUAN (THU / CHI / SỐ DƯ)
            var transactions = _transService.GetByMonth(_currentUserId, currentMonth, currentYear);

            // Tính tổng thu chi (Cần đảm bảo Repo đã Include(t => t.Category) để lấy được Type)
            decimal totalIncome = transactions
                .Where(t => t.Category != null && t.Category.Type.ToLower() == "income")
                .Sum(t => t.Amount);

            decimal totalExpense = transactions
                .Where(t => t.Category != null && t.Category.Type.ToLower() == "expense")
                .Sum(t => t.Amount);

            // Gọi hàm tính Balance của bạn
            decimal balance = _budgetService.GetBalance(_currentUserId, currentMonth, currentYear);

            TxtTotalIncome.Text = $"{totalIncome:N0} đ";
            TxtTotalExpense.Text = $"{totalExpense:N0} đ";
            TxtBalance.Text = $"{balance:N0} đ";

            // 2. XỬ LÝ WIDGET CẢNH BÁO NGÂN SÁCH
            var budgets = _budgetService.GetBudgets(_currentUserId, currentMonth, currentYear);
            var alerts = new List<string>();

            foreach (var budget in budgets)
            {
                // Bỏ qua nếu ngân sách bằng 0 để tránh lỗi chia cho 0
                if (budget.Amount <= 0) continue;

                if (budget.Allocated > budget.Amount)
                {
                    alerts.Add($"🔴 NGUY HIỂM: Đã vượt ngân sách [{budget.Category?.Name}]. Tiêu: {budget.Allocated:N0} / Mức: {budget.Amount:N0}");
                }
                else if (budget.Allocated >= budget.Amount * 0.8m)
                {
                    alerts.Add($"🟠 CẢNH BÁO: Sắp vượt ngân sách [{budget.Category?.Name}]. Tiêu: {budget.Allocated:N0} / Mức: {budget.Amount:N0}");
                }
            }

            if (!alerts.Any())
            {
                alerts.Add("🟢 Tốt: Mọi khoản chi tiêu trong tháng đều an toàn.");
            }

            LstAlerts.ItemsSource = alerts;
        }

        // Điều hướng sang Profile
        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWin = new UserProfileWindow();
            profileWin.Show();
            this.Close();
        }

        // Điều hướng quay lại Login
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWin = new LoginWindow(); // Giả định tên màn hình Login của bạn
            loginWin.Show();
            this.Close();
        }
    }
}
    

