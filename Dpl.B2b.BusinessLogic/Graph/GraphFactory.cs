using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace Dpl.B2b.BusinessLogic.Graph
{
    public class GraphFactory : BaseFactory, IGraphFactory
    {
        public GraphFactory(IConfiguration configuration) : base(configuration)
        {
            Config = _configuration.GetSection("AzureAd").Get<AuthenticationConfig>();
            Config.ClientId = Config.ClientId.Replace("api://", string.Empty);
            Config.AuthorityUrl = string.Format(AuthenticationConfig.AuthorityUrlTemplate, Config.TenantId);
            Config.Scopes ??= new[] {"https://graph.microsoft.com/.default"};
        }

        public AuthenticationConfig Config { get; }

        public ClientCredentialProvider CreateAuthProvider()
        {
            //  Authentifizierungsanbieters basierend auf dem Szenario -> WebAPI, die webapis aufruft
            // https://docs.microsoft.com/de-de/graph/sdks/choose-authentication-providers?tabs=CS
            
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(Config.ClientId)
                .WithTenantId(Config.TenantId)
                .WithClientSecret(Config.ClientSecret)
                .Build();
            
            return new ClientCredentialProvider(confidentialClientApplication);
        }
        
        public OnBehalfOfProvider CreateOnBehalfOfProvider(string[] scopes=null)
        {
            scopes ??= Config.Scopes;
            
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(Config.ClientId)
                .WithAuthority(Config.AuthorityUrl)
                .WithClientSecret(Config.ClientSecret)
                .Build();

            var authProvider = new OnBehalfOfProvider(confidentialClientApplication, scopes);
            
            return authProvider;
        }
        
        /// <summary>
        /// For explanation only
        /// Gibt einen Authentifikation Anbieter zurück der eigens codierte funktion verwendet um ein token abzurufen.
        /// Es wird ein Token für einen Client, kein delegiertes Token abgerufen!
        /// </summary>
        /// <returns></returns>
        private DelegateAuthenticationProvider CreateDelegateAuthenticationProvider()
        {
            var delegateAuthenticationProvider = new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                var app = ConfidentialClientApplicationBuilder
                    .Create(Config.ClientId)
                    .WithTenantId(Config.TenantId)
                    .WithClientSecret(Config.ClientSecret)
                    .Build();
                
                var scopes = new[] {"https://graph.microsoft.com/.default"};
                
                // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                var authResult = await app
                    .AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                var header = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

                // Add the access token in the Authorization header of the API request.
                requestMessage.Headers.Authorization = header;
            });
            return delegateAuthenticationProvider;
        }
        
        /// <summary>
        /// For explanation only
        /// </summary>
        /// <returns></returns>
        private UsernamePasswordProvider GetUsernamePasswordClient()
        {
            var publicClientApplication = PublicClientApplicationBuilder
                .Create(Config.ClientId)
                .WithTenantId(Config.TenantId)
                .Build();

            return new UsernamePasswordProvider(publicClientApplication, Config.Scopes);

            
        }
    }
}