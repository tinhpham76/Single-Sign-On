﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel.Client
{
    public class ClientIdPRestrictionRequest
    {
        public string Provider { get; set; }
        public string ClientId { get; set; }
    }
}
