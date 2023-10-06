using System;
using Rownd.Xamarin.Models.Repos;

namespace Rownd.Xamarin.Utils
{
    public interface IAppleAuthCoordinator
    {
        public void Inject(RowndInstance rownd, AuthRepo authRepo);
        public void SignIn();
        public void SignIn(SignInIntent? intent);
    }
}
