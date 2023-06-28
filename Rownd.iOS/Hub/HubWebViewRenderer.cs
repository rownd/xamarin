using System;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Hub;
using Rownd.Xamarin.iOS.Hub;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HubWebView), typeof(HubWebViewRenderer))]
namespace Rownd.Xamarin.iOS.Hub
{
    public class HubWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        private WKUserContentController userController;

        public HubWebViewRenderer() : this(new WKWebViewConfiguration())
        {
            CustomUserAgent = Constants.DEFAULT_WEB_USER_AGENT;
        }

        public HubWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            userController.AddScriptMessageHandler(this, "rowndIosSDK");
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            ((HubWebView)Element).HandleHubMessage(message.Body.ToString());
        }
    }
}
