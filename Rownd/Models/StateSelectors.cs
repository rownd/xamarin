using ReduxSimple;
using Rownd.Xamarin.Models.Domain;
using static ReduxSimple.Selectors;

namespace Rownd.Xamarin.Models
{
    public static class StateSelectors
    {
        public static ISelectorWithoutProps<GlobalState, AppConfigState> SelectAppConfigState { get; } = CreateSelector(
            (GlobalState state) => state.AppConfig
        );

        public static ISelectorWithoutProps<GlobalState, AuthState> SelectAuthState { get; } = CreateSelector(
            (GlobalState state) => state.Auth
        );

        public static ISelectorWithoutProps<GlobalState, UserState> SelectUserState { get; } = CreateSelector(
            (GlobalState state) => state.User
        );
    }
}
