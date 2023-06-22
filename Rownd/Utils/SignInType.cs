using System;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Utils
{
    public enum SignInType
    {
        [JsonProperty("passkey")]
        Passkey,

        [JsonProperty("anonymous")]
        Anonymous
    }
}
