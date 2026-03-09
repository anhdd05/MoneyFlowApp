using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        public readonly AppDbContext db = new AppDbContext();
        public void Add(Budget budget)
        {
            db.Budgets.Add(budget);
            db.SaveChanges();
        }

        public void Delete(int budgetId)
        {
            var target = db.Budgets.Find(budgetId);
            if (target == null) return;
            db.Budgets.Remove(target);
            db.SaveChanges();
        }

        public Budget? GetById(int budgetId)
        {
            return db.Budgets
                .Include(b => b.Category)
                .FirstOrDefault(b => b.BudgetId == budgetId);
        }

        public List<Budget> GetByMonth(int userId, int month, int year)
        {
            return db.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                .ToList();
        }

        public List<BudgetCategory> GetCategories()
        {
            return db.BudgetCategories
                .Where(bc => bc.IsDeleted != true)
                .OrderBy(bc => bc.Name)
                .ToList();
        }

        public void Update(Budget budget)
        {
            var target = db.Budgets.Find(budget.BudgetId);
            if (target == null) return;
            target.CategoryId = budget.CategoryId;
            target.Amount = budget.Amount;
            target.Description = budget.Description;
            target.AutoRenew = budget.AutoRenew;
            db.SaveChanges();
        }

        public void UpdateAllocated(int budgetId, decimal delta)
        {
            var target = db.Budgets.Find(budgetId);
            if (target == null) return;
            target.Allocated += delta;
            db.SaveChanges();
        }
    }
}
