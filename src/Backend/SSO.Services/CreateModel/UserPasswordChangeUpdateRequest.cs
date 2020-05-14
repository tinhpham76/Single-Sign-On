using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.CreateModel
{
    public class UserPasswordChangeUpdateRequest
    {
        public string UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
