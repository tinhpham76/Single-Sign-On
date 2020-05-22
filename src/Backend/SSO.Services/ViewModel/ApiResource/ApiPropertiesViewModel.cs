using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.ApiResource
{
    public class ApiPropertiesViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ApiResourceId { get; set; }
    }
}
