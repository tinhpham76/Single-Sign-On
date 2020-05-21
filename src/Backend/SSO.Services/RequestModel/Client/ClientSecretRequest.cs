using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.Client
{
    public class ClientSecretRequest
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public string ClientId { get; set; }
    }
}
