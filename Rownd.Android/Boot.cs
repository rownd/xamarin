using System;
using Rownd.Xamarin.Utils;
using Xamarin.Forms;

namespace Rownd.Xamarin.Android
{
    public class Boot
    {
        public static void Init()
        {
            DependencyService.Register<ISignInLinkHandler, SignInLinkHandler>();
            DependencyService.Register<IPlatformUtils, PlatformUtils>();
        }
    }
}
