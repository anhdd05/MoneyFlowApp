using Repositories;
using BusinessObjects;

namespace Service
{
    public class AdminService
    {
        private readonly IAdminRepository adminRepo = new AdminRepository();

        public (int Total, int Active, int Transactions) GetSystemMetrics() =>
            (adminRepo.GetTotalUsers(), adminRepo.GetActiveUsers(), adminRepo.GetTotalTransactions());

        public List<(DateTime Date, int Count)> GetRegistrationStats(int days = 30) =>
            adminRepo.GetRegistrationsByDay(days);

        public List<User> GetUsers(string? search, string? status) =>
            adminRepo.GetAllUsers(search, status);

        public void BanUser(int userId) => adminRepo.BanUser(userId);
        public void UnbanUser(int userId) => adminRepo.UnbanUser(userId);
        public void UpdateLastLogin(int userId) => adminRepo.UpdateLastLogin(userId);

        public List<TransactionCategory> GetAllTransactionCategories() =>
            adminRepo.GetAllTransactionCategories();
        public void AddTransactionCategory(TransactionCategory cat) => adminRepo.AddTransactionCategory(cat);
        public void UpdateTransactionCategory(TransactionCategory cat) => adminRepo.UpdateTransactionCategory(cat);
        public void ToggleTransactionCategory(int id) => adminRepo.ToggleTransactionCategory(id);

        public List<BudgetCategory> GetAllBudgetCategories() =>
            adminRepo.GetAllBudgetCategories();
        public void AddBudgetCategory(BudgetCategory cat) => adminRepo.AddBudgetCategory(cat);
        public void UpdateBudgetCategory(BudgetCategory cat) => adminRepo.UpdateBudgetCategory(cat);
        public void ToggleBudgetCategory(int id) => adminRepo.ToggleBudgetCategory(id);
    }
}
