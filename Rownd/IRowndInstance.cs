using System;
using System.Threading.Tasks;
using ReduxSimple;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Models.Domain;
using Rownd.Xamarin.Utils;

namespace Rownd.Xamarin
{
    public interface IRowndInstance
    {
        void RequestSignIn();
        void RequestSignIn(SignInMethod with);
        void RequestSignIn(SignInOptions opts);
        void SignOut();
        Task<string> GetAccessToken();
        Task<string> GetAccessToken(string token);
        ReduxStore<GlobalState> Store { get; }
#pragma warning disable SA1300 // Element should begin with upper-case letter
        Task _InternalTestRefreshToken();
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}
