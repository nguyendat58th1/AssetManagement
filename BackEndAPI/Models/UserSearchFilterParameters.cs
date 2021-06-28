using BackEndAPI.Enums;

namespace BackEndAPI.Models
{
    public class UserSearchFilterParameters : GenericSearchFilterParameters
    {
        public UserType? Type { get; set; }
    }
}