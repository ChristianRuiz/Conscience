using Conscience.Domain;
using Conscience.Mobile.Hosts.Core.Services;
using Conscience.Plugins;
using Microsoft.AspNet.SignalR.Client;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Conscience.Mobile.Hosts.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly AppState _appState;
        private readonly GraphQLService _graphService;
        
        public LoginViewModel(AppState appState, GraphQLService graphService)
        {
            _appState = appState;
            _graphService = graphService;

#if DEBUG
            UserName = "dolores";
            Password = "123456";
#endif
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private bool _hasError;
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value;
                RaisePropertyChanged(() => HasError);
            }
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                _loginCommand = _loginCommand ?? new MvxAsyncCommand(Login);
                return _loginCommand;
            }
        }

        private async Task Login()
        {
            HasError = false;

            try
            {
                var query =
@"mutation Login($userName: String!, $password: String!) {
  accounts
  {
    login(userName:$userName, password:$password)
    {
      id,
      userName,
      roles {
        name
      }
    }
  }
}";
                var variables = new Dictionary<string, object>
                                {
                                    { "userName", UserName },
                                    { "password", Password }
                                };

                _appState.CurrentUser = await _graphService.ExecuteAsync<Account>(query, variables, json => json["accounts"]["login"]);

                ShowViewModel<MainViewModel>();
            }
            catch
            {
                HasError = true;
            }
        }
    }
}
