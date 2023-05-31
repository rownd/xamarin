using System;
using Xamarin.Forms;

namespace Rownd.HubWebView
{
	public interface IHubWebView
	{
        void AddJavascriptListener(string eventName);
    }
}

