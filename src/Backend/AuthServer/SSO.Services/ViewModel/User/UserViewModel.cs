using System;

namespace SSO.Services.ViewModel.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public String Dob { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
