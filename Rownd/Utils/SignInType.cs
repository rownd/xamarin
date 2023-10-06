using System;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Utils
{
    public enum SignInType
    {
        [JsonProperty("email")]
        Email,

        [JsonProperty("phone")]
        Phone,

        [JsonProperty("apple")]
        Apple,

        [JsonProperty("google")]
        Google,

        [JsonProperty("passkey")]
        Passkey,

        [JsonProperty("anonymous")]
        Anonymous,
    }
}
