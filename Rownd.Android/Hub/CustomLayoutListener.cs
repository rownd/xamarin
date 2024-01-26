using System;
using Android.Views;
using AndroidX.Core.View;
using XEssentials = Xamarin.Essentials;

namespace Rownd.Xamarin.Android.Hub
{
    public class CustomLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly HubWebViewRenderer renderer;

        public CustomLayoutListener(HubWebViewRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void OnGlobalLayout()
        {
            var rootView = XEssentials.Platform.CurrentActivity.Window.DecorView.RootView;
            var isKeyboardOpen = ViewCompat.GetRootWindowInsets(rootView)?.IsVisible(WindowInsetsCompat.Type.Ime()) ?? true;
            ((Rownd.Xamarin.Hub.HubWebView)renderer.Element)?.HandleKeyboardStateChange(isKeyboardOpen, 0);
        }
    }
}