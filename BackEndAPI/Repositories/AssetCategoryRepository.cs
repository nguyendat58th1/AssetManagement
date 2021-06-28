using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using System.Collections.Generic;
using System;
using BackEndAPI.Helpers;
using System.Text.RegularExpressions;

namespace BackEndAPI.Repositories
{
    public class AssetCategoryRepository : AsyncGenericRepository<AssetCategory>, IAsyncAssetCategoryRepository
    {
        public AssetCategoryRepository(AssetsManagementDBContext context) : base(context)
        { }

        public bool DidCategoryNameExist(string categoryName)
        {

            if (categoryName == null)
            {

                throw new ArgumentNullException(Message.NullOrEmptyCategoryName);

            }

            categoryName = Regex.Replace(categoryName, @"\s+", " ").Trim();

            if (categoryName == "")
            {

                throw new ArgumentNullException(Message.NullOrEmptyCategoryName);

            }

            var result = _context.Set<AssetCategory>().FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower());

            if (result == null)
            {

                return false;

            }

            return true;

        }

        public bool DidCategoryCodeExist(string categoryCode)
        {

            if (categoryCode == null)
            {

                throw new ArgumentNullException(Message.NullOrEmptyCategoryCode);

            }

            categoryCode = Regex.Replace(categoryCode, @"\s+", " ").Trim();

            if (categoryCode == "")
            {

                throw new ArgumentNullException(Message.NullOrEmptyCategoryCode);

            }

            var result = _context.Set<AssetCategory>().FirstOrDefault(c => c.CategoryCode.ToLower() == categoryCode.ToLower());

            if (result == null)
            {

                return false;

            }

            return true;

        }

    }
}