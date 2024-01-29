using System;
using Android.Graphics;
using AndroidX.Core.View;
using Rownd.Xamarin.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;
using XEssentials = Xamarin.Essentials;

namespace Rownd.Xamarin.Android
{
    public class PlatformUtils : IPlatformUtils
    {
        public PlatformUtils()
        {
        }

        public Thickness GetWindowSafeArea()
        {
            var rootView = XEssentials.Platform.CurrentActivity.Window.DecorView.RootView;
            var insets = ViewCompat.GetRootWindowInsets(rootView)?.GetInsets(WindowInsetsCompat.Type.SystemBars());

            return new Thickness
            {
                Top = PixelsToDp(insets.Top),
                Bottom = PixelsToDp(insets.Bottom),
                Right = PixelsToDp(insets.Right),
                Left = PixelsToDp(insets.Left),
            };
        }

        public static double PixelsToDp(float pixelValue)
        {
            var density = DeviceDisplay.MainDisplayInfo.Density;
            var dp = pixelValue / density;
            return dp;
        }

        public double GetWindowHeight()
        {
            var rootView = XEssentials.Platform.CurrentActivity.Window.DecorView.RootView;
            return PixelsToDp(rootView.Height);
        }
    }
}
