
using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncAssignmentRepository : IAsyncRepository<Assignment>
    {

        int GetCountAsset(int id);

        int GetCountUser(int id);
        
    }
}