using BusinessObjects;
using Repositories;

namespace Service
{
    public class MonthSummary
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public string Label => $"{Month}/{Year}";
    }

    public class CategorySummary
    {
        public string CatName { get; set; } = "";
        public decimal Total { get; set; }
        public double Percentage { get; set; }
    }

    public class ReportService
    {
        private readonly ITransactionRepository transRepo = new TransactionRepository();

        public List<MonthSummary> GetMonthSummaries(int userId, int monthCount)
        {
            DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-(monthCount - 1));
            DateTime to = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);

            List<Transaction> trans = transRepo.getByDateRange(userId, from, to);

            var result = new List<MonthSummary>();

            for (int i = monthCount-1; i>= 0; i--)
            {
                var targetDate = DateTime.Today.AddMonths(-i);
                int m = targetDate.Month;
                int y = targetDate.Year;

                var monthTran = trans.Where(t => t.Date.Month == m && t.Date.Year == y).ToList();
                MonthSummary summary = new MonthSummary
                {
                    Month = m,
                    Year = y,
                    Income = monthTran.Where(t => t.Category?.Type!.ToLower() == "income")
                    .Sum(t => t.Amount),
                    Expense = monthTran.Where(t => t.Category?.Type!.ToLower() == "expense")
                    .Sum(t => t.Amount)
                };
                result.Add(summary);
            }
            return result;
        }

        public List<CategorySummary> GetCategorySummaries(int userId, int monthCount, string Type)
        {
            DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-(monthCount - 1));
            DateTime to = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);

            List<Transaction> trans = transRepo.getByDateRange(userId, from, to);

            var filtered = trans.Where(t => t.Category?.Type?.ToLower() == Type.ToLower()).ToList();
            decimal total = filtered.Sum(t => t.Amount);
            if(total == 0) return new List<CategorySummary>();

            var result = filtered
                .GroupBy(t => t.Category?.Name ?? "Khác")
                .Select(g => new CategorySummary { CatName = g.Key, Total = g.Sum(t => t.Amount), Percentage = (double)(g.Sum(t => t.Amount) / total * 100) })
                .OrderByDescending(c => c.Total)
                .ToList();
            return result;
        }
    }
}
