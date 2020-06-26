using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.Client
{
    public class ClientSecretRequest
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public string HashType { get; set; }

        public string Expiration { get; set; }
        public string Description { get; set; }
    }
}
