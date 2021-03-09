using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace Dpl.B2b.BusinessLogic.Graph
{
    public interface IGraphFactory
    {
        ClientCredentialProvider CreateAuthProvider();
        OnBehalfOfProvider CreateOnBehalfOfProvider(string[] scopes=null);

        AuthenticationConfig Config { get; }
    }
}