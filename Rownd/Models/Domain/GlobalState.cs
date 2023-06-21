namespace Rownd.Xamarin.Models.Domain
{
    public class GlobalState
    {
        public AppConfigState AppConfig { get; set; } = new AppConfigState();
        public AuthState Auth { get; set; } = new AuthState();
        public UserState User { get; set; } = new UserState();
    }
}
