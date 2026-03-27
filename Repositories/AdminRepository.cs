using BusinessObject.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AdminRepository : IAdminRepository
    {
        public readonly AppDbContext db = new AppDbContext();

        public int GetTotalUsers() => db.Users.Count();

        public int GetActiveUsers() => db.Users.Count(u => !u.IsBanned);

        public int GetTotalTransactions() => db.Transactions.Count();

        public List<(DateTime Date, int Count)> GetRegistrationsByDay(int days)
        {
            var from = DateTime.Today.AddDays(-days);
            return db.Users
                .Where(u => u.CreatedAt != null && u.CreatedAt >= from)
                .AsEnumerable()
                .GroupBy(u => u.CreatedAt!.Value.Date)
                .Select(g => (g.Key, g.Count()))
                .OrderBy(x => x.Item1)
                .ToList();
        }

        public List<User> GetAllUsers(string? search, string? statusFilter)
        {
            var q = db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(u =>
                    (u.Email != null && u.Email.ToLower().Contains(s)) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(s)));
            }
            q = statusFilter?.ToLower() switch
            {
                "active" => q.Where(u => !u.IsBanned),
                "banned" => q.Where(u => u.IsBanned),
                "unverified" => q.Where(u => u.LastLoginAt == null),
                _ => q
            };
            return q.OrderBy(u => u.UserId).ToList();
        }

        public void BanUser(int userId)
        {
            var u = db.Users.Find(userId);
            if (u != null) { u.IsBanned = true; db.SaveChanges(); }
        }

        public void UnbanUser(int userId)
        {
            var u = db.Users.Find(userId);
            if (u != null) { u.IsBanned = false; db.SaveChanges(); }
        }

        public void UpdateLastLogin(int userId)
        {
            var u = db.Users.Find(userId);
            if (u != null) { u.LastLoginAt = DateTime.Now; db.SaveChanges(); }
        }

        public List<TransactionCategory> GetAllTransactionCategories() =>
            db.TransactionCategories.OrderBy(c => c.Name).ToList();

        public void AddTransactionCategory(TransactionCategory cat)
        {
            db.TransactionCategories.Add(cat);
            db.SaveChanges();
        }

        public void UpdateTransactionCategory(TransactionCategory cat)
        {
            var target = db.TransactionCategories.Find(cat.CategoryId);
            if (target == null) return;
            target.Name = cat.Name;
            target.Type = cat.Type;
            target.Icon = cat.Icon;
            target.Color = cat.Color;
            db.SaveChanges();
        }

        public void ToggleTransactionCategory(int categoryId)
        {
            var c = db.TransactionCategories.Find(categoryId);
            if (c != null) { c.IsDeleted = !(c.IsDeleted ?? false); db.SaveChanges(); }
        }

        public List<BudgetCategory> GetAllBudgetCategories() =>
            db.BudgetCategories.OrderBy(c => c.Name).ToList();

        public void AddBudgetCategory(BudgetCategory cat)
        {
            db.BudgetCategories.Add(cat);
            db.SaveChanges();
        }

        public void UpdateBudgetCategory(BudgetCategory cat)
        {
            var target = db.BudgetCategories.Find(cat.CategoryId);
            if (target == null) return;
            target.Name = cat.Name;
            target.Type = cat.Type;
            target.Icon = cat.Icon;
            target.Color = cat.Color;
            db.SaveChanges();
        }

        public void ToggleBudgetCategory(int categoryId)
        {
            var c = db.BudgetCategories.Find(categoryId);
            if (c != null) { c.IsDeleted = !(c.IsDeleted ?? false); db.SaveChanges(); }
        }
    }
}
