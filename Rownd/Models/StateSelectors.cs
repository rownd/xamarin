using System;
using ReduxSimple;
using static ReduxSimple.Selectors;
using Rownd.Models.Domain;


namespace Rownd.Models
{
	public static class StateSelectors
	{
        public static ISelectorWithoutProps<GlobalState, AppConfigState> SelectAppConfigState = CreateSelector(
            (GlobalState state) => state.AppConfig
        );

        public static ISelectorWithoutProps<GlobalState, AuthState> SelectAuthState = CreateSelector(
            (GlobalState state) => state.Auth
        );
    }
}

