using System;
using System.ComponentModel.DataAnnotations;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class UserInfo
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public UserType Type { get; set; }
    }
}