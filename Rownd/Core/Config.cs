using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Rownd.Core
{
	public class Config
	{
        public String appKey { get; set; }
        public String hubUrl { get; set; } = "https://hub.rownd.io";
        public String apiUrl { get; set; } = "https://api.rownd.io";
        public String postSignInRedirect { get; set; } = "NATIVE_APP";
        public String appleIdCallbackUrl { get; set; } = "https://api.rownd.io/hub/auth/apple/callback";
        public String subdomainExtension { get; set; } = ".rownd.link";
        public long defaultRequestTimeout { get; set; } = 15000;
        public int defaultNumApiRetries { get; set; } = 5;

        public Customizations customizations = new Customizations();

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions() {};

        public Config()
		{
		}

        public async Task<String> GetHubLoaderUrl()
        {
            var jsonConfig = JsonSerializer.Serialize(this, jsonOptions);
            var base64Config = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonConfig), Base64FormattingOptions.None);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("config", base64Config);

            // val signInState = signInRepo.get()
            // val signInInitStr = signInState.toSignInInitHash()
            // uriBuilder.appendQueryParameter("sign_in", signInInitStr)

            UriBuilder uriBuilder = new UriBuilder($"{hubUrl}/mobile_app");
            uriBuilder.Query = queryString.ToString();


            //try {
            //    val authState = authRepo.getLatestAuthState() ?: AuthState()
            //    val rphInitStr = authState.toRphInitHash(userRepo)
            //    uriBuilder.encodedFragment("rph_init=$rphInitStr")
            //} catch (error: Exception) {
            //    Log.d("Rownd.config", "Couldn't compute requested init hash: ${error.message}")
            //}

            return uriBuilder.ToString();
        }
	}
}

