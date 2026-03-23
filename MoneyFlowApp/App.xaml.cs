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
            var loginWin = new LoginWindow(); // Tên cái màn hình Login của ông
            loginWin.Show();
        }
    }


}
