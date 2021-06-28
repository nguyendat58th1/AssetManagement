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
using BackEndAPI.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace BackEndAPI_Tests.Services_Tests
{
    [TestFixture]
    public class AssetCategoryService_Tests
    {

        private static IQueryable<AssetCategory> Categories
        {
            get
            {
                return new List<AssetCategory>
                {
                    new AssetCategory
                    {
                        Id = 1,
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    },
                    new AssetCategory
                    {
                        Id = 2,
                        CategoryCode = "PC",
                        CategoryName = "Personal Computer"
                    }
                }
                .AsQueryable();
            }
        }

        private static IMapper _mapper;

        private Mock<IAsyncAssetCategoryRepository> _repositoryMock;

        public AssetCategoryService_Tests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IAsyncAssetCategoryRepository>(behavior: MockBehavior.Strict);
        }

        [Test]
        public void GetAll_Default_ReturnCategoryList()
        {
            //Arrange
            _repositoryMock.Setup(x => x.GetAll()).Returns(Categories);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var categories = categoryService.GetAll();

            //Assert
            Assert.AreEqual(2, categories.Count());
        }

        [Test]
        public void Create_NullAssetCategoryInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateCategoryModel categoryModel = null;

            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var result = Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryService.Create(categoryModel));

            //Assert
            Assert.AreEqual(Message.NullAssetCategory, result.ParamName);

        }

        [Test]
        public void Create_AssetCategoryNameAlreadyExists_ThrowsExceptionMessage()
        {

            //Arrange
            CreateCategoryModel categoryModel = new CreateCategoryModel
            {
                CategoryName = "Laptop"
            };

            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            _repositoryMock.Setup(x => x.DidCategoryNameExist(It.IsAny<string>())).Returns(true);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await categoryService.Create(categoryModel));

            //Assert
            Assert.AreEqual(Message.CategoryNameExisted, result.Message);

        }

        [Test]
        public void Create_AssetCategoryCodeAlreadyExists_ThrowsExceptionMessage()
        {

            //Arrange
            CreateCategoryModel categoryModel = new CreateCategoryModel
            {
                CategoryName = "Laptop Alien",
                CategoryCode = "LA"
            };

            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            _repositoryMock.Setup(x => x.DidCategoryNameExist(It.IsAny<string>())).Returns(false);
            _repositoryMock.Setup(x => x.DidCategoryCodeExist(It.IsAny<string>())).Returns(true);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await categoryService.Create(categoryModel));

            //Assert
            Assert.AreEqual(Message.CategoryCodeExisted, result.Message);

        }

        [Test]
        public async Task Create_ValidAssetCategoryInserted_ReturnsCreatedAssetCategory()
        {

            //Arrange
            CreateCategoryModel categoryModel = new CreateCategoryModel
            {
                CategoryName = "Monitor",
                CategoryCode = "MO"
            };

            AssetCategory category = _mapper.Map<AssetCategory>(categoryModel);

            AssetCategory createdCategory = new AssetCategory
            {
                CategoryName = "Monitor",
                CategoryCode = "MO"
            };

            _repositoryMock.Setup(x => x.DidCategoryNameExist(It.IsAny<string>())).Returns(false);
            _repositoryMock.Setup(x => x.DidCategoryCodeExist(It.IsAny<string>())).Returns(false);
            _repositoryMock.Setup(x => x.Create(It.IsAny<AssetCategory>())).ReturnsAsync(createdCategory);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act

            var result = await categoryService.Create(categoryModel);

            //Assert
            Assert.AreEqual(result.CategoryName, createdCategory.CategoryName);
            Assert.AreEqual(result.CategoryCode, createdCategory.CategoryCode);

        }

        [TestCase("LA")]
        public void DidCategoryCodeExist_AlreadyExistingCategoryCodeInserted_ReturnsTrue(string categoryCode)
        {

            //Arrange
            _repositoryMock.Setup(x => x.DidCategoryCodeExist(It.IsAny<string>())).Returns(true);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var result = categoryService.DidCategoryCodeExist(categoryCode);

            //Assert
            Assert.AreEqual(true, result);

        }

        [TestCase("MU")]
        public void DidCategoryCodeExist_NotExistingCategoryCodeInserted_ReturnsTrue(string categoryCode)
        {

            //Arrange
            _repositoryMock.Setup(x => x.DidCategoryCodeExist(It.IsAny<string>())).Returns(false);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var result = categoryService.DidCategoryCodeExist(categoryCode);

            //Assert
            Assert.AreEqual(false, result);

        }

        [TestCase("Laptop")]
        public void DidCategoryNameExist_AlreadyExistingCategoryNameInserted_ReturnsTrue(string categoryName)
        {

            //Arrange
            _repositoryMock.Setup(x => x.DidCategoryNameExist(It.IsAny<string>())).Returns(true);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var result = categoryService.DidCategoryNameExist(categoryName);

            //Assert
            Assert.AreEqual(true, result);

        }

        [TestCase("Mouse")]
        public void DidCategoryNameExist_NotExistingCategoryNameInserted_ReturnsTrue(string categoryName)
        {

            //Arrange
            _repositoryMock.Setup(x => x.DidCategoryNameExist(It.IsAny<string>())).Returns(false);
            var categoryService = new AssetCategoryService(
                _repositoryMock.Object,
                _mapper
            );

            //Act
            var result = categoryService.DidCategoryNameExist(categoryName);

            //Assert
            Assert.AreEqual(false, result);

        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}