using Android.Content;
using Rownd.Xamarin.Android.Hub;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Hub;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XEssentials = Xamarin.Essentials;

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
                Control.Settings.UserAgentString = Constants.DEFAULT_WEB_USER_AGENT;

                // Listen for layout changes like the soft keyboard opening
                var rootView = XEssentials.Platform.CurrentActivity.Window.DecorView.RootView;
                rootView.ViewTreeObserver.AddOnGlobalLayoutListener(new CustomLayoutListener(this));
            }
        }

        public HubWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
    }
}