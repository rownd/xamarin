using System;
using Microsoft.Extensions.DependencyInjection;
using Rownd.Core;
using Xamarin.Forms;

namespace Rownd
{
    public class RowndInstance : IRowndInstance
    {
        public Config config;
        private static RowndInstance inst;

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

