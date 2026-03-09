using BusinessObjects;

namespace Repositories
{
    public interface IBudgetRepository
    {
        List<Budget> GetByMonth(int userId, int month, int year);
        Budget? GetById(int budgetId);
        List<BudgetCategory> GetCategories();

        void Add(Budget budget);
        void Update(Budget budget);
        void Delete(int budgetId);
        void UpdateAllocated(int budgetId, decimal delta);
    }
}
