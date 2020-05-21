using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.Client
{
    public class ClientClaimRequest
    {
      
        public string Type { get; set; }
        public string Value { get; set; }
        public string ClientId { get; set; }
        
    }
}
