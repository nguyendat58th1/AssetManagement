using System.Linq;
using BackEndAPI.Interfaces;
using BackEndAPI.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BackEndAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using AutoMapper;

namespace BackEndAPI.Services
{
    public class AssetService : IAssetService
    {

        private readonly IMapper _mapper;
        private readonly IAsyncUserRepository _userRepository;
        private readonly IAsyncAssetRepository _assetRepository;
        private readonly IAsyncAssetCategoryRepository _categoryRepository;
        private readonly IAsyncAssignmentRepository _assignmentRepository;
        
        public AssetService(
            IAsyncAssetRepository assetRepository,
            IAsyncUserRepository userRepository,
            IAsyncAssetCategoryRepository categoryRepository,
            IAsyncAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _assetRepository = assetRepository;
            _categoryRepository = categoryRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<GetAssetsListPagedResponseDTO> GetAssets(
            PaginationParameters paginationParameters,
            int adminId
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assets = PagedList<Asset>.ToPagedList(
                _assetRepository.GetAll()
                    .Where(u =>
                    (u.State == AssetState.Available || u.State == AssetState.NotAvailable || u.State == AssetState.Assigned)
                    && u.Location == adminUser.Location
                ).OrderBy(c => c.AssetCode),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssetsListPagedResponseDTO
            {
                CurrentPage = assets.CurrentPage,
                PageSize = assets.PageSize,
                TotalCount = assets.TotalCount,
                TotalPages = assets.TotalPages,
                HasNext = assets.HasNext,
                HasPrevious = assets.HasPrevious,
                Items = assets.Select(u => _mapper.Map<AssetDTO>(u))
            };
        }

        public async Task<Asset> GetById(int id)
        {

            var asset = await _assetRepository.GetById(id);
            if (asset == null)
            {

                throw new InvalidOperationException(Message.InvalidId);

            }

            return asset;
        }

        public async Task<GetAssetsListPagedResponseDTO> GetAssetsByCategory(
            PaginationParameters paginationParameters,
            int adminId,
            int categoryId
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assets = PagedList<Asset>.ToPagedList(
                _assetRepository.GetAll()
                    .Where(u =>
                    u.Location == adminUser.Location
                    && u.CategoryId == categoryId
                ),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssetsListPagedResponseDTO
            {
                CurrentPage = assets.CurrentPage,
                PageSize = assets.PageSize,
                TotalCount = assets.TotalCount,
                TotalPages = assets.TotalPages,
                HasNext = assets.HasNext,
                HasPrevious = assets.HasPrevious,
                Items = assets.Select(u => _mapper.Map<AssetDTO>(u))
            };
        }

        public async Task<GetAssetsListPagedResponseDTO> GetAssetsByState(
            PaginationParameters paginationParameters,
            int adminId,
            AssetState state
        )
        {
            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assets = PagedList<Asset>.ToPagedList(
                _assetRepository.GetAll()
                    .Where(u =>
                    u.Location == adminUser.Location
                    && u.State == state
                ),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssetsListPagedResponseDTO
            {
                CurrentPage = assets.CurrentPage,
                PageSize = assets.PageSize,
                TotalCount = assets.TotalCount,
                TotalPages = assets.TotalPages,
                HasNext = assets.HasNext,
                HasPrevious = assets.HasPrevious,
                Items = assets.Select(u => _mapper.Map<AssetDTO>(u))
            };
        }

        public async Task<GetAssetsListPagedResponseDTO> SearchAssets(
                    PaginationParameters paginationParameters,
                    int adminId,
                    string searchText
                )
        {
            if (searchText == null)
            {
                throw new Exception("Null search query");
            }

            var adminUser = await _userRepository.GetById(adminId);
            if (adminUser.Type != UserType.Admin)
            {
                throw new Exception("Unauthorized access");
            }

            var assets = PagedList<Asset>.ToPagedList(
                _assetRepository.GetAll()
                    .Where(u =>
                    u.Location == adminUser.Location
                    &&
                    (
                        u.AssetName.StartsWith(searchText)
                        || u.AssetCode.StartsWith(searchText)
                    )
                    ),
                paginationParameters.PageNumber,
                paginationParameters.PageSize
            );

            return new GetAssetsListPagedResponseDTO
            {
                CurrentPage = assets.CurrentPage,
                PageSize = assets.PageSize,
                TotalCount = assets.TotalCount,
                TotalPages = assets.TotalPages,
                HasNext = assets.HasNext,
                HasPrevious = assets.HasPrevious,
                Items = assets.Select(u => _mapper.Map<AssetDTO>(u))
            };
        }

        public async Task<Asset> Create(CreateAssetModel model)
        {

            if (model == null)
            {

                throw new ArgumentNullException(Message.NullAsset);

            }

            Asset asset = _mapper.Map<Asset>(model);

            var assetNumber = _assetRepository.CountingAssetNumber(asset.CategoryId);
            if (assetNumber < 0)
            {
                throw new Exception(Message.AssetNumberError);
            }

            var category = await _categoryRepository.GetById(asset.CategoryId);
            if (category == null)
            {

                throw new Exception(Message.InvalidId);

            }

            asset.AssetCode = AutoGenerator.AutoGeneratedAssetCode(assetNumber, category.CategoryCode);

            return await _assetRepository.Create(asset);
        }
        public async Task<IQueryable<Asset>> GetAllAssets(int userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listAsset = _assetRepository.GetAll()
                               .Where(x => x.Location == user.Location && x.State == AssetState.Available)
                               .AsQueryable();

            return listAsset;
        }

        public async Task<IQueryable<Asset>> GetAssetsBySearching(int userId, string searchText)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Can not find user");
            }
            var listAsset = _assetRepository.GetAll()
                               .Where(x => x.Location == user.Location 
                               && x.State == AssetState.Available
                               && (x.AssetCode.Contains(searchText)
                               || x.AssetName.Contains(searchText)))
                               .AsQueryable();
            return listAsset;
        }

        public async Task Update(int id, EditAssetModel model)
        {
            var asset = await _assetRepository.GetById(id);

            if (asset == null)
            {
                throw new InvalidOperationException(Message.NullAsset);
            }

            asset.AssetName = model.AssetName;
            asset.Specification = model.Specification;
            asset.InstalledDate = model.InstalledDate;
            asset.State = model.State;
            await _assetRepository.Update(asset);
        }

        public async Task Delete(int id)
        {

            if (id <= 0)
            {

                throw new InvalidOperationException(Message.InvalidId);

            }

            var countAsset = _assignmentRepository.GetCountAsset(id);
            if (countAsset > 0)
            {

                throw new Exception(Message.AssetHadHistoricalAssignment);

            }

            Asset asset = await _assetRepository.GetById(id);
            if (asset == null)
            {

                throw new Exception(Message.NullAsset);

            }

            await _assetRepository.Delete(asset);
        }
    }
}