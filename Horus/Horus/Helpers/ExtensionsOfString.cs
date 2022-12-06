using System;

namespace Horus.Helpers
{
    public static class ExtensionsOfString
    {
        public static string AsNavigation(this string pageName)
        {
            return $"CustomNavigationPage/{pageName}";
        }
        
        public static Uri AsNavigationAbsolute(this string pageName)
        {
            return new Uri($"https://horuschallenge.com/CustomNavigationPage/{pageName}", UriKind.Absolute);
        }
    }
}