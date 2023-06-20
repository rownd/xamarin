using System;
using Android.Webkit;
using Java.Interop;

namespace Rownd.Android.HubWebView
{
    public class JSBridge : Java.Lang.Object
    {
        private readonly WeakReference<HubWebViewRenderer> renderer;

        public JSBridge(HubWebViewRenderer renderer)
        {
            this.renderer = new WeakReference<HubWebViewRenderer>(renderer);
        }

        [JavascriptInterface]
        [Export("postMessage")]
        public void InvokeAction(string message)
        {
            if (this.renderer != null && this.renderer.TryGetTarget(out var renderer) && renderer?.Element != null)
            {
                ((Rownd.HubWebView.HubWebView)renderer.Element).HandleHubMessage(message);
            }
        }
    }
}
