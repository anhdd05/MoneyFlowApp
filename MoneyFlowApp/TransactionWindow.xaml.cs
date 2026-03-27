using System.Windows;
using BusinessObject.Models;
using Service;

namespace MoneyFlowApp
{

    public partial class TransactionWindow : Window
    {
        private readonly int userId;
        private readonly TransactionService transactionService;
        public TransactionWindow(int userId, TransactionService transactionService)
        {
            InitializeComponent();
            this.userId = userId;
            this.transactionService = transactionService;

            CboCategory.ItemsSource = transactionService.GetCategories();
            CboCategory.SelectedIndex = 0;
            DpDate.SelectedDate = DateTime.Today;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CboCategory.SelectedItem is not TransactionCategory category)
            {
                TxtError.Text = "Vui lòng chọn một loại giao dịch!";
                return;
            }

            string amountText = TxtAmount.Text.Replace(",", "").Trim();
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                TxtError.Text = "Số tiền phải là số dương hợp lệ.";
                return;
            }

            if (DpDate.SelectedDate == null)
            {
                TxtError.Text = "Vui lòng chọn ngày.";
                return;
            }

            transactionService.AddTransaction(
                userId,
                category.CategoryId,
                amount,
                DateOnly.FromDateTime(DpDate.SelectedDate.Value),
                TxtNote.Text.Trim());

            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
