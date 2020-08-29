namespace Samples.HttpClientCon
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Security;
    using System.Threading.Tasks;
    using Authentication;
    using Contract;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Refit;
    using AuthenticatedParameterizedHttpClientHandler = Authentication.AuthenticatedParameterizedHttpClientHandler;

    class Program
    {
        // https://github.com/reactiveui/refit?WT.mc_id=-blog-scottha#using-httpclientfactory
        // https://www.hanselman.com/blog/UsingASPNETCore21sHttpClientFactoryWithRefitsRESTLibrary.aspx
        // https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore
        
        static void Main(string[] args)
        {
            RunAsync()
                .GetAwaiter()
                .GetResult();
        }
        
        private static async Task RunAsync()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<Program>()
                .Build();
            
            services
                .AddLogging(builder =>
                {
                    builder.ClearProviders()
                        .AddConsole(options => options.IncludeScopes = true)
                        .AddDebug();
                })
                .AddSingleton<IConfiguration>(configuration)
                .AddTransient<AuthenticatedParameterizedHttpClientHandler>()
                .AddWebAppCallsProtectedWebApi(configuration, new string[] { })
                .AddInMemoryTokenCaches();
            
            var azureAdOptions = new AzureADOptions();
            const string azureAdOptionsSectionName = "AzureAd";
            
            configuration.Bind(azureAdOptionsSectionName, azureAdOptions);

            var redirectUri = configuration.GetValue<string>($"{azureAdOptionsSectionName}:RedirectUri");

            var clientCredentialsApp = ConfidentialClientApplicationBuilder
                .Create(azureAdOptions.ClientId)
                .WithClientSecret(azureAdOptions.ClientSecret)
                .WithAuthority($"{azureAdOptions.Instance}/{azureAdOptions.TenantId}/oauth2/token")
                .WithRedirectUri(redirectUri)
                .Build();

            services.AddScoped(arg => clientCredentialsApp);
            services.AddScoped(typeof(IClientCredentialsAuthClient), typeof(ClientCredentialsAuthClient));

            var resource = configuration.GetSection("ProtectedResources:Resource2").Get<MyResource>();

            services.AddScoped(typeof(MyResource), provider => resource);
            

            // try a remote endpoint with no authentication
            var baseUrl = "https://jsonplaceholder.typicode.com";
            services
                .AddRefitClient<IJsonPlaceHolderAPI>(
                    new RefitSettings
                    {
                        AuthorizationHeaderValueGetter = () => Task.FromResult("my derived token goes here")
                    })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

            var apiClient = services
                .BuildServiceProvider()
                .GetRequiredService<IJsonPlaceHolderAPI>();
            
            var response = await apiClient.GetTodo(1);
            Console.WriteLine($"remote api response for todo item 1 => {response.Title}");


            // try a local endpoint with authentication
            baseUrl = "https://localhost:5001";
            services
                .AddRefitClient<ILocalService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                // add additional IHttpClientBuilder chained methods as required here (order matters people)
                .AddHttpMessageHandler<AuthenticatedParameterizedHttpClientHandler>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(2));

            var localApi = services
                .BuildServiceProvider()
                .GetRequiredService<ILocalService>();
            var localApiResponse = await localApi.Get();
            Console.WriteLine($"local api response for todo item 1 => {localApiResponse.Count()}");
        }
    }
}
