using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAssetService
    {

        Task<GetAssetsListPagedResponseDTO> GetAssets(
            PaginationParameters paginationParameters,
            int adminId
        );

        Task<Asset> GetById(int id);

        Task<GetAssetsListPagedResponseDTO> GetAssetsByCategory(
            PaginationParameters paginationParameters,
            int adminId,
            int categoryId
        );

        Task<GetAssetsListPagedResponseDTO> GetAssetsByState(
            PaginationParameters paginationParameters,
            int adminId,
            AssetState state
        );
        Task<GetAssetsListPagedResponseDTO> SearchAssets(
            PaginationParameters paginationParameters,
            int adminId,
            string searchText
        );

        Task<Asset> Create(CreateAssetModel model);

        Task<IQueryable<Asset>> GetAllAssets(int userId);

        Task<IQueryable<Asset>> GetAssetsBySearching(int userId, string searchText);

        Task Update(int id, EditAssetModel model);

        Task Delete(int id);

    }
}