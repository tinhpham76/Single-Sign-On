using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.Client
{
    public class ClientClaimViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int ClientId { get; set; }
    }
}
