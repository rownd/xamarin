using System;
using JsonKnownTypes;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Hub.HubMessage
{
    [JsonKnownThisType("authentication")]
    public class PayloadAuthenticated : PayloadBase
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
