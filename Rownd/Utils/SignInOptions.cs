using System;
namespace Rownd.Xamarin.Utils
{
    public class SignInOptions
    {
        public string PostSignInRedirect { get; set; } = "NATIVE_APP";
        public SignInIntent Intent { get; set; }
    }
}
