using System.Windows.Controls;
using System.Windows;
using VMS.Infrastructure;
using VMS.ViewModels;

namespace VMS.Views
{
    public partial class LoginWindow : ChromeWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(App.Auth, OnLoginSuccess);
        }

        private void OnLoginSuccess()
        {
            new MainShell().Show();
            Close();
        }

        private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }
    }
}
