using Horus.Controls;
using Horus.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(NavigationPageRenderer))]
namespace Horus.iOS.Renderers
{
    public class NavigationPageRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                var navigationBarAppearance = NavigationBar.StandardAppearance;
                navigationBarAppearance.ConfigureWithTransparentBackground();
                SetAppearanceOnNavigationBar(navigationBarAppearance);
            }
        }

        void SetAppearanceOnNavigationBar(UINavigationBarAppearance navigationBarAppearance)
        {
            NavigationBar.CompactAppearance = navigationBarAppearance;
            NavigationBar.StandardAppearance = navigationBarAppearance;
            NavigationBar.ScrollEdgeAppearance = navigationBarAppearance;
        }
    }
}