using System.Collections.Generic;
using ReduxSimple;
using Rownd.Xamarin.Models.Domain;
using static ReduxSimple.Reducers;

namespace Rownd.Xamarin.Models
{
    public static class StateReducers
    {
        public static IEnumerable<On<GlobalState>> CreateReducers()
        {
            return CombineReducers(
                GetAppConfigReducers(),
                GetAuthReducers(),
                GetUserReducers()
            );
        }

        public static IEnumerable<On<GlobalState>> GetAppConfigReducers()
        {
            return CreateSubReducers(StateSelectors.SelectAppConfigState)
                .On<StateActions.SetAppConfig>(
                    (state, action) =>
                    {
                        return action.AppConfig;
                    }
            ).ToList();
        }

        public static IEnumerable<On<GlobalState>> GetAuthReducers()
        {
            return CreateSubReducers(StateSelectors.SelectAuthState)
                .On<StateActions.SetAuthState>(
                    (state, action) =>
                    {
                        return action.AuthState;
                    }
            ).ToList();
        }

        public static IEnumerable<On<GlobalState>> GetUserReducers()
        {
            return CreateSubReducers(StateSelectors.SelectUserState)
                .On<StateActions.SetUserState>(
                    (state, action) =>
                    {
                        return action.UserState;
                    }
            ).ToList();
        }
    }
}
