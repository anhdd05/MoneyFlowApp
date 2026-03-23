using System.Windows;

namespace MoneyFlowApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Chạy Admin: MoneyFlowApp.exe --admin
        var args = Environment.GetCommandLineArgs();
        bool isAdminMode = args.Any(a => string.Equals(a, "--admin", StringComparison.OrdinalIgnoreCase));

        if (isAdminMode)
        {
            var admin = new Admin.AdminWindow();
            admin.Show();
        }
        else
        {
            int userId = 1; // Giữ nguyên như trước - Login/Logout do người khác làm
            var main = new MainWindow(userId);
            main.Show();
        }
    }
}
