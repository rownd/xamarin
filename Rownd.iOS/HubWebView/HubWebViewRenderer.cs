using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Rownd.iOS.HubWebView;
using Rownd.HubWebView;

[assembly: ExportRenderer(typeof(HubWebView), typeof(HubWebViewRenderer))]
namespace Rownd.iOS.HubWebView
{
	public class HubWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        WKUserContentController userController;

        public HubWebViewRenderer() : this(new WKWebViewConfiguration())
		{
		}

        public HubWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            userController.AddScriptMessageHandler(this, "rowndIosSDK");
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            ((Rownd.HubWebView.HubWebView)Element).HandleHubMessage(message.Body.ToString());
        }
    }
}

