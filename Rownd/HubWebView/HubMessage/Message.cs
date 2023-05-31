using System;
using JsonKnownTypes;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rownd.HubWebView.HubMessage
{
    public class Message
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public MessageType Type { get; set; }

        //[JsonConverter(typeof(JsonSubtypes))]
        public PayloadBase Payload { get; set; }
	}
}

