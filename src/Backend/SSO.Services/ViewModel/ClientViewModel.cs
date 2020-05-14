using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel
{
    public class ClientViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string LogoUri { get; set; }
        public IEnumerable<string> AllowedCorsOrigins { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }

        public bool Enabled { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }

    }
}
