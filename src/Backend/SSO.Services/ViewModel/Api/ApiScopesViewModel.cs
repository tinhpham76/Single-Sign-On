using System.Collections.Generic;

namespace SSO.Services.ViewModel.Api
{
    public class ApiScopesViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool Emphasize { get; set; }
    }
}
