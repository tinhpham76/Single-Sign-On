﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.Client
{
    public class ClientPostLogoutRedirectUriViewModel
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}
