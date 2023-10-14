using JsonKnownTypes;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Hub.HubMessage
{
    [JsonKnownThisType("hub_resize")]
    public class PayloadHubResize : PayloadBase
    {
        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
