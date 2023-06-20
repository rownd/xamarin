using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Rownd.Core;
using Rownd.Models.Domain;
using Xamarin.Forms;

namespace Rownd.Models.Repos
{
    public class AppConfigRepo
    {
        private readonly StateRepo stateRepo = StateRepo.Get();

        private class AppConfigResponse
        {
            public AppConfigState App;
        }

        public static AppConfigRepo Get()
        {
            return Shared.ServiceProvider.GetService<AppConfigRepo>();
        }

        public async Task<AppConfigState> LoadAppConfigAsync()
        {
            var apiClient = ApiClient.Get();
            try
            {
                var response = await apiClient.Client.GetJsonAsync<AppConfigResponse>("hub/app-config");
                Device.BeginInvokeOnMainThread(() =>
                    stateRepo.Store.Dispatch(new StateActions.SetAppConfig()
                    {
                        AppConfig = response.App
                    })
                );
                return response.App;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch app config: {ex}");
                return null;
            }
        }
    }
}
