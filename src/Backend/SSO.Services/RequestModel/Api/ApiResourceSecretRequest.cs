using System;

namespace SSO.Services.RequestModel.Api
{
    public class ApiResourceSecretRequest
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public string HashType { get; set; }

        public string Expiration { get; set; }
        public string Description { get; set; }
    }
}
