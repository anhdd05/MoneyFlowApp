using System.Configuration;
using System.Data;
using System.Windows;

namespace MoneyFlowApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // obtain userId (login, config, args, etc.)
            int userId = 1; // example value

            var main = new MainWindow(userId);
            main.Show();
        }
    }


}
