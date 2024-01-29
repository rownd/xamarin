using System;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.View;
using XEssentials = Xamarin.Essentials;

namespace Rownd.Xamarin.Android.Hub
{
    public class CustomLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly HubWebViewRenderer renderer;
        private bool isKeyboardOpen = false;

        public CustomLayoutListener(HubWebViewRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void OnGlobalLayout()
        {
            var rootView = XEssentials.Platform.CurrentActivity.Window.DecorView.RootView;

            Rect r = new Rect();
            XEssentials.Platform.CurrentActivity.Window.DecorView.GetWindowVisibleDisplayFrame(r);

            var navInsets = ViewCompat.GetRootWindowInsets(rootView)?.GetInsets(WindowInsetsCompat.Type.NavigationBars());

            var isKeyboardOpen = ViewCompat.GetRootWindowInsets(rootView)?.IsVisible(WindowInsetsCompat.Type.Ime()) ?? true;
            if (isKeyboardOpen != this.isKeyboardOpen)
            {
                ((Rownd.Xamarin.Hub.HubWebView)renderer.Element)?.HandleKeyboardStateChange(isKeyboardOpen, rootView.Height - r.Bottom - (navInsets?.Bottom ?? 0));
                this.isKeyboardOpen = isKeyboardOpen;
            }
        }
    }
}