using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using Rownd.Xamarin.Core;
using Rownd.Xamarin.Models.Domain;
using Xamarin.Forms;

namespace Rownd.Xamarin.Models.Repos
{
    public class AuthRepo
    {
        public class RefreshTokenExpiredException : Exception
        {
            public RefreshTokenExpiredException() { }
            public RefreshTokenExpiredException(string message) : base(message) { }
        }

        private readonly StateRepo stateRepo = StateRepo.Get();

        private Task<AuthState> refreshTask;

        private class TokenRequestBody
        {
            [JsonProperty("app_id")]
            public string AppId { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("id_token")]
            public string IdToken { get; set; }
        }

        public static AuthRepo Get()
        {
            return Shared.ServiceProvider.GetService<AuthRepo>();
        }

        public AuthRepo()
        {
        }

        public async Task<string> GetAccessToken()
        {
            var authState = stateRepo.Store.State.Auth;

            if (!authState.IsAccessTokenValid)
            {
                authState = await RefreshToken();
            }

            return authState?.AccessToken;
        }

        public async Task<string> GetAccessToken(string token)
        {
            var apiClient = ApiClient.Get();
            try
            {
                var request = new RestRequest("hub/auth/token")
                    .AddBody(new TokenRequestBody
                    {
                        AppId = stateRepo.Store.State.AppConfig.Id,
                        IdToken = token
                    });
                var response = await apiClient.Client.ExecutePostAsync<AuthState>(request);

                Device.BeginInvokeOnMainThread(() =>
                    stateRepo.Store.Dispatch(new StateActions.SetAuthState()
                    {
                        AuthState = response.Data
                    })
                );

                return response.Data.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh access token: {ex}");
                return null;
            }
        }

        public async Task AuthenticateUsingSignInLink(string link)
        {
            var apiClient = ApiClient.Get();
            var request = new RestRequest(link);
            var response = await apiClient.Client.ExecuteGetAsync(request);
        }

        internal async Task<AuthState> RefreshToken()
        {
            if (refreshTask != null)
            {
                await refreshTask;
                return refreshTask.Result;
            }

            try
            {
                refreshTask = MakeRefreshTokenRequest();
                return await refreshTask.ContinueWith(task =>
                {
                    var result = refreshTask.Result;
                    refreshTask = null;

                    Device.BeginInvokeOnMainThread(() =>
                    stateRepo.Store.Dispatch(new StateActions.SetAuthState()
                    {
                        AuthState = result
                    })
                );

                    return result;
                });
            }
            catch (RefreshTokenExpiredException ex)
            {
                Console.WriteLine($"Failed to refresh token. It was likely expired. User will be signed out. Reason: {ex}");
                Device.BeginInvokeOnMainThread(() => stateRepo.Store.Dispatch(new StateActions.SetAuthState()));
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh token. This may be recoverable. Reason: {ex}");
                throw ex;
            }
        }

        private async Task<AuthState> MakeRefreshTokenRequest()
        {
            var request = new RestRequest("hub/auth/token")
                .AddBody(new TokenRequestBody
                {
                    AppId = stateRepo.Store.State.AppConfig.Id,
                    RefreshToken = stateRepo.Store.State.Auth.RefreshToken
                });
            var apiClient = ApiClient.Get();
            var result = await apiClient.Client.ExecutePostAsync<AuthState>(request);

            if (result.ResponseStatus == ResponseStatus.Completed)
            {
                return result.Data;
            }
            else if (result.ResponseStatus == ResponseStatus.Error && result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new RefreshTokenExpiredException(result.Content);
            }

            throw new Exception(result.Content);
        }
    }
}
