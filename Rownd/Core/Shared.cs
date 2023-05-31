using System;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;
using Microsoft.Extensions.Logging.Console;
using Xamarin.Forms;

namespace Rownd.Core
{
    public static class Shared
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init(Config config = null)
        {
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

            } else
            {
                services.AddSingleton<Config, Config>();
            }

            // add the ViewModel, but as a Transient, which means it will create a new one each time.
            //services.AddTransient<MyViewModel>();

            //Another thing we can do is access variables from that json file
            //var world = ctx.Configuration["Hello"];
        }
    }
}

