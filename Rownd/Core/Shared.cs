using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GuerrillaNtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rownd.Xamarin.Models.Repos;
using Xamarin.Forms;

namespace Rownd.Xamarin.Core
{
    public static class Shared
    {
        public static Application App { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init(Application app, Config config = null)
        {
            App = app;

            var root = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "root");
            Directory.CreateDirectory(root);

            var host = new HostBuilder()
                .ConfigureHostConfiguration(c =>
                {
                    c.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { HostDefaults.ContentRootKey, root }
                    });
                })
                .ConfigureServices((ctx, svcCollection) =>
                {
                    // Configure our local services and access the host configuration
                    ConfigureServices(ctx, svcCollection, config);
                })
                .Build();

            // Save our service provider so we can use it later.
            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services, Config config)
        {
            // add as a singleton so only one ever will be created.
            if (config != null)
            {
                services.AddSingleton(config);
            }
            else
            {
                services.AddSingleton(new Config());
            }

            NtpClient ntpClient = new NtpClient("time.cloudflare.com");
            services.AddSingleton(ntpClient);

            services.AddSingleton(new StateRepo());
            services.AddSingleton<ApiClient, ApiClient>();
            services.AddSingleton<AppConfigRepo, AppConfigRepo>();
            services.AddSingleton<AuthRepo>();
            services.AddSingleton<UserRepo>();

            Task.Run(async () =>
                {
                    NtpClock clock = await ntpClient.QueryAsync();
                }
            );
        }
    }
}
