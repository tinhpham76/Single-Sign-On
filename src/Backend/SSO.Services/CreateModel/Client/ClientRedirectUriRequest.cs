using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientRedirectUriRequest
    {
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
    }
}
