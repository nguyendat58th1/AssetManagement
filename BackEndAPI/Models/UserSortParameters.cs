using BackEndAPI.Enums.Sort;

namespace BackEndAPI.Models
{
    public class UserSortParameters
    {
        public UserSortColumn? SortCol { get; set; } = UserSortColumn.STAFF_CODE;
        public SortOrder? Order { get; set; } = SortOrder.ASCEND;
    }
}