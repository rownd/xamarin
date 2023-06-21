using JsonKnownTypes;
using Newtonsoft.Json;

namespace Rownd.Xamarin.Hub.HubMessage
{
    [JsonKnownThisType("can_touch_background_to_dismiss")]
    public class PayloadCanTouchBackgroundToDismiss : PayloadBase
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }
    }
}