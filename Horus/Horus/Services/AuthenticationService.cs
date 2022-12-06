using System;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Horus.Api.Account;
using Horus.Bootstrapping;
using Horus.Services.Api;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using Refit;

namespace Horus.Services
{
    public interface IAuthenticationService : IDisposable
    {
        IObservable<bool> ObserveBusy { get; }
        IObservable<AuthenticationStatus> Login(string email, string password);
    }
    
    [Singleton]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountApi _accountApi;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly BusyNotifier _busyNotifier = new BusyNotifier();

        public AuthenticationService(IAccountApi accountApi,
                                     IAccessTokenProvider tokenProvider)
        {
            _accountApi = accountApi;
            _tokenProvider = tokenProvider;
        }

        public IObservable<bool> ObserveBusy => _busyNotifier;

        public IObservable<AuthenticationStatus> Login(string email, string password)
        {
            return Observable.Create<AuthenticationStatus>(o =>
            {
                var disposables = new CompositeDisposable();

                var busy = _busyNotifier.ProcessStart();

                _accountApi.SignIn(new SignInRequest {Email = email, Password = password})
                    .Finally(busy.Dispose)
                    .Subscribe(OnLogin, OnError)
                    .AddTo(_disposables);

                void OnLogin(SignInResponse response)
                {
                    if (string.IsNullOrWhiteSpace(response.AuthorizationToken))
                    {
                        OnUnauthorized();
                        return;
                    }
                    
                    _tokenProvider.SetToken(response.AuthorizationToken);
                    o.OnNext(AuthenticationStatus.Successful);
                    o.OnCompleted();
                }

                void OnError(Exception ex)
                {
                    if (ex is ApiException {StatusCode: HttpStatusCode.Unauthorized})
                    {
                        OnUnauthorized();
                        return;
                    }
                    
                    o.OnError(ex);
                }
                
                void OnUnauthorized()
                {
                    _tokenProvider.SetToken(null);
                    o.OnNext(AuthenticationStatus.Unauthorized);
                    o.OnCompleted();
                }
                
                return disposables;
            });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

    }

    public enum AuthenticationStatus
    {
        Successful,
        Unauthorized
    }
}