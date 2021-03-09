namespace Dpl.B2b.BusinessLogic.Graph
{
    public class AuthenticationConfig
    {
        public const string AuthorityUrlTemplate  = "https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize";
        public const string TokenUrlTemplate = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";
        public const string Resource = "https://graph.microsoft.com/.default";
        
        //public const string AdminconsentTemplate = "https://login.microsoftonline.com/{tenantId}/adminconsent";
        
        public string Instance { get; set; }
        public string TenantId { get; set;} 
        public string ClientId { get; set;}
        
        public string ClientSecret { get; set; }
       
        public string Domain { get; set; }
        
        public string AuthorityUrl  { get; set; }
        public string[] Scopes { get; set; } = new[] {Resource};
    }
}