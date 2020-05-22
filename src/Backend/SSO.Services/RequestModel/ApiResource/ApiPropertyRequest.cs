using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.RequestModel.ApiResource
{
    public class ApiPropertyRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int ApiResourceId { get; set; }
    }
}
