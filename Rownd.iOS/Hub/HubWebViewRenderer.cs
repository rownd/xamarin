using System;
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
            Console.WriteLine("iOS base constructor");
        }

        public HubWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            Console.WriteLine("iOS config constructor");
            userController = config.UserContentController;
            userController.AddScriptMessageHandler(this, "rowndIosSDK");
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            ((HubWebView)Element).HandleHubMessage(message.Body.ToString());
        }
    }
}
