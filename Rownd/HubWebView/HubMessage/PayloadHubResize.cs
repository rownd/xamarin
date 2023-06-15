using JsonKnownTypes;
using Newtonsoft.Json;

namespace Rownd.HubWebView.HubMessage
{
    [JsonKnownThisType("hub_resize")]
    public class PayloadHubResize : PayloadBase
    {
        [JsonProperty("height")]
        public int Height { get; set; }
    }
}

