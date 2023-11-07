using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Rownd.Xamarin.Controls
{
    public static class PageExtensions
    {
        private static readonly IDictionary<Type, WindowSoftInputModeAdjust> OriginalWindowSoftInputModeAdjusts = new Dictionary<Type, WindowSoftInputModeAdjust>();

        public static void UseWindowSoftInputModeAdjust(this Page page, WindowSoftInputModeAdjust windowSoftInputModeAdjust)
        {
            var platformElementConfiguration = global::Xamarin.Forms.Application.Current.On<Android>();

            var pageType = page.GetType();
            if (!OriginalWindowSoftInputModeAdjusts.ContainsKey(pageType))
            {
                var originalWindowSoftInputModeAdjust = platformElementConfiguration.GetWindowSoftInputModeAdjust();
                OriginalWindowSoftInputModeAdjusts.Add(pageType, originalWindowSoftInputModeAdjust);
            }

            platformElementConfiguration.UseWindowSoftInputModeAdjust(windowSoftInputModeAdjust);
        }

        public static void ResetWindowSoftInputModeAdjust(this Page page)
        {
            var pageType = page.GetType();

            if (OriginalWindowSoftInputModeAdjusts.TryGetValue(pageType, out var originalWindowSoftInputModeAdjust))
            {
                OriginalWindowSoftInputModeAdjusts.Remove(pageType);

                var platformElementConfiguration = global::Xamarin.Forms.Application.Current.On<Android>();
                platformElementConfiguration.UseWindowSoftInputModeAdjust(originalWindowSoftInputModeAdjust);
            }
        }
    }
}

