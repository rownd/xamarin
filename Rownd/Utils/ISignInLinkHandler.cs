using System;
using System.Threading.Tasks;

namespace Rownd.Xamarin.Utils
{
    public interface ISignInLinkHandler
    {
        public Task<string> HandleSignInLink();
    }
}
