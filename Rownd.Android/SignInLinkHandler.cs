using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Rownd.Xamarin.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

//[assembly: Dependency(typeof(Rownd.Xamarin.Android.SignInLinkHandler))]
namespace Rownd.Xamarin.Android
{
    public class SignInLinkHandler : ISignInLinkHandler
    {
        public SignInLinkHandler()
        {
        }

        public Task<string> HandleSignInLink()
        {
            return Task.Run(SignInWithLinkIfPresentOnLaunchOrClipboard);
        }

        public string SignInWithLinkIfPresentOnLaunchOrClipboard()
        {
            var ctx = Platform.CurrentActivity;
            var action = ctx.Intent?.Action;

            if (action == "ACTION_VIEW" && IsRowndSignInLink(Platform.CurrentActivity.Intent?.Data))
            {
                var intentData = ctx.Intent?.Data.ToString();
                if (intentData == null || !intentData.Contains("rownd.link"))
                {
                    return null;
                }

                return intentData;
            }
            else if (ctx.HasWindowFocus)
            {
                return SignInWithLinkFromClipboardIfPresent(ctx);
            }

            return null;
        }

        private string SignInWithLinkFromClipboardIfPresent(Activity ctx)
        {
            var clipboard = ctx.GetSystemService(Context.ClipboardService) as ClipboardManager;
            if (clipboard.PrimaryClipDescription?.HasMimeType(ClipDescription.MimetypeTextPlain) != true)
            {
                return null;
            }

            var clipboardText = clipboard.PrimaryClip?.GetItemAt(0)?.Text.ToString();
            if (string.IsNullOrEmpty(clipboardText))
            {
                return null;
            }

            if (!clipboardText.Contains("rownd.link"))
            {
                return null;
            }

            if (!clipboardText.StartsWith("http"))
            {
                clipboardText = "https://${clipboardText}";
            }

            var urlObj = Uri.Parse(clipboardText);
            if (urlObj.Fragment != null)
            {
                clipboardText = clipboardText.Replace($"#{urlObj.Fragment}", "");
            }

            clipboardText = clipboardText.Replace("http://", "https://");

            clipboard.ClearPrimaryClip();

            return clipboardText;
        }

        public bool IsRowndSignInLink(Uri uri)
        {
            return uri?.Host?.EndsWith("rownd.link") == true;
        }
    }
}
