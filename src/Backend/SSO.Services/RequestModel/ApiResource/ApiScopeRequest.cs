using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.ApiResource
{
    public class ApiScopeRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public int ApiResourceId { get; set; }
    }
}
