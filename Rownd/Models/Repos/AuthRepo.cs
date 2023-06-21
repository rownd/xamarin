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
        private readonly StateRepo stateRepo = StateRepo.Get();

        private Task<RestResponse<AuthState>> refreshTask;

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

            return authState.AccessToken;
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
                        AuthState = refreshTask.Result.Data
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

        public async Task<AuthState> RefreshToken()
        {
            if (refreshTask != null)
            {
                await refreshTask;
                return refreshTask.Result.Data;
            }

            var apiClient = ApiClient.Get();
            try
            {
                var request = new RestRequest("hub/auth/token")
                    .AddBody(new TokenRequestBody
                    {
                        AppId = stateRepo.Store.State.AppConfig.Id,
                        RefreshToken = stateRepo.Store.State.Auth.RefreshToken
                    });
                refreshTask = apiClient.Client.ExecutePostAsync<AuthState>(request);
                await refreshTask;

                Device.BeginInvokeOnMainThread(() =>
                    stateRepo.Store.Dispatch(new StateActions.SetAuthState()
                    {
                        AuthState = refreshTask.Result.Data
                    })
                );

                return refreshTask.Result.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh access token: {ex}");
                return null;
            }
            finally
            {
                refreshTask = null;
            }
        }
    }
}
