using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;

namespace BackEndAPI.Services
{
    public class ReturnRequestService : IReturnRequestService
    {

        private readonly IAsyncUserRepository _userRepository;
        private readonly IAsyncAssignmentRepository _assignmentRepository;
        private readonly IAsyncAssetRepository _assetRepository;
        private readonly IAsyncReturnRequestRepository _returnRequestRepository;
        private readonly IMapper _mapper;


        public ReturnRequestService(
            IAsyncUserRepository userRepository,
            IAsyncAssetRepository assetRepository,
            IAsyncReturnRequestRepository returnRequestRepository,
            IAsyncAssignmentRepository assignmentRepository,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _assetRepository = assetRepository;
            _returnRequestRepository = returnRequestRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }


        public async Task<ReturnRequestDTO> Create(
            CreateReturnRequestModel model,
            int requestedById
        )
        {
            if (model is null)
            {
                throw new Exception(Message.NullInputModel);
            }

            var user = _userRepository.GetAll()
                .Where(u => u.Id == requestedById)
                .SingleOrDefault();
            if (user is null)
            {
                throw new Exception(Message.UserNotFound);
            }
            if (user.Status == UserStatus.Disabled)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var assignment = await _assignmentRepository.GetById(model.AssignmentId);
            if (assignment is null)
            {
                throw new Exception(Message.AssignmentNotFound);
            }
            if (user.Type != UserType.Admin && assignment.AssignedToUserId != requestedById)
            {
                throw new Exception(Message.TriedToCreateReturnRequestForSomeoneElseAssignment);
            }
            if (assignment.State != AssignmentState.Accepted)
            {
                throw new Exception(Message.AssignedAssetNotAccepted);
            }

            var newReturnRequest = new ReturnRequest
            {
                Assignment = assignment,
                AssetCodeCopy = assignment.Asset.AssetCode,
                AssetNameCopy = assignment.Asset.AssetName,
                AssignedDateCopy = assignment.AssignedDate.Date,
                RequestedByUser = user,
                State = RequestState.WaitingForReturning
            };
            var resultReturnRequest = await _returnRequestRepository.Create(newReturnRequest);
            return _mapper.Map<ReturnRequestDTO>(resultReturnRequest);
        }

        public async Task<GetReturnRequestsPagedResponseDTO> Filter(
            PaginationParameters paginationParameters,
            int adminId,
            ReturnRequestFilterParameters filterParameters
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var filteredReturnRequests = _returnRequestRepository.GetAll();

            if (filterParameters.ReturnedDate is not null)
            {
                filteredReturnRequests = filteredReturnRequests.Where(rr =>
                                                                    rr.ReturnedDate != null
                                                                && rr.ReturnedDate.Value.Date == filterParameters.ReturnedDate.Value.Date);
            }
            if (filterParameters.RequestState is not null)
            {
                filteredReturnRequests = filteredReturnRequests.Where(rr => rr.State == filterParameters.RequestState);
            }

            var pagedFilteredReturnRequests = PagedList<ReturnRequest>.ToPagedList(
                filteredReturnRequests,
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetReturnRequestsPagedResponseDTO
            {
                CurrentPage = pagedFilteredReturnRequests.CurrentPage,
                PageSize = pagedFilteredReturnRequests.PageSize,
                TotalCount = pagedFilteredReturnRequests.TotalCount,
                TotalPages = pagedFilteredReturnRequests.TotalPages,
                HasNext = pagedFilteredReturnRequests.HasNext,
                HasPrevious = pagedFilteredReturnRequests.HasPrevious,
                Items = pagedFilteredReturnRequests.Select(rr => _mapper.Map<ReturnRequestDTO>(rr))
            };
        }

        public async Task<GetReturnRequestsPagedResponseDTO> GetAll(
            PaginationParameters paginationParameters,
            int adminId
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var returnRequests = PagedList<ReturnRequest>.ToPagedList(
                _returnRequestRepository.GetAll().OrderBy(c=>c.State)
                                                .ThenBy(c=>c.RequestedByUser.UserName)
                                                .ThenBy(c=>c.AssetCodeCopy),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetReturnRequestsPagedResponseDTO
            {
                CurrentPage = returnRequests.CurrentPage,
                PageSize = returnRequests.PageSize,
                TotalCount = returnRequests.TotalCount,
                TotalPages = returnRequests.TotalPages,
                HasNext = returnRequests.HasNext,
                HasPrevious = returnRequests.HasPrevious,
                Items = returnRequests.Select(rr => _mapper.Map<ReturnRequestDTO>(rr))
            };
        }

        public int GetAssociatedActiveCount(string assetCode)
        {
            if (string.IsNullOrWhiteSpace(assetCode))
            {
                return 0;
            }

            var associatedReturnRequests = _returnRequestRepository.GetAll()
                .Where(rr => rr.Assignment.Asset.AssetCode == assetCode);
            return associatedReturnRequests.Count();
        }

        public async Task<GetReturnRequestsPagedResponseDTO> Search(
            PaginationParameters paginationParameters,
            int adminId,
            string searchQuery
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception(Message.UnauthorizedUser);
            }

            var resultingSearch = _returnRequestRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                resultingSearch = resultingSearch.Where(rr =>
                            rr.RequestedByUser.UserName.StartsWith(searchQuery)
                            || rr.Assignment.Asset.AssetCode.StartsWith(searchQuery)
                            || rr.Assignment.Asset.AssetName.StartsWith(searchQuery)
                        );
            }

            var returnRequests = PagedList<ReturnRequest>.ToPagedList(
                resultingSearch,
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetReturnRequestsPagedResponseDTO
            {
                CurrentPage = returnRequests.CurrentPage,
                PageSize = returnRequests.PageSize,
                TotalCount = returnRequests.TotalCount,
                TotalPages = returnRequests.TotalPages,
                HasNext = returnRequests.HasNext,
                HasPrevious = returnRequests.HasPrevious,
                Items = returnRequests.Select(rr => _mapper.Map<ReturnRequestDTO>(rr))
            };
        }

        public async Task Approve(int rrId, int adminId)
        {
            var returnRequest = await _returnRequestRepository.GetById(rrId);
            if (returnRequest == null)
            {
                throw new Exception(Message.ReturnRequestNotFound);
            }

            var admin = await _userRepository.GetById(adminId);
            if (admin.Type != UserType.Admin || admin.Status == UserStatus.Disabled)
            {
                throw new Exception(Message.UnauthorizedUser);
            }
            var associatedAssignment = returnRequest.Assignment;
            var associatedAsset = await _assetRepository.GetById(associatedAssignment.AssetId);
            await _assignmentRepository.Delete(associatedAssignment);
            associatedAsset.State = AssetState.Available;
            await _assetRepository.Update(associatedAsset);

            returnRequest.State = RequestState.Completed;
            returnRequest.AcceptedByUser = admin;
            returnRequest.ReturnedDate = DateTime.Now.Date;
            await _returnRequestRepository.Update(returnRequest);
        }
        public async Task Deny(int rrId, int adminId)
        {
            var returnRequest = await _returnRequestRepository.GetById(rrId);
            if (returnRequest == null)
            {
                throw new Exception(Message.ReturnRequestNotFound);
            }

            var admin = await _userRepository.GetById(adminId);
            if (admin.Type != UserType.Admin || admin.Status == UserStatus.Disabled)
            {
                throw new Exception(Message.UnauthorizedUser);
            }
            
            await _returnRequestRepository.Delete(returnRequest);
        }
    }
}