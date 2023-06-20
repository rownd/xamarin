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
        private HubBottomSheetPage bottomSheet { get; set; }

        public HubWebView()
        {
            Navigated += OnPageLoaded;

            this.FadeTo(0, 0);

            RenderHub();
        }

        private async void RenderHub()
        {
            var url = await config.GetHubLoaderUrl();
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                Source = url;
            });
        }

        public void TriggerHub()
        {
            evaluateJavaScript("rownd.requestSignIn();");
        }

        protected void evaluateJavaScript(string code)
        {
            var wrappedJs = $@"
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

        public void HandleHubMessage(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
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

                                await Task.Delay(2000);

                                await bottomSheet.Dismiss();
                                break;
                            }

                        case MessageType.HubLoaded:
                            {
                                await this.FadeTo(1, 500);
                                bottomSheet.IsLoading = false;
                                break;
                            }

                        case MessageType.HubResize:
                            {
                                Console.WriteLine($"Hub resize request: {hubMessage.Payload}");
                                _ = bottomSheet.RequestHeight((hubMessage.Payload as PayloadHubResize).Height);
                                break;
                            }

                        case MessageType.CanTouchBackgroundToDismiss:
                            {
                                Console.WriteLine($"Hub dismissable change: {hubMessage.Payload}");
                                bottomSheet.IsDismissable = (hubMessage.Payload as PayloadCanTouchBackgroundToDismiss).Enable;
                                break;
                            }

                        case MessageType.CloseHub:
                            {
                                Console.WriteLine($"Hub close request");
                                _ = bottomSheet.Dismiss();
                                break;
                            }

                        case MessageType.UserDataUpdate:
                            {
                                Console.WriteLine($"User data received: {hubMessage.Payload}");
                                stateRepo.Store.Dispatch(new StateActions.SetUserState
                                {
                                    UserState = new UserState()
                                    {
                                        Data = (hubMessage.Payload as PayloadUserDataUpdate).Data
                                    }
                                });
                                break;
                            }

                        default:
                            {
                                Console.WriteLine($"No handler for message type '{hubMessage.Type}'.");
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to decode hub message: {e.Message}");
                }
            });
        }

        public void OnPageLoaded(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.Contains("rownd.io"))
            {
                TriggerHub();
            }
        }

        public void SetBottomSheetParent(HubBottomSheetPage bottomSheet)
        {
            this.bottomSheet = bottomSheet;
        }
    }
}