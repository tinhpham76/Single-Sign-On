using System;
using System.Collections.Generic;
using System.Text;

namespace SSO.Services.ViewModel.User
{
    public class UserRoleViewModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }
    }
}
