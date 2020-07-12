using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USERAPI.Backend.Models
{
    public class UserUpdateRequest
    { 
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Dob { get; set; }
    }
}
