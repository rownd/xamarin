using System;
using System.ComponentModel;
using System.Net.Sockets;
using JWT;
using JWT.Builder;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Rownd.Models.Domain
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
                try
                {
                    DecodeToken();
                    return true;
                }
                catch
                {
                    return false;
                }
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
