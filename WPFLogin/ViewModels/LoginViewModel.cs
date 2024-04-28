using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFLogin.Models;
using WPFLogin.Repositories;

namespace WPFLogin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible=true;
        private IUserRepository userRepository;

        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password 
        { 
            get{ return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage 
        { 
            get{ return _errorMessage; } 
            set
            { 
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible 
        {
            get
            {
                return _isViewVisible;
            }
            set 
            { 
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //command
        public ICommand LoginCommand {  get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPasswordCommand("", ""));
        }

        private void ExecuteRecoverPasswordCommand(string username, string email)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validateData;
            if(string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password ==null || Password.Length < 3)
            {
                validateData = true;
            }
            else
            {
                validateData = false;
            }
            return validateData;
        }
    }
}
