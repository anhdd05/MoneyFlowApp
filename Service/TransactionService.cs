using BusinessObject.Models;
using Repositories;

namespace Service
{
    public class TransactionService
    {
        private readonly ITransactionRepository transRepo = new TransactionRepository();
        public List<Transaction> GetByMonth(int userId, int month, int year) => transRepo.GetByMonth(userId, month, year);
        public List<TransactionCategory> GetCategories() => transRepo.GetCategories();

        public void AddTransaction(int userId, int categoryId, decimal amount,
            DateOnly date, string? note)
        {
            transRepo.Add(new Transaction
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Date = date,
                Note = note,
                CreatedAt = DateTime.Now,
            });
        }
        public void DeleteTransaction(int transactionId) => transRepo.Delete(transactionId);
    }
}
