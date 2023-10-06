using System;
using System.Collections.Generic;
using GuerrillaNtp;
using JWT;
using JWT.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rownd.Xamarin.Core;

namespace Rownd.Xamarin.Utils
{
    public class Jwt
    {
        public Jwt() { }

        public static bool IsJwtValid(string token)
        {
            try
            {
                var valParams = ValidationParameters.Default;
                valParams.ValidateSignature = false;
                valParams.TimeMargin = 60;
                var json = JwtBuilder.Create()
                    .WithValidationParameters(valParams)
                    .Decode(token);

                // Get claims as dictionary
                var jwtClaims = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (jwtClaims["exp"] == null)
                {
                    return false;
                }

                // Coerce exp to long
                var jwtExp = Convert.ToInt64(jwtClaims["exp"]);
                var jwtExpTime = DateTimeOffset.FromUnixTimeSeconds(jwtExp).DateTime;
                var ntpClient = Shared.ServiceProvider.GetService<NtpClient>();
                var currentTime = ntpClient.Last?.UtcNow.UtcDateTime ?? DateTime.UtcNow;

                return jwtExpTime > currentTime.AddMinutes(1);
            }
            catch
            {
                return false;
            }
        }
    }
}
