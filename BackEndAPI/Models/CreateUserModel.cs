using System;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class CreateUserModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public UserType Type { get; set; }

        public Location Location { get; set; }

    }
}