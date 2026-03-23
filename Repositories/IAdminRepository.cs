using BusinessObjects;

namespace Repositories
{
    public interface IAdminRepository
    {
        int GetTotalUsers();
        int GetActiveUsers();
        int GetTotalTransactions();
        List<(DateTime Date, int Count)> GetRegistrationsByDay(int days);

        List<User> GetAllUsers(string? search, string? statusFilter);
        void BanUser(int userId);
        void UnbanUser(int userId);
        void UpdateLastLogin(int userId);

        List<TransactionCategory> GetAllTransactionCategories();
        void AddTransactionCategory(TransactionCategory cat);
        void UpdateTransactionCategory(TransactionCategory cat);
        void ToggleTransactionCategory(int categoryId);

        List<BudgetCategory> GetAllBudgetCategories();
        void AddBudgetCategory(BudgetCategory cat);
        void UpdateBudgetCategory(BudgetCategory cat);
        void ToggleBudgetCategory(int categoryId);
    }
}
