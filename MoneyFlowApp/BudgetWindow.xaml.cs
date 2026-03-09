using System.Windows;
using BusinessObjects;
using Service;

namespace MoneyFlowApp
{

    public partial class BudgetWindow : Window
    {
        private readonly Budget? editing;
        private readonly int userId;
        private readonly int month;
        private readonly int year;
        private readonly BudgetService budgetService;
        public BudgetWindow(Budget? editing, int userId, int month, int year, BudgetService budgetService)
        {
            InitializeComponent();
            this.editing = editing;
            this.userId = userId;
            this.month = month;
            this.year = year;
            this.budgetService = budgetService;
            CboCategory.ItemsSource = budgetService.GetCategories();
            if (editing == null) TxtTitle.Text = "Thêm ngân sách mới";
            else
            {
                TxtTitle.Text = "Chỉnh sửa ngân sách";
                TxtAmount.Text = editing.Amount.ToString("N0");
                TxtDescription.Text = editing.Description ?? "";
                ChkAutoRenew.IsChecked = editing.AutoRenew ?? false;

                CboCategory.SelectedItem = budgetService.GetCategories().FirstOrDefault(c => c.CategoryId == editing.CategoryId);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CboCategory.SelectedItem is not BudgetCategory category)
            {
                TxtError.Text = "Vui lòng chọn một loại ngân sách!";
                return;
            }
            string amount = TxtAmount.Text.Trim();
            if (!decimal.TryParse(amount, out decimal amountNum) || amountNum <= 0)
            {
                TxtError.Text = "Số tiền phải là số dương hợp lệ!";
                return;
            }
            string? description = string.IsNullOrEmpty(TxtDescription.Text.Trim()) ? "None" : TxtDescription.Text.Trim();
            bool autoRenew = ChkAutoRenew.IsChecked ?? false;
            if (editing == null)
            {
                budgetService.AddBudget(userId, category.CategoryId, amountNum, description, autoRenew, month, year);
            }
            else
            {
                budgetService.UpdateBudget(editing.BudgetId, category.CategoryId, amountNum, description, autoRenew);
            }
            this.Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
