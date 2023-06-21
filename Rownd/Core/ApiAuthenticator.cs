using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Rownd.Xamarin.Models.Repos;

namespace Rownd.Xamarin.Core
{
    public class ApiAuthenticator : IAuthenticator
    {
        public ValueTask Authenticate(IRestClient client, RestRequest request)
        {
            var stateRepo = StateRepo.Get();

            if (!string.IsNullOrEmpty(stateRepo.Store.State.Auth.AccessToken))
            {
                request.AddHeader(KnownHeaders.Authorization, $"Bearer {stateRepo.Store.State.Auth.AccessToken}");
            }

            return default;
        }
    }
}
