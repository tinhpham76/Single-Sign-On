using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel
{
    public class ClientUpdateRequest
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; } 
        public string Description { get; set; }
        public string RedirectUris { get; set; }
        public string PostLogoutRedirectUris { get; set; }
        public string LogoUri { get; set; }        


    }
}
