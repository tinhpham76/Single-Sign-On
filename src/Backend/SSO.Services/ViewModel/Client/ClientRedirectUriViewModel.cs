﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.Client
{
    public class ClientRedirectUriViewModel
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }
        public int ClientId { get; set; }
    }
}