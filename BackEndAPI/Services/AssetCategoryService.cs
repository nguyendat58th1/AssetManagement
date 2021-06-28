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
    public class AssetCategoryService : IAssetCategoryService
    {

        private readonly IMapper _mapper;
        private readonly IAsyncAssetCategoryRepository _repository;

        public AssetCategoryService(IAsyncAssetCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AssetCategory> Create(CreateCategoryModel model)
        {

            if (model == null)
            {

                throw new ArgumentNullException(Message.NullAssetCategory);

            }

            bool nameExisted = _repository.DidCategoryNameExist(model.CategoryName);
            if (nameExisted)
            {

                throw new Exception(Message.CategoryNameExisted);

            }

            bool codeExisted = _repository.DidCategoryCodeExist(model.CategoryCode);
            if (codeExisted)
            {

                throw new Exception(Message.CategoryCodeExisted);

            }

            AssetCategory category = _mapper.Map<AssetCategory>(model);

            return await _repository.Create(category);
        }

        public bool DidCategoryCodeExist(string categoryCode)
        {

            return _repository.DidCategoryCodeExist(categoryCode);

        }

        public bool DidCategoryNameExist(string categoryName)
        {

            return _repository.DidCategoryNameExist(categoryName);

        }

        public IQueryable<AssetCategory> GetAll()
        {
            return _repository.GetAll().OrderBy(c=>c.CategoryName);
        }

    }
}