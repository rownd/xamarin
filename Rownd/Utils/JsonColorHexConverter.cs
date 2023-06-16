using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Rownd.Utils
{
    internal class JsonColorHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Color)value).ToHex());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Color.FromHex(Convert.ToString(reader.Value));
        }
    }
}
