using BusinessObject.Models;
using Service;
using System.Windows;

namespace MoneyFlowApp.Admin
{
    public partial class AdminCategoryEditWindow : Window
    {
        private readonly bool isTransactionCategory;
        private readonly int? editCategoryId;

        public object? Result { get; private set; }

        public AdminCategoryEditWindow(object? category, bool isTransactionCategory)
        {
            InitializeComponent();
            this.isTransactionCategory = isTransactionCategory;

            if (isTransactionCategory)
            {
                CboType.ItemsSource = new[] { "Income", "Expense" };
                TxtTitle.Text = category == null ? "Thêm danh mục Giao dịch" : "Sửa danh mục Giao dịch";
            }
            else
            {
                CboType.ItemsSource = new[] { "Expense", "Saving" };
                TxtTitle.Text = category == null ? "Thêm danh mục Ngân sách" : "Sửa danh mục Ngân sách";
            }

            if (category is TransactionCategory tx)
            {
                editCategoryId = tx.CategoryId;
                TxtName.Text = tx.Name;
                CboType.SelectedItem = tx.Type ?? "Expense";
                TxtIcon.Text = tx.Icon ?? "";
                TxtColor.Text = tx.Color ?? "";
            }
            else if (category is BudgetCategory bc)
            {
                editCategoryId = bc.CategoryId;
                TxtName.Text = bc.Name;
                CboType.SelectedItem = bc.Type ?? "Expense";
                TxtIcon.Text = bc.Icon ?? "";
                TxtColor.Text = bc.Color ?? "";
            }
            else
            {
                editCategoryId = null;
                CboType.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click        (object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.");
                return;
            }

            string type = CboType.SelectedItem?.ToString() ?? "Expense";
            string icon = TxtIcon.Text?.Trim() ?? "";
            string color = TxtColor.Text?.Trim() ?? "";

            if (isTransactionCategory)
            {
                Result = new TransactionCategory
                {
                    CategoryId = editCategoryId ?? 0,
                    Name = name,
                    Type = type,
                    Icon = string.IsNullOrEmpty(icon) ? null : icon,
                    Color = string.IsNullOrEmpty(color) ? null : color,
                    IsDeleted = false
                };
            }
            else
            {
                Result = new BudgetCategory
                {
                    CategoryId = editCategoryId ?? 0,
                    Name = name,
                    Type = type,
                    Icon = string.IsNullOrEmpty(icon) ? null : icon,
                    Color = string.IsNullOrEmpty(color) ? null : color,
                    IsDeleted = false
                };
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
