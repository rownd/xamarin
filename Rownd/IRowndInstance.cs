using System;
using System.Threading.Tasks;
using ReduxSimple;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Models.Domain;
using Rownd.Xamarin.Models.Repos;
using Rownd.Xamarin.Utils;
using static Rownd.Xamarin.RowndInstance;

namespace Rownd.Xamarin
{
    public interface IRowndInstance
    {
        void RequestSignIn();
        void RequestSignIn(SignInMethod with);
        void RequestSignIn(SignInOptions opts);
        void SignOut();
        void ManageAccount();
        void ManageAccount(RowndSignInJsOptions opts);
        Task<string> GetAccessToken();
        Task<string> GetAccessToken(string token);
        Task<string> GetAccessToken(RowndTokenOpts opts);
        ReduxStore<GlobalState> Store { get; }
        UserRepo User { get; }
#pragma warning disable SA1300 // Element should begin with upper-case letter
        Task _InternalTestRefreshToken();
#pragma warning restore SA1300 // Element should begin with upper-case letter

        event EventHandler<RowndEventArgs> Events;
    }
}
