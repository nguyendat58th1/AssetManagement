using System;
using System.Collections.Generic;
using BackEndAPI.Enums;
using Microsoft.AspNetCore.Identity;

namespace BackEndAPI.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public UserType Type { get; set; }

        public string StaffCode { get; set; }

        public string Password { get; set; }

        public Location Location { get; set; }

        public UserStatus Status { get; set; }
        public OnFirstLogin OnFirstLogin {get; set;}

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<ReturnRequest> Requests { get; set; }
        
    }
}