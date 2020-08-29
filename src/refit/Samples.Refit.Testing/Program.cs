using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Samples.Refit.Http.Client
{
    class Program
    {
        static string BaseUrl = "https://jsonplaceholder.typicode.com";

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services
                .AddTransient<MyService>()
                .AddRefitClient<IMyApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl));

            var o = services
                .BuildServiceProvider()
                .GetRequiredService<MyService>();

            var item = await o.GetTodo(1);

            Console.WriteLine($"returned => {item.Id} / {item.Title} / {item.UserId} ");
        }
    }
}
