using System.Reflection;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Rownd.Xamarin.Core
{
    public static class Constants
    {
        private static string sdkVersion = Assembly.GetAssembly(typeof(Constants)).GetName().Version.ToString();

        public static readonly string DEFAULT_API_USER_AGENT = $"Rownd SDK for Xamarin/{sdkVersion} (Language C#; Platform={DeviceInfo.Platform} {DeviceInfo.VersionString}; (SDK {sdkVersion});)";
        public static readonly string DEFAULT_WEB_USER_AGENT = $"Rownd SDK for Xamarin/{sdkVersion} (Language C#; Platform={DeviceInfo.Platform} {DeviceInfo.VersionString}; (SDK {sdkVersion});)";
    }
}
