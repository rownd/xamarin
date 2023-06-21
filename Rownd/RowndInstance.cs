using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReduxSimple;
using Rownd.Controls;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Models;
using Rownd.Xamarin.Models.Domain;
using Rownd.Xamarin.Models.Repos;
using Xamarin.Forms;

namespace Rownd.Xamarin
{
    public class RowndInstance : IRowndInstance
    {
        private static RowndInstance inst;

        internal StateRepo State { get; set; }
        internal AuthRepo Auth { get; set; }

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
            DisplayHub();
        }

        public void RequestSignIn(SignInMethod with)
        {

        }

        public void SignOut()
        {
            Store.Dispatch(new StateActions.SetAuthState() { AuthState = new AuthState() });
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
        private void DisplayHub()
        {
            Shared.App.MainPage.Navigation.PushModalAsync(new HubBottomSheetPage(), false);
        }

        #endregion
    }
}
