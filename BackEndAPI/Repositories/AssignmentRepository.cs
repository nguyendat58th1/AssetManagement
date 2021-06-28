using System.Linq;
using BackEndAPI.Entities;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using BackEndAPI.Enums;
using System;
using BackEndAPI.Helpers;

namespace BackEndAPI.Repositories
{
    public class AssignmentRepository : AsyncGenericRepository<Assignment>, IAsyncAssignmentRepository
    {
        public AssignmentRepository(AssetsManagementDBContext context) : base(context)
        {

        }

        public int GetCountAsset(int id)
        {

            if (id <= 0)
            {

                throw new InvalidOperationException(Message.InvalidId);
                
            }

            return _context.Assignments.Count(x => x.AssetId == id);

        }

        public int GetCountUser(int id)
        {
            return _context.Assignments.Count(x => x.AssignedToUserId == id
            && x.State != AssignmentState.Declined);
        }
    }
}