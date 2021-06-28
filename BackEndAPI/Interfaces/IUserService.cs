using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IUserService
    {
        Task<GetUsersListPagedResponseDTO> GetUsers(
            PaginationParameters paginationParameters,
            int adminId
        );

        Task<GetUsersListPagedResponseDTO> GetUsers(
            int adminId,
            UserSearchFilterParameters searchFilterParameters,
            UserSortParameters sortParameters,
            PaginationParameters paginationParameters
        );

        AuthenticateResponse Authenticate(AuthenticateRequest model);

        IEnumerable<User> GetAll();
        Task<IQueryable<UserDTO>> GetAllUsers(int userId);

        Task<IQueryable<UserDTO>> GetUserBySearching(int userId,string searchText);

        Task<User> Create(CreateUserModel user);

        Task Update(int id, EditUserModel model);
        Task Disable(int userId, int id);
        Task<UserInfo> GetById(int id);
        Task ChangePassword(int id, ChangePasswordRequest model);
        Task<User> GetUserByIdWithPassword(int id);
    }
}