using System.Runtime.Serialization;

namespace Rownd.Xamarin.Utils
{
    public enum SignInIntent
    {
        [EnumMember(Value="sign_up")]
        SignUp,

        [EnumMember(Value = "sign_in")]
        SignIn
    }
}
