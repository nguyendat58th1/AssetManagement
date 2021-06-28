using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAssetCategoryService
    {
        IQueryable<AssetCategory> GetAll();

        Task<AssetCategory> Create(CreateCategoryModel model);

        bool DidCategoryNameExist(string categoryName);
        
        bool DidCategoryCodeExist(string categoryCode);
    }
}