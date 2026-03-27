using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetByMonth(int userId, int month, int year);
        List<TransactionCategory> GetCategories();

        void Add(Transaction transaction);
        void Delete(int transactionId);

        decimal SumIncome(int userId);
        decimal SumExpense(int userId);

        List<Transaction> getByDateRange(int userId, DateTime from, DateTime to);
    }
}
