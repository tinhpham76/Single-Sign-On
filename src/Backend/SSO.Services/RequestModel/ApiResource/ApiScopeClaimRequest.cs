using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.ApiResource
{
    public class ApiScopeClaimRequest
    {
        public string Type { get; set; }
        public int ApiScopeId { get; set; }
    }
}
