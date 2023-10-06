using System;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Utils
{
    public enum SignInStep
    {
        [JsonProperty("init")]
        Init,

        [JsonProperty("no_account")]
        NoAccount,

        [JsonProperty("success")]
        Success,

        [JsonProperty("completing")]
        Completing,

        [JsonProperty("error")]
        Error,
    }
}
