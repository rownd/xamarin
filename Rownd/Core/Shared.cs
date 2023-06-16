using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rownd.Models.Repos;
using Xamarin.Forms;

namespace Rownd.Core
{
    public static class Shared
    {
        public static Application App { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init(Application app, Config config = null)
        {
            App = app;
            //var a = Assembly.GetExecutingAssembly();
            //using var stream = a.GetManifestResourceStream("MyAssemblyName.appsettings.json");

            var host = new HostBuilder()
                        //.ConfigureHostConfiguration(c =>
                        //{
                        //    // Tell the host configuration where to file the file (this is required for Xamarin apps)
                        //    c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                        //    //read in the configuration file!
                        //    c.AddJsonStream(stream);
                        //})
                        .ConfigureServices((ctx, svcCollection) =>
                        {
                            // Configure our local services and access the host configuration
                            ConfigureServices(ctx, svcCollection, config);
                        })
                        .ConfigureLogging(l => l.AddConsole(o =>
                        {
                            //setup a console logger and disable colors since they don't have any colors in VS
                            o.DisableColors = true;
                        }))
                        .Build();

            //Save our service provider so we can use it later.
            ServiceProvider = host.Services;
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services, Config config)
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

            services.AddSingleton(new StateRepo());
            services.AddSingleton<ApiClient, ApiClient>();
            services.AddSingleton<AppConfigRepo, AppConfigRepo>();

            // add the ViewModel, but as a Transient, which means it will create a new one each time.
            //services.AddTransient<MyViewModel>();

            //Another thing we can do is access variables from that json file
            //var world = ctx.Configuration["Hello"];
        }
    }
}
