using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Service.RequestModel.IdentityResource
{
    public class IdentityResourceQuickRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }        
    }
}
