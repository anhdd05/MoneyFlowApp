using System.Windows;
using BusinessObjects;
using Service;

namespace MoneyFlowApp
{

    public partial class AllocateWindow : Window
    {
        private readonly Budget budget;
        private readonly int userId;
        private readonly BudgetService budgetService;
        private readonly decimal balance;
        private readonly bool isSaving;
        public AllocateWindow(Budget budget, int userId, BudgetService budgetService, decimal balance)
        {
            InitializeComponent();
            this.budget = budget;
            this.userId = userId;
            this.budgetService = budgetService;
            this.balance = balance;
            this.isSaving = budget.Category?.Type!.ToLower() == "saving";
            LoadUI();
        }

        private void LoadUI()
        {
            TxtBudgetName.Text = budget.Category?.Name ?? "Ko xác định";
            decimal remaining = budget.Amount - budget.Allocated;
            TxtSummary.Text = $"Còn lại: {remaining:N0} / {budget.Amount:N0}₫";
            DpDate.SelectedDate = DateTime.Today;
            if (isSaving)
            {
                LblTxCategory.Visibility = Visibility.Collapsed;
                CboTxCategory.Visibility = Visibility.Collapsed;
                LblDate.Visibility = Visibility.Collapsed;
                DpDate.Visibility = Visibility.Collapsed;
                TxtAvailable.Text = $"Số dư khả dụng: {balance:N0}₫";
                TxtAvailable.Visibility = Visibility.Visible;
            }
            else
            {
                var txService = new TransactionService();
                CboTxCategory.ItemsSource = txService.GetCategories()
                .Where(c => c.Type?.ToLower() == "expense")
                .ToList();
                CboTxCategory.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string amount = TxtAmount.Text.Trim();
            if (!decimal.TryParse(amount, out decimal amountNum) || amountNum <= 0)
            {
                TxtError.Text = "Số tiền phải là số dương hợp lệ!";
                return;
            }
            if (isSaving)
            {
                if (amountNum > balance)
                {
                    TxtError.Text = "Số tiền vượt quá số dư khả dụng!";
                    return;
                }
                budgetService.AllocateSaving(budget.BudgetId, amountNum);
            }
            else
            {
                if (CboTxCategory.SelectedItem is not TransactionCategory txCat)
                {
                    TxtError.Text = "Vui lòng chọn một loại giao dịch!";
                    return;
                }
                if (DpDate.SelectedDate == null)
                {
                    TxtError.Text = "Vui lòng chọn ngày!";
                    return;
                }
                budgetService.AllocateExpense(budget.BudgetId, txCat.CategoryId, amountNum, DateOnly.FromDateTime(DpDate.SelectedDate.Value), TxtNote.Text.Trim(), userId);
            }
            this.Close();

        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    } 
}
