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

            // Obtain the userId from your auth/session logic
            int userId = /* get current user id */ 1;

            var main = new ManageBudgetWindow(userId);
            main.Show();
        }
    }


}
