using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MoneyFlowApp;

public partial class ReportWindow : Window
{
    private readonly ReportService reportService = new ReportService();
    private readonly int userId;

    private int monthCount = 12;
    private string catType = "expense";

    // Bảng màu dùng chung
    private readonly List<Brush> colors = new()
    {
        Brushes.SteelBlue,
        Brushes.Tomato,
        Brushes.MediumSeaGreen,
        Brushes.Orange,
        Brushes.Orchid,
        Brushes.SandyBrown,
        Brushes.CadetBlue,
        Brushes.Coral,
        Brushes.MediumPurple
    };

    public ReportWindow(int uid)
    {
        InitializeComponent();
        userId = uid;
        Loaded += (s, e) => DrawAll();
    }
    //nút chọn range
    private void RdMonth_Checked(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;
        if (Rd3Month.IsChecked == true) monthCount = 3;
        if (Rd6Month.IsChecked == true) monthCount = 6;
        if (Rd12Month.IsChecked == true) monthCount = 12;
        DrawAll();
    }
    //nút chọn loại hiển thị cho biểu đồ tròn
    private void RdType_Checked(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;
        catType = RdIncome.IsChecked == true ? "income" : "expense";
        DrawAll();
    }

    private void DrawAll()
    {
        var months = reportService.GetMonthSummaries(userId, monthCount);
        var categories = reportService.GetCategorySummaries(userId, monthCount, catType);

        DrawPieChart(categories);
        DrawBarChart(months);
    }
    //vẽ biểu đồ tròn
    private void DrawPieChart(List<CategorySummary> categories)
    {
        PieChart.Children.Clear();

        if (categories.Count == 0)
        {
            EmptyChart(PieChart);
            return;
        }

        double cx = 130;
        double cy = PieChart.ActualHeight / 2;
        double radius = 100;

        //chỉ có 1 danh mục -> vẽ hình tròn đầy
        if (categories.Count == 1)
        {
            var circle = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = colors[0],
            };
            Canvas.SetLeft(circle, cx - radius);
            Canvas.SetTop(circle, cy - radius);
            PieChart.Children.Add(circle);
        }
        else
        {
            double startAngle = -90;
            for (int i = 0; i < categories.Count; i++)
            {
                double sweep = categories[i].Percentage / 100.0 * 360.0;
                DrawSlice(cx, cy, radius, startAngle, sweep, colors[i % colors.Count]);
                startAngle += sweep;
            }
        }

        //chú thích
        double legendY = 16;
        for (int i = 0; i < categories.Count; i++)
        {
            var rect = new Rectangle { Width = 12, Height = 12, Fill = colors[i % colors.Count] };
            Canvas.SetLeft(rect, cx + radius + 20);
            Canvas.SetTop(rect, legendY + i * 22);
            PieChart.Children.Add(rect);

            var lbl = new TextBlock
            {
                Text = $"{categories[i].CatName}  {categories[i].Percentage:F1}%",
                FontSize = 11,
            };
            Canvas.SetLeft(lbl, cx + radius + 36);
            Canvas.SetTop(lbl, legendY + i * 22);
            PieChart.Children.Add(lbl);
        }
    }

    private void DrawSlice(double cx, double cy, double radius,
                               double startDeg, double sweepDeg, Brush color)
    {
        double startRad = startDeg * Math.PI / 180;
        double endRad = (startDeg + sweepDeg) * Math.PI / 180;

        double x1 = cx + radius * Math.Cos(startRad);
        double y1 = cy + radius * Math.Sin(startRad);
        double x2 = cx + radius * Math.Cos(endRad);
        double y2 = cy + radius * Math.Sin(endRad);

        var figure = new PathFigure { StartPoint = new Point(cx, cy) };
        figure.Segments.Add(new LineSegment(new Point(x1, y1), true));
        figure.Segments.Add(new ArcSegment(
            new Point(x2, y2),
            new Size(radius, radius),
            rotationAngle: 0,
            isLargeArc: sweepDeg > 180,
            sweepDirection: SweepDirection.Clockwise,
            isStroked: true));
        figure.Segments.Add(new LineSegment(new Point(cx, cy), true));

        var path = new Path
        {
            Data = new PathGeometry(new[] { figure }),
            Fill = color,
            Stroke = Brushes.White,
            StrokeThickness = 1,
        };
        PieChart.Children.Add(path);
    }

    //biểu đồ cột
    private void DrawBarChart(List<MonthSummary> months)
    {
        BarChart.Children.Clear();
        if (months.Count == 0) return;

        double paddingL = 60;
        double paddingB = 40;
        double paddingT = 16;
        double chartW = BarChart.ActualWidth - paddingL - 16;
        double chartH = BarChart.ActualHeight - paddingB - paddingT;

        decimal maxVal = months.Max(m => Math.Max(m.Income, m.Expense));
        if (maxVal == 0) maxVal = 1;

        double groupW = chartW / months.Count;
        double barW = groupW * 0.3;

        for (int i = 0; i < months.Count; i++)
        {
            double x = paddingL + i * groupW;

            // Cột thu nhập
            double incomeH = (double)(months[i].Income / maxVal) * chartH;
            var incomeBar = new Rectangle
            {
                Width = barW,
                Height = Math.Max(incomeH, 1),
                Fill = Brushes.SteelBlue,
            };
            Canvas.SetLeft(incomeBar, x + groupW * 0.1);
            Canvas.SetTop(incomeBar, paddingT + chartH - incomeH);
            BarChart.Children.Add(incomeBar);

            // Cột chi tiêu
            double expenseH = (double)(months[i].Expense / maxVal) * chartH;
            var expenseBar = new Rectangle
            {
                Width = barW,
                Height = Math.Max(expenseH, 1),
                Fill = Brushes.Tomato,
            };
            Canvas.SetLeft(expenseBar, x + groupW * 0.1 + barW + 2);
            Canvas.SetTop(expenseBar, paddingT + chartH - expenseH);
            BarChart.Children.Add(expenseBar);

            // Nhãn tháng
            var label = new TextBlock
            {
                Text = months[i].Label,
                FontSize = 10,
                Width = groupW,
                TextAlignment = TextAlignment.Center,
            };
            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, paddingT + chartH + 4);
            BarChart.Children.Add(label);
        }

        // Baseline
        BarChart.Children.Add(new Line
        {
            X1 = paddingL,
            Y1 = paddingT + chartH,
            X2 = BarChart.ActualWidth - 16,
            Y2 = paddingT + chartH,
            Stroke = Brushes.Gray,
            StrokeThickness = 1,
        });

        // Chú thích
        double legendX = BarChart.ActualWidth - 130;
        AddLegendItem(BarChart, legendX, 8, Brushes.SteelBlue, "Thu nhập");
        AddLegendItem(BarChart, legendX + 65, 8, Brushes.Tomato, "Chi tiêu");
    }

    private void AddLegendItem(Canvas canvas, double x, double y, Brush color, string text)
    {
        var rect = new Rectangle { Width = 12, Height = 12, Fill = color };
        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        canvas.Children.Add(rect);

        var lbl = new TextBlock { Text = text, FontSize = 10 };
        Canvas.SetLeft(lbl, x + 14);
        Canvas.SetTop(lbl, y);
        canvas.Children.Add(lbl);
    }

    private void EmptyChart(Canvas canvas)
    {
        var lbl = new TextBlock
        {
            Text = "Không có dữ liệu",
            FontSize = 13,
            Foreground = Brushes.Gray,
        };
        Canvas.SetLeft(lbl, canvas.ActualWidth / 2 - 60);
        Canvas.SetTop(lbl, canvas.ActualHeight / 2 - 10);
        canvas.Children.Add(lbl);
    }
}