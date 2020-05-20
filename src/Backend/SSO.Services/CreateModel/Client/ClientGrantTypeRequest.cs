using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientGrantTypeRequest
    {
        public string GrantType { get; set; }
        public string ClientId { get; set; }
    }
}
