using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.ApiResource
{
    public class ApiClaimsViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }
    }
}
