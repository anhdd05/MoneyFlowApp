using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public readonly AppDbContext db = new AppDbContext();
        public void Add(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public void Delete(int transactionId)
        {
            var target = db.Transactions.Find(transactionId);
            if (target == null) return;
            db.Transactions.Remove(target);
            db.SaveChanges();
        }

        public List<Transaction> GetByMonth(int userId, int month, int year)
        {
            return db.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.Date.Month == month && t.Date.Year == year)
            .OrderByDescending(t => t.Date)
            .ToList();
        }

        public List<TransactionCategory> GetCategories()
        {
            return db.TransactionCategories
                .Where(tc => tc.IsDeleted != true)
                .OrderBy(tc => tc.Name)
                .ToList();
        }

        public decimal SumExpense(int userId)
        {
            return db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Category != null && t.Category.Type!.ToLower() == "expense")
                .Sum(t => t.Amount);
        }

        public decimal SumIncome(int userId)
        {
            return db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Category != null && t.Category.Type!.ToLower() == "income")
                .Sum(t => t.Amount);
        }

        public List<Transaction> getByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            DateOnly from = DateOnly.FromDateTime(startDate);
            DateOnly to = DateOnly.FromDateTime(endDate);

            return db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Date >= from && t.Date <= to)
                .OrderBy(t => t.Date)
                .ToList();
        }
    }
}
