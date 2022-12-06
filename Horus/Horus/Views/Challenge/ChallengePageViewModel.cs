using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Horus.Api;
using Horus.Helpers;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;

namespace Horus.Views.Challenge
{
    public class ChallengePageViewModel : ViewModelBase
    {
        private readonly IApi _api;
        private readonly BusyNotifier _busyNotifier = new BusyNotifier();

        public ChallengePageViewModel(INavigationService navigationService, 
                                      IPageDialogService pageDialogService,
                                      IApi api) : base(navigationService, pageDialogService)
        {
            _api = api;

            Items = new EnhancedReactiveCollection<ChallengeItemViewModel>().AddTo(Disposables);
            IsBusy = _busyNotifier.ToReadOnlyReactiveProperty().AddTo(Disposables);
            CompletedChallenges = new ReactiveProperty<int>().AddTo(Disposables);
            TotalChallenges = new ReactiveProperty<int>().AddTo(Disposables);
            
            BackCommand = new ReactiveCommand().AddTo(Disposables);

            BackCommand.Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOnUIDispatcher()
                .Subscribe(_ => NavigationService.GoBackAsync().ToObservable().Subscribe().AddTo(Disposables))
                .AddTo(Disposables);
        }
        
        public EnhancedReactiveCollection<ChallengeItemViewModel> Items { get; }
        public ReadOnlyReactiveProperty<bool> IsBusy { get; }
        public ReactiveProperty<int> CompletedChallenges { get; }
        public ReactiveProperty<int> TotalChallenges { get; }
        public ReactiveCommand BackCommand { get; }

        public override void Initialize(INavigationParameters parameters)
        {
            var busy = _busyNotifier.ProcessStart();

            _api.GetChallenges()
                .Finally(busy.Dispose)
                .Subscribe(OnChallenges, OnError)
                .AddTo(Disposables);
        }

        private void OnChallenges(IEnumerable<Api.Challenges.Challenge> items)
        {
            var vms = items.Select(x => new ChallengeItemViewModel(x, PageDialogService).AddTo(Disposables)).ToList();
            TotalChallenges.Value = vms.Count;
            CompletedChallenges.Value = vms.Count(x => x.IsCompleted);
            Items.AddRange(vms);
        }

        private void OnError(Exception ex)
        {
            PageDialogService
                .DisplayAlertAsync("Error", "Something went wrong loading challenges. Please try again later.", "OK")
                .ToObservable()
                .Subscribe()
                .AddTo(Disposables);
        }
    }

    public class ChallengeItemViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Api.Challenges.Challenge _challenge;

        public ChallengeItemViewModel(Api.Challenges.Challenge challenge,
                                      IPageDialogService pageDialogService)
        {
            _challenge = challenge;

            SelectCommand = new ReactiveCommand().AddTo(_disposables);
            SelectCommand.Throttle(TimeSpan.FromMilliseconds(250))
                .ObserveOnUIDispatcher()
                .Subscribe(_ =>
                {
                    pageDialogService.DisplayAlertAsync(Title, Description, "OK")
                        .ToObservable()
                        .Subscribe()
                        .AddTo(_disposables);
                })
                .AddTo(_disposables);

        }

        public string Title => _challenge.Title;
        public string Description => _challenge.Description;
        public double CurrentPoints => _challenge.CurrentPoints;
        public double TotalPoints => _challenge.TotalPoints;
        
        public double Progress => CurrentPoints / TotalPoints;
        public double Percent => Math.Floor(Progress * 100);

        public bool IsCompleted => (int) Progress == 1;
        
        public ReactiveCommand SelectCommand { get; }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}