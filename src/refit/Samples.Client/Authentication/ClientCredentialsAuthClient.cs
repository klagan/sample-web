namespace Samples.HttpClientCon.Authentication
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Contract;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;

    public class ClientCredentialsAuthClient : IClientCredentialsAuthClient
    {
        private readonly ILogger<ClientCredentialsAuthClient> _logger;

        public ClientCredentialsAuthClient(ILogger<ClientCredentialsAuthClient> logger, IConfidentialClientApplication confidentialClientApplication)
        {
            _logger = logger;
            ConfidentialClientApplication = confidentialClientApplication;
        }

        /// <summary>
        /// Gets the authentication context using the client credentials OAuth flow.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> GetAuthContextAsync(IProtectedResource resource, SecureString password)
        {
            if (resource is null)
            {
                _logger.LogWarning($"Protected resource is null");

                return new EmptyAuthenticationResult();
            }

            var accounts = await ConfidentialClientApplication.GetAccountsAsync();

            AuthenticationResult result;

            try
            {
                result = await ConfidentialClientApplication.AcquireTokenSilent(resource.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException e)
            {
                _logger.LogWarning($"Failed token acquisition for {e.Message}");

                result = ConfidentialClientApplication
                    .AcquireTokenForClient(resource.Scopes)
                    .WithAuthority(ConfidentialClientApplication.Authority, true)
                    .ExecuteAsync()
                    .Result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed token acquisition for {e.Message}");

                return new EmptyAuthenticationResult();
            }

            return result;
        }

        public IConfidentialClientApplication ConfidentialClientApplication { get; }
    }
}