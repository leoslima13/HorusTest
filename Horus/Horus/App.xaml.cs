using System;
using Horus.Api;
using Horus.Api.Account;
using Horus.Bootstrapping;
using Horus.Controls;
using Horus.Helpers;
using Horus.Services.Api;
using Horus.Views.Challenge;
using Horus.Views.Login;
using Microsoft.Extensions.DependencyInjection;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Horus
{
    public partial class App
    {
        private const string HORUS_API_URL = "https://horuschallenges.azurewebsites.net/api";

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().BootstrapTypes(new []{typeof(App).Assembly});
            
            containerRegistry.RegisterServices(s =>
            {
                var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());

                s.AddHttpClient("HorusAccount", c =>
                    {
                        c.BaseAddress = new Uri(HORUS_API_URL);
                        c.Timeout = TimeSpan.FromSeconds(60);
                    })
                    .AddTypedClient(c => Refit.RestService.For<IAccountApi>(c, refitSettings));

                s.AddHttpClient("HorusApi", c =>
                    {
                        c.BaseAddress = new Uri(HORUS_API_URL);
                        c.Timeout = TimeSpan.FromSeconds(60);
                    })
                    .AddTypedClient(c => Refit.RestService.For<IApi>(c, refitSettings))
                    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
            });

            containerRegistry.RegisterForNavigation<Xamarin.Forms.NavigationPage>();
            containerRegistry.RegisterForNavigation<CustomNavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ChallengePage, ChallengePageViewModel>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            
            var customNavigationPage = new CustomNavigationPage(new LoginPage())
            {
                BarTextColor = Colors.WhiteColor,
                BarBackgroundColor = Color.Transparent
            };
            MainPage = customNavigationPage;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}