using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Horus.Bootstrapping;
using Reactive.Bindings.Extensions;

namespace Horus.Services.Api
{
    public interface IAccessTokenProvider
    {
        IObservable<string> ObserveToken { get; }
        void SetToken(string token);
    }
    
    [Singleton]
    public class AccessTokenProvider : IAccessTokenProvider, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly BehaviorSubject<string> _tokenSubject;

        public AccessTokenProvider()
        {
            _tokenSubject = new BehaviorSubject<string>(null).AddTo(_disposables);
        }

        public IObservable<string> ObserveToken => _tokenSubject;

        public void SetToken(string token) => _tokenSubject.OnNext(token);

        public void Dispose() => _disposables.Dispose();
    }
}