using BusinessObject.Models;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MoneyFlowApp.Admin
{
    public partial class AdminCategoryControl : UserControl
    {
        private readonly AdminService adminService = new AdminService();

        public AdminCategoryControl()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadAll();
        }

        public void LoadAll()
        {
            LoadTxCategories();
            LoadBudgetCategories();
        }

        private void LoadTxCategories()
        {
            var list = adminService.GetAllTransactionCategories();
            TxCategoryGrid.ItemsSource = list.Select(c => new
            {
                c.CategoryId,
                c.Name,
                c.Type,
                c.Icon,
                c.Color,
                IsActive = !(c.IsDeleted ?? false),
                IsActiveLabel = (c.IsDeleted ?? false) ? "Ẩn" : "Hiện"
            }).ToList();
        }

        private void LoadBudgetCategories()
        {
            var list = adminService.GetAllBudgetCategories();
            BudgetCategoryGrid.ItemsSource = list.Select(c => new
            {
                c.CategoryId,
                c.Name,
                c.Type,
                c.Icon,
                c.Color,
                IsActive = !(c.IsDeleted ?? false),
                IsActiveLabel = (c.IsDeleted ?? false) ? "Ẩn" : "Hiện"
            }).ToList();
        }

        private void BtnAddTxCat_Click        (object sender, RoutedEventArgs e)
        {
            var dlg = new AdminCategoryEditWindow(null, true);
            if (dlg.ShowDialog() == true && dlg.Result is TransactionCategory cat)
            {
                adminService.AddTransactionCategory(cat);
                LoadTxCategories();
            }
        }

        private void BtnEditTxCat_Click        (object sender, RoutedEventArgs e)
        {
            if (TxCategoryGrid.SelectedItem == null) { MessageBox.Show("Chọn danh mục cần sửa."); return; }
            dynamic sel = TxCategoryGrid.SelectedItem;
            var list = adminService.GetAllTransactionCategories();
            var cat = list.FirstOrDefault(c => c.CategoryId == (int)sel.CategoryId);
            if (cat == null) return;
            var dlg = new AdminCategoryEditWindow(cat, true);
            if (dlg.ShowDialog() == true && dlg.Result is TransactionCategory updated)
            {
                adminService.UpdateTransactionCategory(updated);
                LoadTxCategories();
            }
        }

        private void ToggleTxCat_Click        (object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton btn && int.TryParse(btn.Tag?.ToString(), out var id))
            {
                adminService.ToggleTransactionCategory(id);
                LoadTxCategories();
            }
        }

        private void BtnRefreshTxCat_Click(object sender, RoutedEventArgs e) => LoadTxCategories();

        private void BtnAddBudgetCat_Click        (object sender, RoutedEventArgs e)
        {
            var dlg = new AdminCategoryEditWindow(null, false);
            if (dlg.ShowDialog() == true && dlg.Result is BudgetCategory cat)
            {
                adminService.AddBudgetCategory(cat);
                LoadBudgetCategories();
            }
        }

        private void BtnEditBudgetCat_Click        (object sender, RoutedEventArgs e)
        {
            if (BudgetCategoryGrid.SelectedItem == null) { MessageBox.Show("Chọn danh mục cần sửa."); return; }
            dynamic sel = BudgetCategoryGrid.SelectedItem;
            var list = adminService.GetAllBudgetCategories();
            var cat = list.FirstOrDefault(c => c.CategoryId == (int)sel.CategoryId);
            if (cat == null) return;
            var dlg = new AdminCategoryEditWindow(cat, false);
            if (dlg.ShowDialog() == true && dlg.Result is BudgetCategory updated)
            {
                adminService.UpdateBudgetCategory(updated);
                LoadBudgetCategories();
            }
        }

        private void ToggleBudgetCat_Click        (object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton btn && int.TryParse(btn.Tag?.ToString(), out var id))
            {
                adminService.ToggleBudgetCategory(id);
                LoadBudgetCategories();
            }
        }

        private void BtnRefreshBudgetCat_Click(object sender, RoutedEventArgs e) => LoadBudgetCategories();
    }
}
