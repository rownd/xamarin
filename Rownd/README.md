# Rownd SDK for Xamarin

The Rownd SDK for Xamarin provides authetnication, account and user profile management, deep linking, and more for cross-platform Xamarin apps.

Using the Rownd platform, you can easily bring the same authentication that's on your website to your mobile apps. Or if you only authenticate users on your mobile apps, you can streamline the authentication process using Rownd's passwordless sign-in links, enabling you to seamlessly authenticate users from an app link sent to their email or phone number.

Once a user is authenticated, you can retrieve and update their profile information on the fly using native APIs. Leverage Rownd's pre-built mobile app components to give users profile management tools. Additionally, you can manage encryption of data on-device before sending it back to Rownd or your own backend.

## Getting started

### Prerequisites

- .NET Standard 2.1 or higher
- Android API 26 or higher (Android 8+)
- iOS/iPadOS 14 or higher

### Installation

Rownd provides a core shared library alongside platform-specific libraries that contain a few critical linkages into native code.

In your shared app project, add a NuGet dependency on `Rownd.Xamarin`.

```bash
dotnet add package Rownd.Xamarin
```

In your platform-specific application projects, add the applicable NuGet package as follows:

**iOS**
```
dotnet add package Rownd.Xamarin.iOS
```

**Android**
```
dotnet add package Rownd.Xamarin.Android
```

## Usage

### Initialize the SDK

In your shared app project, open the main entry point (usually `App.xaml.cs`) and add the following code to the constructor. A basic example might look something like this:

```C#
using Rownd.Xamarin;
using RowndXamarinExample.Services;
using Xamarin.Forms;

namespace RowndXamarinExample
{
    public partial class App : Application
    {
        public RowndInstance Rownd;

        public App()
        {
            Rownd = RowndInstance.GetInstance(this);
            Rownd.Configure("YOUR_APP_KEY");

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

```

If you're using `DependencyService`, you might want to add Rownd to make it easily accessible across the project:

```
DependencyService.RegisterSingleton<IRowndInstance>(Rownd);
```

### iOS only - pre-init
In order to ensure that certain native extensions get loaded early enough, add the following line to your `AppDelegate.cs` file:

```C#
Rownd.Xamarin.iOS.Boot.Init();
```

Example:

```C#
namespace RowndXamarinExample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rownd.Xamarin.iOS.Boot.Init();  // <-- ADD THIS BEFORE XAMARIN FORMS INIT

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
```

### Handling authentication state

Rownd leverages the concept of observables so that your app can react to changes in state without requiring complex logic.

Generally, you'll want to listen to updates from Rownd's state and then trigger actions (e.g., navigating to a new page) or update properties on your ViewModels based on the changed values.

Here's an example of a ViewModel that subscribes to the authentication state. A XAML view might adjust its layout based on `AuthState.IsAuthenticated`.

```C#
namespace RowndXamarinExample.ViewModels
{
    public class ExampleViewModel : BaseViewModel
    {
        public IRowndInstance Rownd => DependencyService.Get<IRowndInstance>();
        
        public AuthState AuthState { get; set; }
        
        public ExampleViewModel()
        {
            Rownd.Store.Select(state => state.Auth).Subscribe((authState) =>
            {
                AuthState = authState;
                OnPropertyChanged("AuthState");
            });
        }
}
    }
}
```

Once the state subscription is initialized, the lambda function will fire any time a change in state occurs that is different from the previous state.

The following properties are available as portions of the state:

#### `.AuthState`

```C#
class AuthState
{
    public string AccessToken { get; }
    public string RefreshToken { get; }
    public bool IsAuthenticated { get; }
    public bool IsAccessTokenValid { get; }
}
```

#### `.UserState`

```C#
class UserState
{
    public string Id { get; }
    public Dictionary<string, dynamic> Data { get; }
}
```

## API reference

In addition to the state APIs, Rownd provides imperative APIs that you can call to request sign in, get and retrieve user profile information, retrieve a current access token, and so on.

### `void Rownd.RequestSignIn()`
Opens the Rownd sign-in dialog to begin authentication.


### `void Rownd.RequestSignIn(RowndSignInOpts(...))`
Opens the Rownd sign-in dialog for authentication, as before, but allows passing additional context options as shown below.

`intent: RowndSignInIntent` - This option applies only when you have opted to split the sign-up/sign-in flow via the Rownd dashboard. Valid values are .SignIn or .SignUp. If you don’t set this value, the user will be presented with the unified sign-in/sign-up flow. Please reach out to support@rownd.io to enable.

`postSignInRedirect (Not recommended): String` - If a subdomain is provided in the Rownd dashboard, this behavior will work by default. When the user completes the authentication challenge via email or SMS, they'll be redirected to the URL set for postSignInRedirect. If this is an Android App Link, it will redirect the user back to your app.

Example:

```C#
Rownd.requestSignIn(RowndSignInOpts(
    intent = RowndSignInIntent.SignUp
))
```

### `async Task<string> Rownd.GetAccessToken()`
Assuming a user is signed-in, returns a valid access token, refreshing the current one if needed. If an access token cannot be returned due to a temporary condition (e.g., inaccessible network), this function will throw. If an access token cannot be returned because the refresh token is invalid, null will be returned and the Rownd authentication state will flip to `IsAuthenticated = false`.

### `async Task<string> Rownd.GetAccessToken(token: String)`
When possible, exchanges a non-Rownd access token for a Rownd access token. This is primarily used in scenarios where an app is migrating from some other authentication mechanism to Rownd. Using Rownd integrations, the system will accept a third-party token. If it successfully validates, Rownd will sign-in the user and return a fresh Rownd access token to the caller.

This API returns null if the token could not be validated and exchanged. If that occurs, it's likely that the user should sign-in normally via Rownd.requestSignIn().

> NOTE: This API is typically used once. After a Rownd token is available, other tokens should be discarded.

Example:

```C#
    // Assume `oldToken` was retrieved from some prior authenticator.
    var accessToken = await Rownd.GetAccessToken(oldToken);

    if (accessToken != null) {
        // Navigate to the UI that an authenticated user should typically see
    } else {
        Rownd.RequestSignIn()
    }
```

> NOTE: The following user profile APIs technically accept `dynamic` as the value of a field. However, that value **must** be serializable using the [Newtonsoft JSON.NET library](https://www.newtonsoft.com/json). If the value is not serializable out of the box, you'll need to serialize it to a string prior to storing it in the user object, then deserialize it when getting it back out.

### `Dictionary<string, dynamic> Rownd.User.Get()`
Returns the entire user profile as a Dictionary

### `<T> Rownd.User.Get<T>(field: string)`
Returns the value of a specific field in the user's data Dictionary. "id" is a special case that will return the user's ID, even though it's technically not in the Dictionary itself.

Your application code is responsible for knowing which type the value should cast to. If the cast fails or the entry doesn't exist, a null value will be returned.

### `void Rownd.User.Set(data: Dictionary<string, dynamic>)`
Replaces the user's data with that contained in the Dictionary. This may overwrite existing values, but must match the schema you defined within your Rownd application dashboard.

### `void Rownd.User.Set(field: string, value: dynamic)`
Sets a specific user profile field to the provided value, overwriting if a value already exists.
