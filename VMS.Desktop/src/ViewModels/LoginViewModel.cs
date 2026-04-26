using System;
using System.Windows.Input;
using VMS.Services;
using VMS.ViewModels.Base;

namespace VMS.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username      = string.Empty;
        private string _password      = string.Empty;
        private string _errorMessage  = string.Empty;
        private bool   _isLoading;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { SetProperty(ref _errorMessage, value); OnPropertyChanged(nameof(HasError)); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        public ICommand LoginCommand { get; }

        private readonly AuthService _auth;
        private readonly Action _onSuccess;

        public LoginViewModel(AuthService auth, Action onSuccess)
        {
            _auth      = auth;
            _onSuccess = onSuccess;
            LoginCommand = new RelayCommand(_ => ExecuteLogin(), _ => !IsLoading);
        }

        private async void ExecuteLogin()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password.";
                return;
            }

            IsLoading    = true;
            ErrorMessage = string.Empty;
            var ok       = await _auth.LoginAsync(Username, Password);
            IsLoading    = false;

            if (ok) _onSuccess();
            else ErrorMessage = "Invalid username or password.";
        }
    }
}
