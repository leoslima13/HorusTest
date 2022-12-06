using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Horus.Api.Account;
using Horus.Helpers;
using Horus.Services;
using Newtonsoft.Json.Serialization;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using Exception = System.Exception;

namespace Horus.Views.Login
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginPageViewModel(INavigationService navigationService, 
                                  IPageDialogService pageDialogService,
                                  IAuthenticationService authenticationService) : base(navigationService, pageDialogService)
        {
            _authenticationService = authenticationService;

            Email = new ReactiveProperty<string>().AddTo(Disposables);
            Password = new ReactiveProperty<string>().AddTo(Disposables);

            IsBusy = _authenticationService.ObserveBusy.ToReadOnlyReactiveProperty().AddTo(Disposables);
            
            LoginCommand = Email
                .CombineLatest(Password,
                    (email, password) => !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                .ToReactiveCommand()
                .WithSubscribe(OnLoginCommand, c => c.AddTo(Disposables));
        }
        
        public ReactiveProperty<string> Email { get; }
        public ReactiveProperty<string> Password { get; }
        public ReadOnlyReactiveProperty<bool> IsBusy { get; }
        public ReactiveCommand LoginCommand { get; }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Email.Value = null;
            Password.Value = null;
        }

        private void OnLoginCommand()
        {
            _authenticationService.Login(Email.Value, Password.Value)
                .ObserveOnUIDispatcher()
                .Subscribe(OnSignIn, OnError)
                .AddTo(Disposables);

            void OnSignIn(AuthenticationStatus authenticationStatus)
            {
                if (authenticationStatus == AuthenticationStatus.Unauthorized)
                {
                    PageDialogService.DisplayAlertAsync("Error",
                            "Invalid username or password", "OK")
                        .ToObservable()
                        .Subscribe()
                        .AddTo(Disposables);
                    return;
                }

                NavigationService.NavigateAsync(Pages.ChallengePage)
                    .ToObservable()
                    .Subscribe()
                    .AddTo(Disposables);
            }

            void OnError(Exception ex)
            {
                PageDialogService.DisplayAlertAsync("Error",
                        "Something went wrong trying to login. Check your credentials and try again", "OK")
                    .ToObservable()
                    .Subscribe()
                    .AddTo(Disposables);
            }
        }
    }
}