using System;
using Microsoft.Extensions.DependencyInjection;
using Rownd.Core;
using Rownd.Models;
using Rownd.Models.Repos;
using Xamarin.Forms;

namespace Rownd
{
    public class RowndInstance : IRowndInstance
    {
        private static RowndInstance inst;

        public Config config;
        public StateRepo state;

        private RowndInstance(Application app,  Config config = null){
            Shared.Init(app, config);
            config = Shared.ServiceProvider.GetService<Config>();
            state = StateRepo.Get();
            state.Setup();
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

