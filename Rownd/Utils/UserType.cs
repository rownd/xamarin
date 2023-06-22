using Newtonsoft.Json;

namespace Rownd.Xamarin.Utils
{
    public enum UserType
    {
        [JsonProperty("new_user")]
        NewUser,

        [JsonProperty("existing_user")]
        ExistingUser,
    }
}
