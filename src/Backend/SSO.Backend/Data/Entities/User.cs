using Microsoft.AspNetCore.Identity;
using SSO.Backend.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SSO.Backend.Data.Entities
{
    public class User : IdentityUser, IDateTracking
    {
        public User()
        {
        }

        public User(string id, string userName, string firstName, string lastName,
            string email, string phoneNumber, DateTime dob)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Dob = dob;
        }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
