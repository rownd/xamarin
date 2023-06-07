using System;
using System.ComponentModel;
using System.Net.Sockets;
using JWT;
using JWT.Builder;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Rownd.Models.Domain
{
    public class AuthState : StateBase
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public Boolean IsAuthenticated
        {
            get { return AccessToken != null; }
        }
        public Boolean IsAccessTokenValid
        {
            get
            {
                try
                {
                    decodeToken();
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
            //this.PropertyChanged += this.onAuthChanged;
        }

        //public AuthState(string accessToken, string refreshToken)
        //{
        //    AccessToken = accessToken;
        //    RefreshToken = refreshToken;
        //}

        private void decodeToken()
        {
            var valParams = ValidationParameters.Default;
            valParams.ValidateSignature = false;
            valParams.TimeMargin = 60;
            var json = JwtBuilder.Create()
                .WithValidationParameters(valParams)
                .Decode(AccessToken);
        }
    }
}

