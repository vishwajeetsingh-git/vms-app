using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VMS.Models;
using VMS.ViewModels.Base;
using VMS.Views;
using VMS.Views.Pages;

namespace VMS.ViewModels.Pages
{
    public class CamerasViewModel : ViewModelBase
    {
        private string  _searchText    = string.Empty;
        private bool    _isLoading;
        private CameraModel? _selected;

        private readonly ObservableCollection<CameraModel> _all = new();
        public ObservableCollection<CameraModel> Filtered { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) ApplyFilter(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public CameraModel? Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public int TotalCount  => _all.Count;
        public int OnlineCount => _all.Count(c => c.IsOnline);

        public ICommand RefreshCommand         { get; }
        public ICommand AddCommand             { get; }
        public ICommand EditCommand            { get; }
        public ICommand DeleteCommand          { get; }
        public ICommand ToggleRecordingCommand { get; }

        public CamerasViewModel()
        {
            RefreshCommand         = new RelayCommand(async _ => await LoadAsync());
            AddCommand             = new RelayCommand(async _ => await AddCameraAsync());
            EditCommand            = new RelayCommand(async p  => await EditCameraAsync(p as CameraModel), _ => true);
            DeleteCommand          = new RelayCommand(async p  => await DeleteCameraAsync(p as CameraModel), _ => true);
            ToggleRecordingCommand = new RelayCommand(p => ToggleRecording(p as CameraModel));

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var cameras = await App.Cameras.GetCamerasAsync();
                _all.Clear();
                foreach (var cam in cameras) _all.Add(cam);
                ApplyFilter();
                OnPropertyChanged(nameof(TotalCount));
                OnPropertyChanged(nameof(OnlineCount));
            }
            catch (Exception ex)
            {
                AppDialog.Show($"Failed to load cameras:\n{ex.Message}", "Error", AppDialogIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilter()
        {
            Filtered.Clear();
            var q = _searchText.Trim().ToLower();
            foreach (var cam in _all)
            {
                if (string.IsNullOrEmpty(q)
                    || cam.Name.ToLower().Contains(q)
                    || cam.RtspUrl.ToLower().Contains(q)
                    || cam.Location.ToLower().Contains(q)
                    || cam.OnvifHost.ToLower().Contains(q))
                {
                    Filtered.Add(cam);
                }
            }
        }

        private async Task AddCameraAsync()
        {
            var result = AddEditCameraDialog.ShowAdd();
            if (result is null) return;

            IsLoading = true;
            try
            {
                var created = await App.Cameras.CreateAsync(result);
                if (created is not null)
                {
                    _all.Add(created);
                    ApplyFilter();
                    OnPropertyChanged(nameof(TotalCount));
                    OnPropertyChanged(nameof(OnlineCount));
                }
            }
            catch (Exception ex)
            {
                AppDialog.Show($"Failed to add camera:\n{ex.Message}", "Error", AppDialogIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task EditCameraAsync(CameraModel? camera)
        {
            if (camera is null) return;

            var result = AddEditCameraDialog.ShowEdit(camera);
            if (result is null) return;

            IsLoading = true;
            try
            {
                var updated = await App.Cameras.UpdateAsync(camera.Id, result);
                if (updated is not null)
                {
                    var idx = _all.IndexOf(camera);
                    if (idx >= 0) _all[idx] = updated;
                    ApplyFilter();
                }
            }
            catch (Exception ex)
            {
                AppDialog.Show($"Failed to update camera:\n{ex.Message}", "Error", AppDialogIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteCameraAsync(CameraModel? camera)
        {
            if (camera is null) return;
            if (!AppDialog.Confirm($"Delete \"{camera.Name}\"?", "Delete Camera")) return;

            IsLoading = true;
            try
            {
                var ok = await App.Cameras.DeleteAsync(camera.Id);
                if (ok)
                {
                    _all.Remove(camera);
                    ApplyFilter();
                    OnPropertyChanged(nameof(TotalCount));
                    OnPropertyChanged(nameof(OnlineCount));
                }
                else
                {
                    AppDialog.Show("Delete failed. Please try again.", "Error", AppDialogIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AppDialog.Show($"Failed to delete camera:\n{ex.Message}", "Error", AppDialogIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private static void ToggleRecording(CameraModel? camera)
        {
            if (camera is null) return;
            AppDialog.Show(
                "Recording control is managed through the VR Server.\nConnect to the VR Server service to start or stop recording.",
                "Recording Control",
                AppDialogIcon.Info);
        }
    }
}
