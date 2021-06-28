using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using Microsoft.Extensions.Options;

namespace BackEndAPI.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncUserRepository _userrepository;
        private readonly IAsyncAssetRepository _assetrepository;
        private readonly IAsyncAssignmentRepository _repository;
        public AssignmentService(IAsyncAssignmentRepository repository, IAsyncUserRepository userrepository, IAsyncAssetRepository assetrepository, IMapper mapper)
        {
            _repository = repository;
            _userrepository = userrepository;
            _mapper = mapper;
            _assetrepository = assetrepository;
        }

        public async Task Create(int userId, AssignmentModel model)
        {
            var assignedToUserId = await _userrepository.GetById(model.AssignedToUserId);
            var assignedByUserId = await _userrepository.GetById(userId);
            var asset = await _assetrepository.GetById(model.AssetId);

            if (assignedByUserId.Type == UserType.User)
            {
                throw new Exception("The user is not an admin");
            }

            if (assignedByUserId.Status == UserStatus.Disabled)
            {
                throw new Exception("The user has been disabled");
            }

            if (assignedToUserId.Status == UserStatus.Disabled)
            {
                throw new Exception("The user has been disabled");
            }

            if (asset == null)
            {
                throw new InvalidOperationException("Can not find asset");
            }

            if (assignedToUserId == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (assignedByUserId == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (model == null)
            {
                throw new ArgumentNullException("Null assignment");
            }

            if (asset.State != AssetState.Available)
            {
                throw new Exception("Can not assign this asset");
            }

            if (model.AssignedDate.Date < DateTime.Now.Date)
            {
                throw new Exception("Assign Date is earlier than now");
            }

            Assignment assignment = _mapper.Map<Assignment>(model);

            assignment.AssignedByUserId = userId;
            assignment.State = AssignmentState.WaitingForAcceptance;
            assignment.AssignedToUserName = assignedToUserId.UserName;
            assignment.CreateEditDate = DateTime.Now;
            await _repository.Create(assignment);
            asset.State = AssetState.NotAvailable;
            await _assetrepository.Update(asset);
        }

        public async Task<GetAssignmentListPagedResponse> GetAllAssignments(PaginationParameters paginationParameters, int adminId)
        {
            var adminUser = await _userrepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assignments = PagedList<Assignment>.ToPagedList(
                _repository.GetAll().OrderByDescending(a => a.CreateEditDate)
                ,
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );
            return new GetAssignmentListPagedResponse
            {
                CurrentPage = assignments.CurrentPage,
                PageSize = assignments.PageSize,
                TotalCount = assignments.TotalCount,
                TotalPages = assignments.TotalPages,
                HasNext = assignments.HasNext,
                HasPrevious = assignments.HasPrevious,
                Items = assignments.Select(u => _mapper.Map<AssignmentDTO>(u))
            };
        }

        public async Task<Assignment> GetById(int id)
        {
            var assignment = await _repository.GetById(id);
            if (assignment == null)
            {
                throw new InvalidOperationException("Can not find assignment");
            }
            return assignment;
        }

        public async Task Update(int id, AssignmentModel model)
        {
            var assignment = await _repository.GetById(id);
            var assignedToUserId = await _userrepository.GetById(model.AssignedToUserId);
            var initalAsset = await _assetrepository.GetById(assignment.AssetId);
            var asset = await _assetrepository.GetById(model.AssetId);

            if (assignment == null)
            {
                throw new InvalidOperationException("Can not find assignment");
            }

            if (assignedToUserId.Status == UserStatus.Disabled)
            {
                throw new Exception("The user has been disabled");
            }

            if (asset == null)
            {
                throw new InvalidOperationException("Can not find asset");
            }

            if (assignedToUserId == null)
            {
                throw new InvalidOperationException("Can not find user");
            }

            if (model == null)
            {
                throw new ArgumentNullException("Null assignment");
            }

            if (asset.State != AssetState.Available && asset.Id != initalAsset.Id)
            {
                throw new Exception("Can not assign this asset");
            }

            if (model.AssignedDate.Date < DateTime.Now.Date)
            {
                throw new Exception("Assign Date is earlier than now");
            }

            if (assignment.State != AssignmentState.WaitingForAcceptance)
            {
                throw new Exception("State of this assignment is not 'Waiting for acceptance'");
            }

            assignment.AssetId = model.AssetId;
            assignment.AssignedToUserId = model.AssignedToUserId;
            assignment.AssignedDate = model.AssignedDate;
            assignment.Note = model.Note;
            assignment.AssignedToUserName = assignedToUserId.UserName;
            assignment.CreateEditDate = DateTime.Now;
            await _repository.Update(assignment);
            if (asset.Id != initalAsset.Id)
            {
                asset.State = AssetState.NotAvailable;
                await _assetrepository.Update(asset);
                initalAsset.State = AssetState.Available;
                await _assetrepository.Update(initalAsset);
            }

        }

        public async Task<GetAssignmentListPagedResponse> GetAssignmentByState(PaginationParameters paginationParameters, int adminId, AssignmentState state)
        {
            var adminUser = await _userrepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assignment = PagedList<Assignment>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                    u.State == state
                ).OrderByDescending(a => a.CreateEditDate),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssignmentListPagedResponse
            {
                CurrentPage = assignment.CurrentPage,
                PageSize = assignment.PageSize,
                TotalCount = assignment.TotalCount,
                TotalPages = assignment.TotalPages,
                HasNext = assignment.HasNext,
                HasPrevious = assignment.HasPrevious,
                Items = assignment.Select(u => _mapper.Map<AssignmentDTO>(u))
            };
        }

        public async Task<GetAssignmentListPagedResponse> SearchAssignments(PaginationParameters paginationParameters, int adminId, string searchText)
        {
            if (searchText == null)
            {
                throw new Exception("Null search query");
            }

            var adminUser = await _userrepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assignment = PagedList<Assignment>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                    (
                        u.Asset.AssetCode.Contains(searchText)
                        || u.Asset.AssetName.Contains(searchText)
                        || u.AssignedToUserName.Contains(searchText)
                    )
                    ).OrderByDescending(a => a.CreateEditDate),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssignmentListPagedResponse
            {
                CurrentPage = assignment.CurrentPage,
                PageSize = assignment.PageSize,
                TotalCount = assignment.TotalCount,
                TotalPages = assignment.TotalPages,
                HasNext = assignment.HasNext,
                HasPrevious = assignment.HasPrevious,
                Items = assignment.Select(u => _mapper.Map<AssignmentDTO>(u))
            };
        }

        public async Task<GetAssignmentListPagedResponse> GetAssignmentByDate(PaginationParameters paginationParameters, int adminId, 
        int year, int month, int day)
        {
             var adminUser = await _userrepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assignment = PagedList<Assignment>.ToPagedList(
                _repository.GetAll()
                    .Where(u =>
                     u.AssignedDate.Year == year && u.AssignedDate.Month == month && u.AssignedDate.Day == day
                ).OrderByDescending(a => a.CreateEditDate),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssignmentListPagedResponse
            {
                CurrentPage = assignment.CurrentPage,
                PageSize = assignment.PageSize,
                TotalCount = assignment.TotalCount,
                TotalPages = assignment.TotalPages,
                HasNext = assignment.HasNext,
                HasPrevious = assignment.HasPrevious,
                Items = assignment.Select(u => _mapper.Map<AssignmentDTO>(u))
            };
        }

        public IQueryable<Assignment> GetAll()
        {
            return _repository.GetAll().AsQueryable();
        }

        public async Task Delete(int id)
        {
            var assignment = await _repository.GetById(id);
            var asset = await _assetrepository.GetById(assignment.AssetId);
            if (assignment == null)
            {
                throw new InvalidOperationException("Can not find assignment");
            }

            if(assignment.State == AssignmentState.Accepted)
            {
                throw new Exception("Can not delete this assignment");
            }
            await _repository.Delete(assignment);
            asset.State = AssetState.Available;
            await _assetrepository.Update(asset);

        }

        public async Task<IQueryable<Assignment>> GetAllForUser(int userId)
        {
            var user = await _userrepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            return _repository.GetAll().Where(u => u.AssignedToUserId == userId
                                                    && u.AssignedDate.Date <= DateTime.Now.Date)
                                        .OrderBy(c=>c.State)
                                        .ThenBy(c=>c.Asset.AssetCode)
                                        .AsQueryable();
        }
        
    public async Task AcceptAssignment(int id)
    {
        var assignment = await _repository.GetById(id);
        if (assignment == null)
        {
            throw new InvalidOperationException("Can not find assignment");
        } 
        
        assignment.State = AssignmentState.Accepted;

        await _repository.Update(assignment);
    }

    public async Task DeclineAssignment(int id)
    {
      var assignment = await _repository.GetById(id);
        if (assignment == null)
        {
            throw new InvalidOperationException("Can not find assignment");
        } 
        
        assignment.State = AssignmentState.Declined;

        await _repository.Update(assignment);
    }

    public async Task UndoResponeAssignment(int id)
    {
      var assignment = await _repository.GetById(id);
        if (assignment == null)
        {
            throw new InvalidOperationException("Can not find assignment");
        } 
        
        assignment.State = AssignmentState.WaitingForAcceptance;

        await _repository.Update(assignment);
    }
    }
}