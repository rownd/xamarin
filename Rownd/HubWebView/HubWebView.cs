using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rownd.Controls;
using Rownd.Core;
using Rownd.HubWebView.HubMessage;
using Rownd.Models;
using Rownd.Models.Domain;
using Rownd.Models.Repos;
using Xamarin.Forms;

namespace Rownd.HubWebView
{
    public class HubWebView : WebView, IBottomSheetChild
    {
        private Config config = Shared.ServiceProvider.GetService<Config>();
        private StateRepo stateRepo = StateRepo.Get();
        private HubPageRelative bottomSheet { get; set; }

        public HubWebView()
        {
            this.Navigated += OnPageLoaded;

            this.FadeTo(0, 0);

            RenderHub();
        }

        private async void RenderHub()
        {
            var url = await config.GetHubLoaderUrl();
            Dispatcher.BeginInvokeOnMainThread(() => {
                this.Source = url;
            });
        }

        public async void TriggerHub()
        {
            // Access the platform-specific implementation using DependencyService
            //var customWebView = DependencyService.Get<IHubWebView>();
            //customWebView?.AddJavascriptListener("eventName");
            this.evaluateJavaScript("rownd.requestSignIn();");
        }

        protected void evaluateJavaScript(String code)
        {
            String wrappedJs = $@"
if (typeof rownd !== 'undefined') {{
    {code}
}} else {{
    _rphConfig.push(['onLoaded', () => {{
        {code}
    }}]);
}}
".Replace(System.Environment.NewLine, "");

            this.EvaluateJavaScriptAsync(wrappedJs);
        }

        public void HandleHubMessage(String message)
        {
            Console.WriteLine($"Received message: {message}");
            try
            {
                var hubMessage = JsonConvert.DeserializeObject<Message>(message, new JsonConverterPayload());

                switch (hubMessage.Type)
                {
                    case MessageType.Authentication:
                        {
                            Console.WriteLine($"Received auth payload: {hubMessage.Payload}");
                            stateRepo.Store.Dispatch(new StateActions.SetAuthState
                            {
                                AuthState = new AuthState()
                                {
                                    AccessToken = (hubMessage.Payload as PayloadAuthenticated).AccessToken,
                                    RefreshToken = (hubMessage.Payload as PayloadAuthenticated).RefreshToken
                                }
                            });
                            break;
                        }
                    case MessageType.HubLoaded:
                        {
                            this.FadeTo(1, 500);
                            break;
                        }
                    case MessageType.HubResize:
                        {
                            Console.WriteLine($"Hub resize request: {hubMessage.Payload}");
                            _ = bottomSheet.RequestHeight((hubMessage.Payload as PayloadHubResize).Height);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"No handler for message type '{hubMessage.Type}'.");
                            break;
                        }
                }
            } catch (Exception e)
            {
                Console.WriteLine($"Failed to decode hub message: {e.Message}");
            }
        }

        public void OnPageLoaded(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.Contains("rownd.io"))
            {
                TriggerHub();
            }
        }

        public void SetBottomSheetParent(HubPageRelative bottomSheet)
        {
            this.bottomSheet = bottomSheet;
        }
    }
}

