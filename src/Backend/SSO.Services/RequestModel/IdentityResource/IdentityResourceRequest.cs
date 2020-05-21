using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.IdentityResource
{
    public class IdentityResourceRequest
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
    }
}
