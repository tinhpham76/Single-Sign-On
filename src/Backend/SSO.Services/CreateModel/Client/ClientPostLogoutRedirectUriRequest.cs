using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientPostLogoutRedirectUriRequest
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientId { get; set; }
    }
}
