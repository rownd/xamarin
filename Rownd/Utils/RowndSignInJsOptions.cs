using System;
using Newtonsoft.Json;
using Rownd.Xamarin.Core;

namespace Rownd.Xamarin.Utils
{
    public class RowndSignInJsOptions
    {
        [JsonProperty("post_login_redirect")]
        public string PostSignInRedirect { get; set; } = Config.GetConfig().PostSignInRedirect;

        public string Token { get; set; } = null;

        [JsonProperty("login_step")]
        public SignInStep SignInStep { get; set; }

        public SignInIntent Intent { get; set; }

        [JsonProperty("user_type")]
        public UserType UserType { get; set; }

        [JsonProperty("sign_in_type")]
        public SignInType SignInType { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; } = null;

        public string ToJsonString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
