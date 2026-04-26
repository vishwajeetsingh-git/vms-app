using VMS.ViewModels.Base;

namespace VMS.ViewModels.Pages
{
    public class DashboardViewModel : ViewModelBase
    {
        public string WelcomeMessage => "Welcome back.";
        public string TotalCameras   => "12";
        public string OnlineCameras  => "10";
        public string Recordings     => "8";
        public string ActiveAlarms   => "3";
    }
}
