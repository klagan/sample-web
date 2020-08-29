namespace Samples.HttpClientCon.Contract
{
    using System.Security;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;

    public interface IClientCredentialsAuthClient
    {
        Task<AuthenticationResult> GetAuthContextAsync(IProtectedResource resource, SecureString password);
        
        IConfidentialClientApplication ConfidentialClientApplication { get; }
    }
}