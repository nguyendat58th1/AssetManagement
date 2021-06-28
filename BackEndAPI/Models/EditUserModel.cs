using System;
using System.ComponentModel.DataAnnotations;
using BackEndAPI.Entities;
using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class EditUserModel
    {
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