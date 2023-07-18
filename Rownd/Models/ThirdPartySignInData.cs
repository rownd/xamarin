using System;
using System.Collections.Generic;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Utils;

namespace Rownd.Xamarin.Models
{
    public class ThirdPartySignInData
    {
        public string Token { get; set; }
        public SignInIntent? Intent { get; set; }
        public SignInMethod SignInMethod { get; set; }
        public Dictionary<string, string> UserData { get; set; }
    }
}
