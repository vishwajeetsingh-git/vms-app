using VMS.Infrastructure;
using VMS.ViewModels;

namespace VMS.Views
{
    public partial class MainShell : ChromeWindow
    {
        public MainShell()
        {
            InitializeComponent();
            DataContext = new MainShellViewModel();
        }
    }
}
