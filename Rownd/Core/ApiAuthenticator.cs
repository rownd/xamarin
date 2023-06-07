using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Rownd.Models.Repos;

namespace Rownd.Core
{
	public class ApiAuthenticator : IAuthenticator
	{
        public ValueTask Authenticate(IRestClient client, RestRequest request)
        {
            var stateRepo = StateRepo.Get();

            if (!String.IsNullOrEmpty(stateRepo.Store.State.Auth.AccessToken))
            {
                request.AddHeader(KnownHeaders.Authorization, $"Bearer {stateRepo.Store.State.Auth.AccessToken}");
            }

            return new ValueTask();
        }
    }
}

