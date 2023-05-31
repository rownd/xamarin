using System;
using Xamarin.Forms;

using Rownd.HubWebView;

[assembly: Dependency(typeof(Rownd.HubWebView.iOS.HubWebViewiOS))]
namespace Rownd.HubWebView.iOS
{
	public class HubWebViewiOS : WebView, IHubWebView
    {
		public HubWebViewiOS()
		{
            Console.WriteLine("Running iOS WebView");
		}

        public void AddJavascriptListener(string eventName)
        {
            Console.WriteLine("Initialized JS on iOS!!");
            
        }
    }
}

