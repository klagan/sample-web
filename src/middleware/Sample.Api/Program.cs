using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sample.Api
{
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(
            string[] args
        )
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(
            string[] args
        ) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

                    builder.AddConfiguration(configuration.GetSection("Logging"));
                    builder.AddConsole(options => options.IncludeScopes = true);
                    builder.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
