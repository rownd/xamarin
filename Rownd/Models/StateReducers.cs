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
                GetAppConfigReducers()
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
            //On<GoBackAction, RootState>(
            //    state =>
            //    {
            //        var newPages = state.Pages.RemoveAt(state.Pages.Length - 1);

            //        return state with {
            //            CurrentPage = newPages.LastOrDefault(),
            //            Pages = newPages
            //        };
            //    }
            //),
            //On<ResetAction, RootState>(
            //    state => state with {
            //        CurrentPage = string.Empty,
            //        Pages = ImmutableArray<string>.Empty
            //    }
            //)
        }
    }
}

