namespace Samples.HttpClientCon.Authentication
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Contract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;

    internal class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
    {
        private readonly IClientCredentialsAuthClient _authenticationClient;
        private readonly ILogger<ClientCredentialsAuthClient> _logger;
        private MyResource _myResource;

        public AuthenticatedParameterizedHttpClientHandler(
            ILogger<ClientCredentialsAuthClient> logger,
            IClientCredentialsAuthClient authenticationClient,
            MyResource resource
        )
        {
            _logger = logger;
            _authenticationClient = authenticationClient;
            _myResource = resource;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthenticationResult result;

            var authHeader = request.Headers.Authorization;

            if (authHeader == null)
            {
                _logger.LogWarning($"Authentication header is null");
                return await base.SendAsync(request, cancellationToken);
            }

            if (_myResource is null)
            {
                _logger.LogWarning($"Protected resource is null");

                _myResource = MyResource.Empty();
                result = new EmptyAuthenticationResult();
            }

            var accounts = await _authenticationClient.ConfidentialClientApplication.GetAccountsAsync();
            
            try
            {
                result = await _authenticationClient
                    .ConfidentialClientApplication
                    .AcquireTokenSilent(_myResource.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException e)
            {
                _logger.LogWarning($"Failed token acquisition for {e.Message}");

                result = _authenticationClient
                    .ConfidentialClientApplication
                    .AcquireTokenForClient(_myResource.Scopes)
                    .WithAuthority(_authenticationClient.ConfidentialClientApplication.Authority, true)
                    .ExecuteAsync(cancellationToken)
                    .Result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed token acquisition for {e.Message}");

                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(authHeader.Scheme, result.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}