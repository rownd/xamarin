using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Rownd.Xamarin.Core;

namespace Rownd.Xamarin.Utils
{
    public class RowndSignInJsOptions
    {
        [JsonProperty("post_login_redirect")]
        public string PostSignInRedirect { get; set; } = Config.GetConfig().PostSignInRedirect;

        public string Token { get; set; } = null;

        [JsonProperty("login_step")]
        [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
        public SignInStep SignInStep { get; set; }

        public SignInIntent? Intent { get; set; }

        [JsonProperty("user_type")]
        [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
        public UserType UserType { get; set; }

        [JsonProperty("sign_in_type")]
        [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
        public SignInType? SignInType { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; } = null;

        [JsonProperty("visible_profile_fields")]
        public string[] VisibleProfileFields { get; set; }

        [JsonProperty("auto_focus_field")]
        public string AutoFocusField { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
