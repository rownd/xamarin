using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rownd.Controls;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Hub.HubMessage;
using Rownd.Xamarin.Models;
using Rownd.Xamarin.Models.Domain;
using Rownd.Xamarin.Models.Repos;
using Rownd.Xamarin.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Rownd.Xamarin.Hub
{
    public class HubWebView : WebView, IBottomSheetChild
    {
        private Config config = Shared.ServiceProvider.GetService<Config>();
        private StateRepo stateRepo = StateRepo.Get();
        private HubBottomSheetPage bottomSheet { get; set; }
        internal RowndSignInJsOptions HubOpts { get; set; } = new RowndSignInJsOptions();
        internal HubPageSelector TargetPage { get; set; } = HubPageSelector.SignIn;

        public HubWebView()
        {
            Navigated += OnPageLoaded;
            Navigating += WebView_Navigating;

            this.FadeTo(0, 0);
        }

        public HubWebView(HubPageSelector page, RowndSignInJsOptions opts) : this()
        {
            HubOpts = opts;
            TargetPage = page;
        }

        internal async void RenderHub()
        {
            var url = await config.GetHubLoaderUrl();
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                if (Source == null)
                {
                    Source = url;
                }
                else if (Source is UrlWebViewSource source && source.Url != url)
                {
                    Source = url;
                    TriggerHub();
                }
                else
                {
                    TriggerHub();
                }
            });
        }

        public void TriggerHub()
        {
            switch (TargetPage)
            {
                case HubPageSelector.SignIn:
                    {
                        EvaluateJavaScript($"rownd.requestSignIn({HubOpts?.ToJsonString()});");
                        break;
                    }

                case HubPageSelector.SignOut:
                    {
                        EvaluateJavaScript("rownd.signOut({\"show_success\":true});");
                        break;
                    }

                case HubPageSelector.Profile:
                    {
                        EvaluateJavaScript("rownd.user.manageAccount();");
                        break;
                    }

                case HubPageSelector.ConnectAuthenticator:
                    {
                        EvaluateJavaScript($"rownd.connectAuthenticator({HubOpts?.ToJsonString()})");
                        break;
                    }

                case HubPageSelector.None:
                    {
                        break;
                    }
            }
        }

        protected void EvaluateJavaScript(string code)
        {
            Console.WriteLine($"Executing JS: {code}");
            var wrappedJs = $@"
if (typeof rownd !== 'undefined') {{
    {code}
}} else {{
    _rphConfig.push(['onLoaded', () => {{
        {code}
    }}]);
}}
".Replace(System.Environment.NewLine, "");

            EvaluateJavaScriptAsync(wrappedJs);
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

                                // Reset last sign in state
                                stateRepo.Store.Dispatch(new StateActions.SetSignInState { SignInState = new SignInState() });
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
                                bottomSheet.RequestHeight((hubMessage.Payload as PayloadHubResize).Height);
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
                                bottomSheet.IsDismissable = true;
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

                        case MessageType.TriggerSignInWithApple:
                            {
                                Shared.Rownd.RequestSignIn(SignInMethod.Apple);
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

        public void HandleKeyboardStateChange(bool isKeyboardOpen)
        {
            if (!isKeyboardOpen)
            {
                return;
            }

            bottomSheet.Expand();
        }

        public void OnPageLoaded(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith(config.HubUrl))
            {
                TriggerHub();
            }
        }

        public void SetBottomSheetParent(HubBottomSheetPage bottomSheet)
        {
            this.bottomSheet = bottomSheet;
        }

        public void WebView_Navigating(object sender, WebNavigatingEventArgs args)
        {
            string[] allowedWebViewUrls =
            {
                "https://appleid.apple.com/auth/authorize",
                config.HubUrl
            };

            foreach (string url in allowedWebViewUrls)
            {
                if (args.Url.StartsWith(url))
                {
                    return;
                }
            }

            Launcher.OpenAsync(new Uri(args.Url));

            args.Cancel = true;
        }
    }
}