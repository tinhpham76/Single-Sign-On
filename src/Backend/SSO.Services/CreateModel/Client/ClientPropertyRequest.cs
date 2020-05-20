using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientPropertyRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ClientId { get; set; }
    }
}
