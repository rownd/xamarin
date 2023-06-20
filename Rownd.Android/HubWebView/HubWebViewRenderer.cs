using System;
using Android.Content;
using Android.Webkit;
using Java.Interop;
using Rownd.Android.HubWebView;
using Rownd.HubWebView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HubWebView), typeof(HubWebViewRenderer))]
namespace Rownd.Android.HubWebView
{
    public class HubWebViewRenderer : WebViewRenderer
    {
        private Context _context;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("rowndAndroidSDK");
            }

            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "rowndAndroidSDK");
            }
        }

        public HubWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
    }
}