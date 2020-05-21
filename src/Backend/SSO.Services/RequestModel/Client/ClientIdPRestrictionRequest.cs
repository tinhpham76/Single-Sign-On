using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.Client
{
    public class ClientIdPRestrictionRequest
    {
        public string Provider { get; set; }
        public string ClientId { get; set; }
    }
}
