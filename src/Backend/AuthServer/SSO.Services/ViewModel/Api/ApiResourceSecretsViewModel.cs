﻿using System;

namespace SSO.Services.ViewModel.Api
{
    public class ApiResourceSecretsViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public DateTime? Expiration { get; set; }
        public string Description { get; set; }
    }
}
