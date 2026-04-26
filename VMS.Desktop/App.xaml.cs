using System.Windows;
using VMS.Infrastructure;
using VMS.Services;
using VMS.Views;

namespace VMS
{
    public partial class App : Application
    {
        public static AuthService Auth { get; } = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemeManager.ApplyTheme(AppSettings.Instance.Theme);
            new LoginWindow().Show();
        }
    }
}
