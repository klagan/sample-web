namespace Samples.HttpClientCon.Authentication
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Identity.Client;

    public class EmptyAuthenticationResult : AuthenticationResult
    {
        public EmptyAuthenticationResult()
            : this(string.Empty,
                false,
                string.Empty,
                DateTimeOffset.Now,
                DateTimeOffset.Now,
                string.Empty,
                null,
                string.Empty,
                new string[] { },
                Guid.Empty
            )
        { }

        private EmptyAuthenticationResult(
            string accessToken, 
            bool isExtendedLifeTimeToken, 
            string uniqueId, 
            DateTimeOffset expiresOn, 
            DateTimeOffset extendedExpiresOn, 
            string tenantId, 
            IAccount account, 
            string idToken, 
            IEnumerable<string> scopes, 
            Guid correlationId, 
            string tokenType = "Bearer") 
            : base(accessToken, isExtendedLifeTimeToken, uniqueId, expiresOn, extendedExpiresOn, tenantId, account, idToken, scopes, correlationId, tokenType)
        { }
    }
}