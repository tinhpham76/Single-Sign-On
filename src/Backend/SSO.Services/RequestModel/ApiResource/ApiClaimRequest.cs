using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.ApiResource
{
    public class ApiClaimRequest
    {
        public string Type { get; set; }
        public int ApiResourceId { get; set; }
    }
}
