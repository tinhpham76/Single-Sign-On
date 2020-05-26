﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.Api
{
    public class ApiResourceViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }  
        public List<string> UserClaims { get; set; }
    }
}
