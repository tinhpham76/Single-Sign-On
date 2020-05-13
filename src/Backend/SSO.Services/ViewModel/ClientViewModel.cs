using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel
{
    public class ClientViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ProtocolType { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public IEnumerable<string> AllowedCorsOrigins { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }
    }
}
