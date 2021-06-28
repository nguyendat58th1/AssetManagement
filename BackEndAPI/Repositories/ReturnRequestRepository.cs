using System;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;

namespace BackEndAPI.Repositories
{
    public class ReturnRequestRepository : AsyncGenericRepository<ReturnRequest>, IAsyncReturnRequestRepository
    {
        public ReturnRequestRepository(AssetsManagementDBContext context) : base(context)
        { }
    }
}