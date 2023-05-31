using System;
using Foundation;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Rownd.HubWebView.HubWebView), typeof(Rownd.iOS.HubWebView.HubWebViewRenderer))]
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

