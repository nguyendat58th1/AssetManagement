using System.Threading.Tasks;
using BackEndAPI.Models;

namespace BackEndAPI.Interfaces
{
    public interface IReturnRequestService
    {
        Task<GetReturnRequestsPagedResponseDTO> GetAll(
           PaginationParameters paginationParameters,
           int adminId
       );

        Task<GetReturnRequestsPagedResponseDTO> Filter(
            PaginationParameters paginationParameters,
            int adminId,
            ReturnRequestFilterParameters filterParameters
        );

        Task<GetReturnRequestsPagedResponseDTO> Search(
            PaginationParameters paginationParameters,
            int adminId,
            string searchQuery
        );

        Task<ReturnRequestDTO> Create(CreateReturnRequestModel model, int requestedById);

        int GetAssociatedActiveCount(string assetCode);

        Task Approve(int rrId, int adminId);

        Task Deny(int rrId, int adminId);
    }
}