using System.Collections.ObjectModel;
using System.Windows.Input;
using VMS.Models;
using VMS.ViewModels.Base;
using VMS.ViewModels.Pages;

namespace VMS.ViewModels
{
    public class MainShellViewModel : ViewModelBase
    {
        private ViewModelBase? _currentPage;
        private NavItem? _selectedItem;
        private bool _isSidebarExpanded = true;

        public ViewModelBase? CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public NavItem? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public bool IsSidebarExpanded
        {
            get => _isSidebarExpanded;
            set => SetProperty(ref _isSidebarExpanded, value);
        }

        public ObservableCollection<NavGroup> NavGroups { get; } = new();

        public ICommand NavigateCommand { get; }
        public ICommand ToggleSidebarCommand { get; }
        public ICommand ToggleGroupCommand { get; }

        public MainShellViewModel()
        {
            NavigateCommand      = new RelayCommand(OnNavigate);
            ToggleSidebarCommand = new RelayCommand(_ => IsSidebarExpanded = !IsSidebarExpanded);
            ToggleGroupCommand   = new RelayCommand(g => { if (g is Models.NavGroup ng) ng.IsExpanded = !ng.IsExpanded; });

            BuildNav();
            CurrentPage = new DashboardViewModel();
        }

        private void BuildNav()
        {
            NavGroups.Add(new NavGroup
            {
                Name = "MONITORING", Icon = "📹", IsExpanded = true,
                Items = new()
                {
                    new NavItem { Name = "Live View",  Icon = "📹", IconPath = "/Assets/Icons/live-view.png",  PageFactory = () => new PlaceholderViewModel("Live View") },
                    new NavItem { Name = "Playback",   Icon = "⏯️", IconPath = "/Assets/Icons/playback.png",   PageFactory = () => new PlaceholderViewModel("Playback") },
                }
            });

            NavGroups.Add(new NavGroup
            {
                Name = "DEVICES", Icon = "⚙️", IsExpanded = true,
                Items = new()
                {
                    new NavItem { Name = "Cameras",    Icon = "📷", IconPath = "/Assets/Icons/device-management.png", PageFactory = () => new PlaceholderViewModel("Cameras") },
                    new NavItem { Name = "VR Servers", Icon = "🖥️", IconPath = "/Assets/Icons/system-configuration.png", PageFactory = () => new PlaceholderViewModel("VR Servers") },
                }
            });

            NavGroups.Add(new NavGroup
            {
                Name = "MANAGEMENT", Icon = "👥", IsExpanded = true,
                Items = new()
                {
                    new NavItem { Name = "Users", Icon = "👥", IconPath = "/Assets/Icons/user-management.png", PageFactory = () => new PlaceholderViewModel("Users") },
                }
            });

            NavGroups.Add(new NavGroup
            {
                Name = "ANALYTICS", Icon = "🧠", IsExpanded = false,
                Items = new()
                {
                    new NavItem { Name = "Face Recognition", Icon = "👤", IconPath = "/Assets/Icons/face-recognition.png",  IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Face Recognition") },
                    new NavItem { Name = "People Counting",  Icon = "👥", IconPath = "/Assets/Icons/peopel-counting.png",   IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("People Counting") },
                    new NavItem { Name = "LPR",              Icon = "🚗", IconPath = "/Assets/Icons/LPR.png",               IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("LPR") },
                    new NavItem { Name = "VCA",              Icon = "🎯", IconPath = "/Assets/Icons/VCA.png",               IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("VCA") },
                    new NavItem { Name = "Access Control",   Icon = "🔐", IconPath = "/Assets/Icons/access control.png",   IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Access Control") },
                }
            });

            NavGroups.Add(new NavGroup
            {
                Name = "REPORTS", Icon = "📋", IsExpanded = false,
                Items = new()
                {
                    new NavItem { Name = "Operation Log",  Icon = "📝", IconPath = "/Assets/Icons/log.png",           IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Operation Log") },
                    new NavItem { Name = "Alarm Records",  Icon = "🚨", IconPath = "/Assets/Icons/alarm-records.png", IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Alarm Records") },
                    new NavItem { Name = "E-Map",          Icon = "🗺️", IconPath = "/Assets/Icons/e-map.png",         IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("E-Map") },
                }
            });

            NavGroups.Add(new NavGroup
            {
                Name = "CONFIGURATION", Icon = "🔧", IsExpanded = false,
                Items = new()
                {
                    new NavItem { Name = "System Settings",     Icon = "⚙️", IconPath = "/Assets/Icons/system-configuration.png", PageFactory = () => new PlaceholderViewModel("System Settings") },
                    new NavItem { Name = "Recording Schedule",  Icon = "📅", IconPath = "/Assets/Icons/Record-Schedule.png",      IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Recording Schedule") },
                    new NavItem { Name = "Alarm Config",        Icon = "🔔", IconPath = "/Assets/Icons/alarm-configuration.png",  IsPlaceholder = true, PageFactory = () => new PlaceholderViewModel("Alarm Config") },
                }
            });
        }

        private void OnNavigate(object? parameter)
        {
            if (parameter is not NavItem item || item.PageFactory is null) return;
            if (_selectedItem != null) _selectedItem.IsSelected = false;
            SelectedItem     = item;
            item.IsSelected  = true;
            CurrentPage      = item.PageFactory();
        }
    }
}
