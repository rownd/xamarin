using System;
using ReduxSimple;
using static ReduxSimple.Reducers;
using System.Collections.Generic;
using Rownd.Models.Domain;

namespace Rownd.Models
{
	public static class StateReducers
	{
        public static IEnumerable<On<GlobalState>> CreateReducers()
        {
            return CombineReducers(
                GetAppConfigReducers(),
                GetAuthReducers()
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
    }
}

