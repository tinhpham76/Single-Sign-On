using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.ApiResource
{
    public class ApiSecretRequest
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public int ApiResourceId { get; set; }
    }
}
