namespace Rownd.Xamarin.Models.Domain
{
    public class GlobalState
    {
        public bool IsInitialized { get; set; } = false;
        public AppConfigState AppConfig { get; set; } = new AppConfigState();
        public AuthState Auth { get; set; } = new AuthState();
        public UserState User { get; set; } = new UserState();
        public SignInState SignIn { get; set; } = new SignInState();
    }
}
