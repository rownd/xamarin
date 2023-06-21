using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rownd.Xamarin.Hub.HubMessage
{
    public class Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }

        //[JsonConverter(typeof(JsonSubtypes))]
        public PayloadBase Payload { get; set; }
    }
}
