using System;
using System.Data;
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
        private readonly Config config = Shared.ServiceProvider.GetService<Config>();
        private readonly StateRepo stateRepo = StateRepo.Get();
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
            HubOpts = opts ?? new RowndSignInJsOptions();
            TargetPage = page;
        }

        internal async void RenderHub()
        {
            var url = await config.GetHubLoaderUrl();
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                var connectionState = Connectivity.NetworkAccess;
                if (connectionState != NetworkAccess.Internet)
                {
                    Source = new HtmlWebViewSource
                    {
                        Html = NoInternet.Build(Shared.Rownd)
                    };
                    await this.FadeTo(1, 500);
                    bottomSheet.IsLoading = false;
                    bottomSheet.RequestHeight(400);
                    return;
                }

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
            SetFeatureFlagsJS();

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
                        EvaluateJavaScript($"rownd.user.manageAccount({HubOpts?.ToJsonString()});");
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

                                await UserRepo.GetInstance().FetchUser();

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
                                Shared.Rownd.FireEvent(new PayloadEvent
                                {
                                    Event = "user_data_update",
                                    Data = (hubMessage.Payload as PayloadUserDataUpdate).Data
                                });
                                break;
                            }

                        case MessageType.TriggerSignInWithApple:
                            {
                                Shared.Rownd.RequestSignIn(SignInMethod.Apple);
                                break;
                            }

                        case MessageType.Event:
                            {
                                Shared.Rownd.FireEvent((PayloadEvent)hubMessage.Payload);
                                break;
                            }

                        case MessageType.TryAgain:
                            {
                                Source = null;
                                RenderHub();
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

        public bool HandleLinkActivation(string linkUrl)
        {
            // Load only Rownd-related URLs in the webview
            string[] allowedWebViewUrls =
            {
                "https://appleid.apple.com/auth/authorize",
                config.HubUrl,
                "about:"
            };

            foreach (string url in allowedWebViewUrls)
            {
                if (linkUrl.StartsWith(url))
                {
                    return true;
                }
            }

            Device.InvokeOnMainThreadAsync(async () =>
            {
                await Launcher.OpenAsync(new Uri(linkUrl));
            });

            return false;
        }

        public void WebView_Navigating(object sender, WebNavigatingEventArgs args)
        {
            // iOS WKWebView uses a special handler in its custom renderer
            // due to behavioral differences
            if (Device.RuntimePlatform == Device.iOS)
            {
                return;
            }

            if (HandleLinkActivation(args.Url))
            {
                return;
            }

            args.Cancel = true;
        }

        private void SetFeatureFlagsJS()
        {
            var supportedFeaturesStr = Constants.GetSupportedFeatures();
            var code = @$"
if (rownd?.setSessionStorage) {{
    rownd.setSessionStorage('rph_feature_flags',`{supportedFeaturesStr}`);
}}
";
            EvaluateJavaScript(code);
        }
    }
}