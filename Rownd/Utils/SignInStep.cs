using System;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Utils
{
    public enum SignInStep
    {
        [JsonProperty("init")]
        Init,

        [JsonProperty("success")]
        Success,

        [JsonProperty("no_account")]
        NoAccount,

        [JsonProperty("error")]
        Error,
    }
}
