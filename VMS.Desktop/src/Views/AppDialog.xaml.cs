using System.Linq;
using System.Windows;
using System.Windows.Media;
using VMS.Infrastructure;

namespace VMS.Views
{
    public enum AppDialogIcon { Info, Warning, Error }

    public partial class AppDialog : ChromeWindow
    {
        private bool _result;

        private AppDialog(string message, string title, AppDialogIcon icon, bool confirm)
        {
            InitializeComponent();
            TitleBarCtrl.Title = title;
            MessageText.Text   = message;

            switch (icon)
            {
                case AppDialogIcon.Warning:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0xCE, 0x91, 0x78));
                    IconText.Text = "⚠";
                    break;
                case AppDialogIcon.Error:
                    IconCircle.Background = new SolidColorBrush(Color.FromRgb(0xF4, 0x87, 0x71));
                    IconText.Text = "✕";
                    break;
            }

            if (confirm)
            {
                OkPanel.Visibility      = Visibility.Collapsed;
                ConfirmPanel.Visibility = Visibility.Visible;
            }
        }

        public static void Show(string message, string title = "Info",
                                AppDialogIcon icon = AppDialogIcon.Info)
        {
            var d = new AppDialog(message, title, icon, false) { Owner = ActiveWindow() };
            d.ShowDialog();
        }

        public static bool Confirm(string message, string title = "Confirm")
        {
            var d = new AppDialog(message, title, AppDialogIcon.Info, true) { Owner = ActiveWindow() };
            d.ShowDialog();
            return d._result;
        }

        private static Window? ActiveWindow()
            => Application.Current?.Windows.OfType<Window>()
                   .FirstOrDefault(w => w.IsActive && w is not AppDialog)
               ?? Application.Current?.MainWindow;

        private void OK_Click(object sender, RoutedEventArgs e)  { _result = true;  Close(); }
        private void Yes_Click(object sender, RoutedEventArgs e) { _result = true;  Close(); }
        private void No_Click(object sender, RoutedEventArgs e)  { _result = false; Close(); }
    }
}
