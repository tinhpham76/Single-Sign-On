using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.Api
{
    public class ApiSecretsViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }        
        public string Type { get; set; }
        public DateTime? Expiration { get; set; }
        public string Description { get; set; }
    }
}
