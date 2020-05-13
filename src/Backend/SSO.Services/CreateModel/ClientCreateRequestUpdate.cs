using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel
{
    public class ClientCreateRequestUpdate
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; } 
        public string Description { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }


    }
}
