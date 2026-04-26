using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VMS.ViewModels.Base;

namespace VMS.Models
{
    public class NavGroup : INotifyPropertyChanged
    {
        private bool _isExpanded = true;

        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public ObservableCollection<NavItem> Items { get; set; } = new();

        public bool IsExpanded
        {
            get => _isExpanded;
            set { if (_isExpanded != value) { _isExpanded = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class NavItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
        public bool IsPlaceholder { get; set; }
        public Func<ViewModelBase>? PageFactory { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set { if (_isSelected != value) { _isSelected = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
