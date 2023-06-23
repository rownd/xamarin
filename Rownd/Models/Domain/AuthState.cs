using JWT;
using JWT.Builder;
using Newtonsoft.Json;
using Rownd.Xamarin.Utils;

namespace Rownd.Xamarin.Models.Domain
{
    public class AuthState : StateBase
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public bool IsAuthenticated
        {
            get { return AccessToken != null; }
        }

        [JsonIgnore]
        public bool IsNotAuthenticated
        {
            get { return AccessToken == null; }
        }

        [JsonIgnore]
        public bool IsAccessTokenValid
        {
            get
            {
                return Jwt.IsJwtValid(AccessToken);
            }
        }

        public AuthState()
        {
        }

        private string DecodeToken()
        {
            var valParams = ValidationParameters.Default;
            valParams.ValidateSignature = false;
            valParams.TimeMargin = 60;
            return JwtBuilder.Create()
                .WithValidationParameters(valParams)
                .Decode(AccessToken);
        }
    }
}
