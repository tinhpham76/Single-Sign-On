using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USERAPI.Backend.Models
{
    public class UserPasswordChangeRequest
    {    

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string CheckPassword { get; set; }
    }
}
