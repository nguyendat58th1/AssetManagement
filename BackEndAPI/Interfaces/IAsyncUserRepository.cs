using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncUserRepository : IAsyncRepository<User>
    {

        int CountUsername(string username);
        
        int CountAdminRemain();

    }
}