﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.Api
{
    public class ApiSecretRequest
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public string HasType { get; set; }
        public DateTime? Expiration { get; set; }
        public string Description { get; set; }
    }
}
