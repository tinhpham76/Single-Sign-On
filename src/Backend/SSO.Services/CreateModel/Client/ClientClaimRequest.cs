using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientClaimRequest
    {
      
        public string Type { get; set; }
        public string Value { get; set; }
        public string ClientId { get; set; }
        
    }
}
