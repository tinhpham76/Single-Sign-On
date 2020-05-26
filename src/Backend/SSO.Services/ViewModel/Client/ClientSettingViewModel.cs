using System.Collections.Generic;

namespace SSO.Services.ViewModel.Client
{
    public class ClientSettingViewModel
    {
        public bool Enabled { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ProtocolType { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }

    }
}
