using System.Linq;
using System.Windows;
using VMS.Infrastructure;
using VMS.Models;
using VMS.Services;
using VMS.ViewModels.Pages;

namespace VMS.Views.Pages
{
    public partial class AddEditCameraDialog : ChromeWindow
    {
        private readonly AddEditCameraViewModel _vm;

        public CameraCreateDto? CreateResult { get; private set; }
        public CameraUpdateDto? UpdateResult { get; private set; }

        private AddEditCameraDialog(AddEditCameraViewModel vm)
        {
            _vm = vm;
            InitializeComponent();
            DataContext = vm;
            TitleBarCtrl.Title = vm.Title;
        }

        public static CameraCreateDto? ShowAdd()
        {
            var vm     = new AddEditCameraViewModel();
            var dialog = new AddEditCameraDialog(vm) { Owner = ActiveWindow() };
            dialog.ShowDialog();
            return dialog.CreateResult;
        }

        public static CameraUpdateDto? ShowEdit(CameraModel camera)
        {
            var vm     = new AddEditCameraViewModel(camera);
            var dialog = new AddEditCameraDialog(vm) { Owner = ActiveWindow() };
            dialog.ShowDialog();
            return dialog.UpdateResult;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // read PasswordBox values (can't bind Password in WPF)
            _vm.Password      = RtspPasswordBox.Password;
            _vm.OnvifPassword = OnvifPasswordBox.Password;

            var error = _vm.Validate();
            if (error is not null)
            {
                AppDialog.Show(error, "Validation", AppDialogIcon.Warning);
                return;
            }

            if (_vm.EditId.HasValue)
                UpdateResult = _vm.ToUpdateDto();
            else
                CreateResult = _vm.ToCreateDto();

            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        private static Window? ActiveWindow()
            => Application.Current?.Windows.OfType<Window>()
                   .FirstOrDefault(w => w.IsActive && w is not AppDialog)
               ?? Application.Current?.MainWindow;
    }
}
