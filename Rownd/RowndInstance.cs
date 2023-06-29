using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReduxSimple;
using Rownd.Controls;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Hub;
using Rownd.Xamarin.Models;
using Rownd.Xamarin.Models.Domain;
using Rownd.Xamarin.Models.Repos;
using Rownd.Xamarin.Utils;
using Xamarin.Forms;

namespace Rownd.Xamarin
{
    public class RowndInstance : IRowndInstance
    {
        private static RowndInstance inst;

        internal StateRepo State { get; private set; }
        internal AuthRepo Auth { get; private set; }
        public UserRepo User { get; private set; }

        public Config Config { get; set; }
        public ReduxStore<GlobalState> Store
        {
            get
            {
                return State.Store;
            }
        }

        private RowndInstance(Application app, Config config = null)
        {
            Shared.Init(app, config);
            Config = Shared.ServiceProvider.GetService<Config>();
            State = StateRepo.Get();
            Auth = AuthRepo.Get();
            User = UserRepo.GetInstance();
            State.Setup();
        }

        public static RowndInstance GetInstance(Application app, Config config = null)
        {
            inst ??= new RowndInstance(app, config);

            return inst;
        }

        public RowndInstance Configure(string appKey)
        {
            var config = Shared.ServiceProvider.GetService<Config>();

            config.AppKey = appKey;

            return inst;
        }

        public void RequestSignIn()
        {
            DisplayHub(HubPageSelector.SignIn);
        }

        public void RequestSignIn(SignInMethod with)
        {
            RequestSignIn(new SignInOptions());
        }

        public void RequestSignIn(SignInOptions opts) {
            
        }

        public void SignOut()
        {
            Store.Dispatch(new StateActions.SetAuthState() { AuthState = new AuthState() });
            Store.Dispatch(new StateActions.SetUserState() { UserState = new UserState() });
        }

        public async Task<string> GetAccessToken()
        {
            return await Auth.GetAccessToken();
        }

        public async Task<string> GetAccessToken(string token)
        {
            return await Auth.GetAccessToken(token);
        }

        [Obsolete("Use GetAccessToken() instead")]
        public async Task _InternalTestRefreshToken()
        {
            await Task.WhenAll(
                Auth.RefreshToken(),
                Auth.RefreshToken(),
                Auth.RefreshToken()
            );
        }

        #region Internal methods
        private void DisplayHub(HubPageSelector page, RowndSignInJsOptions opts = null)
        {
            var hubBottomSheet = new HubBottomSheetPage();
            var webView = hubBottomSheet.GetHubWebView();
            webView.TargetPage = page;
            webView.HubOpts = opts;
            webView.RenderHub();

            Shared.App.MainPage.Navigation.PushModalAsync(hubBottomSheet, false);
        }

        #endregion
    }
}
