using Android.Content;
using Rownd.Xamarin.Android.Hub;
using Rownd.Xamarin.Hub;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HubWebView), typeof(HubWebViewRenderer))]
namespace Rownd.Xamarin.Android.Hub
{
    public class HubWebViewRenderer : WebViewRenderer
    {
        private Context _context;

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
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