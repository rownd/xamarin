using System;
using Microsoft.Extensions.DependencyInjection;
using Rownd.Core;
using Xamarin.Forms;
using Rownd.Store;
using Redux;

namespace Rownd
{
    public class RowndInstance : IRowndInstance
    {
        private static RowndInstance inst;

        public Config config;

        //private Redux.Types.IStore<Store.State, Store.Actions> store = Redux.Store.createStore(
        //    Rownd.Store.Store.reducer,
        //    new Store.State(
        //        new Store.AuthState(null, null)
        //    ),
        //    Rownd.Store.Store.defaultEnhancer
        //);

        private Redux.Types.IStore<Store.State, Store.Actions> store = Store.Store.createStore(new Store.State(
            new Store.AuthState(null, null)
        ));

        private RowndInstance(Config config = null){
            Shared.Init(config);
            config = Shared.ServiceProvider.GetService<Config>();
        }

        public static RowndInstance GetInstance(Config config = null)
        {
            if (inst == null)
            {
                inst = new RowndInstance(config);
            }

            return inst;
        }

        public static RowndInstance Configure(String appKey)
        {
            GetInstance();
            var config = Shared.ServiceProvider.GetService<Config>();

            config.appKey = appKey;

            return inst;
        }

        public void RequestSignIn()
        {

        }

        public void RequestSignIn(SignInMethod with)
        {

        }

        public String GetAccessToken()
        {
            return "foo";
        }

        public String GetAccessToken(String token)
        {
            return "bar";
        }
    }
}

