using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using System.Collections.Generic;
using System;
using BackEndAPI.Helpers;

namespace BackEndAPI.Repositories
{
    public class AssetRepository : AsyncGenericRepository<Asset>, IAsyncAssetRepository
    {
        public AssetRepository(AssetsManagementDBContext context) : base(context)
        { }

        public int CountingAssetNumber(int cateId)
        {

            if (cateId <= 0)
            {

                throw new InvalidOperationException(Message.InvalidId);

            }

            return _context.Set<Asset>().Where(c => c.CategoryId == cateId).Count();
        }
    }
}