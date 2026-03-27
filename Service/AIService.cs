using BusinessObject.Models;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Service;

namespace DuyAnh.Services;

public class AIService
{
    private readonly GenerativeModel _model;

    public AIService()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string apiKey = config["Gemini:ApiKey"] ?? "";
        var google = new GoogleAI(apiKey);
        _model = google.GenerativeModel("gemini-2.0-flash");
    }
    // ── TÍNH NĂNG 2: Nhận xét báo cáo ──
    public async Task<string> AnalyzeReportAsync(List<MonthSummary> months, List<CategorySummary> categories, string catType)
    {
        var monthLines = months.Select(m =>
            $"  {m.Label}: Thu {m.Income:N0}₫ — Chi {m.Expense:N0}₫");
        string monthData = string.Join("\n", monthLines);

        var catLines = categories.Select(c =>
            $"  {c.CatName}: {c.Total:N0}₫ ({c.Percentage:F1}%)");
        string catData = string.Join("\n", catLines);

        string typeLabel = catType == "income" ? "thu nhập" : "chi tiêu";

        string prompt =
            $"Đây là dữ liệu tài chính cá nhân:\n\n" +
            $"Thu/Chi theo tháng:\n{monthData}\n\n" +
            $"Tỉ lệ danh mục {typeLabel}:\n{catData}\n\n" +
            $"Hãy viết nhận xét ngắn gọn (3-5 câu) bằng tiếng Việt về tình hình tài chính, " +
            $"xu hướng nổi bật và 1 lời khuyên thực tế.";

        var response = await _model.GenerateContent(prompt);
        return response.Text?.Trim() ?? "";
    }

    // ── TÍNH NĂNG 4: Chat tư vấn tài chính ──
    public async Task<string> ChatAsync(List<(string role, string content)> history, decimal netWorth)
    {
        string historyText = string.Join("\n", history.Select(h =>
            h.role == "user" ? $"Người dùng: {h.content}" : $"Trợ lý: {h.content}"));

        string prompt =
            $"Bạn là trợ lý tư vấn tài chính. Số dư người dùng: {netWorth:N0}₫.\n\n" +
            $"Lịch sử hội thoại:\n{historyText}\n\n" +
            $"Hãy trả lời tin nhắn cuối cùng của người dùng bằng tiếng Việt, ngắn gọn.";

        var response = await _model.GenerateContent(prompt);
        return response.Text?.Trim() ?? "";
    }
}