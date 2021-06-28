using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IAssignmentService
    {
        IQueryable<Assignment> GetAll();
        Task Create(int userId, AssignmentModel model);
        Task Update(int id, AssignmentModel model);

        Task Delete(int id);
        Task<GetAssignmentListPagedResponse> GetAllAssignments(
         PaginationParameters paginationParameters,
         int adminId
        );

        Task<IQueryable<Assignment>> GetAllForUser(int userId);
        Task<Assignment> GetById(int id);

        Task<GetAssignmentListPagedResponse> GetAssignmentByState(
            PaginationParameters paginationParameters,
            int adminId,
            AssignmentState state
    );

        Task<GetAssignmentListPagedResponse> GetAssignmentByDate(
         PaginationParameters paginationParameters,
         int adminId,
         int year,
         int month,
         int day
 );

        Task<GetAssignmentListPagedResponse> SearchAssignments(
            PaginationParameters paginationParameters,
            int adminId,
            string searchText
        );

        Task AcceptAssignment(int id);

        Task DeclineAssignment(int id);
        
        Task UndoResponeAssignment(int id);
         
    }
}