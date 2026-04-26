using VMS.ViewModels.Base;

namespace VMS.ViewModels.Pages
{
    public class PlaceholderViewModel : ViewModelBase
    {
        public string PageName { get; }
        public string Message { get; }

        public PlaceholderViewModel(string pageName)
        {
            PageName = pageName;
            Message = $"{pageName} is coming soon.";
        }
    }
}
