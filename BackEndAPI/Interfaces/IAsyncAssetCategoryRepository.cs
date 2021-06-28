
using BackEndAPI.Entities;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncAssetCategoryRepository : IAsyncRepository<AssetCategory>
    {

        bool DidCategoryNameExist(string categoryName);

        bool DidCategoryCodeExist(string categoryCode);

    }
}