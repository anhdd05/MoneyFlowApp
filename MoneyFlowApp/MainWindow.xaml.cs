using BusinessObjects;
using Service;
using System.Windows;
using System.Windows.Media;

namespace MoneyFlowApp;

public partial class MainWindow : Window
{
    private readonly BudgetService budgetService = new BudgetService();
    private readonly TransactionService transactionService = new TransactionService();

    private readonly int userId;
    private int currentMonth = DateTime.Today.Month;
    private int currentYear = DateTime.Today.Year;

    public MainWindow(int userId)
    {
        InitializeComponent();
        this.userId = userId;
        LoadAll();
    }

    private void LoadAll()
    {
        LoadHeader();
        LoadBudgets();
        LoadTransactions();
    }

    private void LoadHeader()
    {
        var date = new DateTime(currentYear, currentMonth, 1);
        TxtMonth.Text = date.ToString("MMMM yyyy");

        decimal balance = budgetService.GetBalance(userId, currentMonth, currentYear);
        TxtBalance.Text = balance.ToString("N0") + "₫";
        TxtBalance.Foreground = balance >= 0 ? Brushes.White : Brushes.LightCoral;
    }

    private void LoadBudgets()
    {
        List<Budget> budgets = budgetService.GetBudgets(userId, currentMonth, currentYear);

        BudgetGrid.ItemsSource = budgets.Select(b => new
        {
            b.BudgetId,
            b.Category,
            b.Amount,
            b.Allocated,
            Remaining = b.Amount - b.Allocated,
            b.Description,
            b.AutoRenew,
        }).ToList();

        int completed = budgets.Count(b => b.Allocated >= b.Amount && b.Amount > 0);
        TxtBudgetSummary.Text = $"Tổng: {budgets.Count} ngân sách  |  Hoàn thành: {completed}/{budgets.Count}";
    }

    private void LoadTransactions()
    {
        TransactionGrid.ItemsSource =
            transactionService.GetByMonth(userId, currentMonth, currentYear);
    }


    private void BtnPrevMonth_Click(object sender, RoutedEventArgs e)
    {
        var prev = new DateTime(currentYear, currentMonth, 1).AddMonths(-1);
        currentMonth = prev.Month;
        currentYear = prev.Year;
        LoadAll();
    }

    private void BtnNextMonth_Click(object sender, RoutedEventArgs e)
    {
        var next = new DateTime(currentYear, currentMonth, 1).AddMonths(1);
        var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        if (next > thisMonth) return;

        currentMonth = next.Month;
        currentYear = next.Year;
        LoadAll();
    }

    private void BtnAddBudget_Click(object sender, RoutedEventArgs e)
    {
        var popup = new BudgetWindow(null, userId, currentMonth, currentYear, budgetService);
        popup.Owner = this;
        popup.Show();
        LoadAll();
    }

    private void BtnEditBudget_Click(object sender, RoutedEventArgs e)
    {
        if (BudgetGrid.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn một ngân sách để chỉnh sửa.");
            return;
        }

        dynamic selected = BudgetGrid.SelectedItem;
        int budgetId = (int)selected.BudgetId;

        Budget? budget = budgetService.GetById(budgetId);
        if (budget == null) return;

        var popup = new BudgetWindow(budget, userId, currentMonth, currentYear, budgetService);
        popup.Owner = this;
        popup.Show();
        LoadAll();
    }

    private void BtnDeleteBudget_Click(object sender, RoutedEventArgs e)
    {
        if (BudgetGrid.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn một ngân sách để xóa.");
            return;
        }
        var result = MessageBox.Show("Bạn có muốn xóa ngân sách này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;
        dynamic selected = BudgetGrid.SelectedItem;
        int budgetId = (int)selected.BudgetId;
        budgetService.DeleteBudget(budgetId);
        LoadAll();
    }

    private void BtnAllocate_Click(object sender, RoutedEventArgs e)
    {
        if (BudgetGrid.SelectedItem == null)
        {
            MessageBox.Show("Vui lòng chọn một ngân sách để phân bổ tiền.");
            return;
        }

        dynamic selected = BudgetGrid.SelectedItem;
        int budgetId = (int)selected.BudgetId;

        Budget? budget = budgetService.GetById(budgetId);
        if (budget == null) return;
        decimal balance = budgetService.GetBalance(userId, currentMonth, currentYear);

        var popup = new AllocateWindow(budget, userId, budgetService, balance);
        popup.Owner = this;
        popup.Show();
        LoadAll();
    }

    private void BtnAddTransaction_Click(object sender, RoutedEventArgs e)
    {
        var popup = new TransactionWindow(userId, transactionService);
        popup.Owner = this;
        popup.Show();
        LoadAll();
    }

    private void BtnDeleteTransaction_Click(object sender, RoutedEventArgs e)
    {
        if (TransactionGrid.SelectedItem is not Transaction tx)
        {
            MessageBox.Show("Vui lòng chọn một giao dịch để xóa.");
            return;
        }
        var result = MessageBox.Show("Bạn có muốn xóa giao dịch này?","Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;
        transactionService.DeleteTransaction(tx.TransactionId);
        LoadAll();
    }
}