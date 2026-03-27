using Repositories;
using BusinessObject.Models;

namespace Service
{
    public class BudgetService
    {
        private readonly IBudgetRepository budgetRepo = new BudgetRepository();
        private readonly ITransactionRepository transRepo = new TransactionRepository();

        public List<Budget> GetBudgets(int userId, int month, int year) => budgetRepo.GetByMonth(userId, month, year);
        public Budget? GetById(int budgetId) => budgetRepo.GetById(budgetId);

        public List<BudgetCategory> GetCategories() => budgetRepo.GetCategories();

        public void AddBudget(int userId, int categoryId, decimal amount,
        string? description, bool autoRenew, int month, int year)
        {
            budgetRepo.Add(new Budget
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Allocated = 0,
                Month = month,
                Year = year,
                Description = description,
                AutoRenew = autoRenew,
                CreatedAt = DateTime.Now,
            });
        }
        public void UpdateBudget(int budgetId, int categoryId, decimal amount, string? description, bool autoRenew)
        {
            budgetRepo.Update(new Budget
            {
                BudgetId = budgetId,
                CategoryId = categoryId,
                Amount = amount,
                Description = description,
                AutoRenew = autoRenew,
            });
        }
        public void DeleteBudget(int budgetId) => budgetRepo.Delete(budgetId);
        
        public void AllocateExpense(int budgetId, int txCategoryId, decimal amount, DateOnly date, string? note, int userId)
        {
            budgetRepo.UpdateAllocated(budgetId, amount);
            transRepo.Add(new Transaction
            {
                UserId = userId,
                CategoryId = txCategoryId,
                Amount = amount,
                Date = date,
                Note = note,
                CreatedAt = DateTime.Now,
            });
        }

        public void AllocateSaving(int budgetId, decimal amount) => budgetRepo.UpdateAllocated(budgetId, amount);

        public decimal GetBalance(int userId, int month, int year)
        {
            decimal income = transRepo.SumIncome(userId);
            decimal expense = transRepo.SumExpense(userId);
            decimal saving = budgetRepo.GetByMonth(userId, month, year)
                .Where(b => b.Category?.Type!.ToLower() == "saving")
                .Sum(b => b.Allocated);
            return income - expense - saving;
        }
    }
}
