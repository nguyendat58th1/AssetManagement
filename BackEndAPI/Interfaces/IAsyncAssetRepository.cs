using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncAssetRepository : IAsyncRepository<Asset>
    {

        int CountingAssetNumber(int cateId);

    }
}