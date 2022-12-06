using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace Horus
{
    public abstract class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected ViewModelBase(INavigationService navigationService,
                             IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            Disposables = new CompositeDisposable();
        }
        
        protected INavigationService NavigationService { get; }
        protected IPageDialogService PageDialogService { get; }
        protected CompositeDisposable Disposables { get; }
        
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}