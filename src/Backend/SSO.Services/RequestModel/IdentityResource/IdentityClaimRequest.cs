using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.IdentityResource
{
    public class IdentityClaimRequest
    {
        public string Type { get; set; }
        public int IdentityResourceId { get; set; }
    }
}
