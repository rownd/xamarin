using System;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using HttpTracer;

namespace Rownd.Core
{
	public class ApiClient
	{
		public RestClient Client;

		public static ApiClient Get()
		{
			return Shared.ServiceProvider.GetService<ApiClient>();
        }

		public ApiClient()
		{
			var config = Config.GetConfig();
			var options = new RestClientOptions(config.ApiUrl)
			{
				Authenticator = new ApiAuthenticator(),
				ConfigureMessageHandler = handler => new HttpTracerHandler(handler)
			};

            JsonSerializerSettings defaultSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };


            Client = new RestClient(
				options,
                configureSerialization: s => s.UseNewtonsoftJson(defaultSettings)
            );

			Client.AddDefaultHeader("x-rownd-app-key", config.AppKey);
		}
	}
}
