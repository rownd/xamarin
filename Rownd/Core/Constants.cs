using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Xamarin.Forms;
using Xamarin.Forms.Core.PlatformConfiguration;

namespace Rownd.Xamarin.Core
{
    public static class Constants
    {
        private static string platform = Device.RuntimePlatform;
        private static string sdkVersion = Assembly.GetAssembly(typeof(Constants)).GetName().Version.ToString();

        public static string DEFAULT_API_USER_AGENT = $"Rownd SDK for Xamarin/{sdkVersion} (Language C#; Platform={platform}; (SDK {sdkVersion});)";
        public static string DEFAULT_WEB_USER_AGENT = $"Rownd SDK for Xamarin/{sdkVersion} (Language C#; Platform={platform}; (SDK {sdkVersion});)";
    }
}
