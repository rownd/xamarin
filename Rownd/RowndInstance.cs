using System;
using Microsoft.Extensions.DependencyInjection;
using ReduxSimple;
using Rownd.Controls;
using Rownd.Core;
using Rownd.Models;
using Rownd.Models.Domain;
using Rownd.Models.Repos;
using Xamarin.Forms;

namespace Rownd
{
    public class RowndInstance : IRowndInstance
    {
        private static RowndInstance inst;

        public Config Config;
        public StateRepo State;

        public ReduxStore<GlobalState> Store {
            get {
                return State.Store;
            }
        }

        private RowndInstance(Application app,  Config config = null){
            Shared.Init(app, config);
            Config = Shared.ServiceProvider.GetService<Config>();
            State = StateRepo.Get();
            State.Setup();
        }

        public static RowndInstance GetInstance(Application app, Config config = null)
        {
            if (inst == null)
            {
                inst = new RowndInstance(app, config);
            }

            return inst;
        }

        public RowndInstance Configure(String appKey)
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

        public String GetAccessToken()
        {
            return "foo";
        }

        public String GetAccessToken(String token)
        {
            return "bar";
        }

        #region Internal methods
        private void DisplayHub()
        {
            Shared.App.MainPage.Navigation.PushModalAsync(new HubBottomSheetPage(), false);
        }

        #endregion
    }
}

