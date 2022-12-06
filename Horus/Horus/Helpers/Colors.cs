using System;
using System.Linq;
using Xamarin.Forms;

namespace Horus.Helpers
{
    public static class Colors
    {
        private static readonly Lazy<ResourceDictionary> _colors = new Lazy<ResourceDictionary>(() =>
        {
            return Application.Current.Resources.MergedDictionaries.Single(md => md.Source.OriginalString.Contains("Styles/Colors.xaml"));
        });

        public static Color PrimaryColor => (Color)_colors.Value["Primary-Color"];
        public static Color SecondaryColor => (Color)_colors.Value["Secondary-Color"];
        public static Color SecondaryLightColor => (Color)_colors.Value["Secondary-Light-Color"];
        public static Color SecondaryDarkColor => (Color)_colors.Value["Secondary-Dark-Color"];
        public static Color WhiteColor => (Color)_colors.Value["White-Color"];
    }
}