using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel
{
    public class ClientUpdateRequest
    {
        //update table client
        public bool Enabled { get; set; }
        public string ClientId { get; set; }      
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int ConsentLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }           
        public int DeviceCodeLifetime { get; set; }        
      
    }
}