using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.DBContext;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using BackEndAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BackEndAPI_Tests.Repositories_Tests
{
    [TestFixture]
    public class AssetCategoryRepository_Tests
    {
        private AssetsManagementDBContext _context;
        private IAsyncAssetCategoryRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "CategoryDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new AssetCategoryRepository(_context);
        }

        [Test]
        public async Task Create_AddNewAssetCategory_WritesToDatabase()
        {

            //Arrange
            var category = new AssetCategory
                    {
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    };

            //Act
            var _category = await _repository.Create(category);
            await _context.SaveChangesAsync();

            //Assert
            Assert.AreEqual(1, _context.AssetCategories.Count());
            Assert.AreEqual(_category, category);

        }

        [Test]
        public async Task GetAll_Default_ShouldGetAllAvailableAssetCategories()
        {
            //Arrange
            var categories = new List<AssetCategory>
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
            };
            await _context.AssetCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            //Act
            var actualUsers = _repository.GetAll();

            //Assert
            Assert.That(actualUsers.Count() == 2);
            Assert.That(actualUsers.Any(u => u.CategoryCode == "LA"));
            Assert.That(actualUsers.Any(u => u.CategoryCode == "PC"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("            ")]
        public void DidCategoryNameExist_NullOrEmptyCategoryNameInserted_ThrowExceptionMessage(string categoryName)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => _repository.DidCategoryNameExist(categoryName));

            //Assert
            Assert.AreEqual(Message.NullOrEmptyCategoryName, result.ParamName);

        }

        [TestCase("Mouse")]
        public async Task DidCategoryNameExist_CategoryNameNotInListInserted_ReturnFalse(string categoryName)
        {

            //Arrange
            var categories = new List<AssetCategory>
            {
                new AssetCategory
                    {
                        Id = 1,
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    }
            };
            await _context.AssetCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            //Act
            var result = _repository.DidCategoryNameExist(categoryName);

            //Assert
            Assert.AreEqual(false, result);

        }
        
        [TestCase("Laptop")]
        public async Task DidCategoryNameExist_CategoryNameInListInserted_ReturnTrue(string categoryName)
        {

            //Arrange
            var categories = new List<AssetCategory>
            {
                new AssetCategory
                    {
                        Id = 1,
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    }
            };
            await _context.AssetCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            //Act
            var result = _repository.DidCategoryNameExist(categoryName);

            //Assert
            Assert.AreEqual(true, result);

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("            ")]
        public void DidCategoryCodeExist_NullOrEmptyCategoryCodeInserted_ThrowExceptionMessage(string categoryCode)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => _repository.DidCategoryCodeExist(categoryCode));

            //Assert
            Assert.AreEqual(Message.NullOrEmptyCategoryCode, result.ParamName);

        }

        [TestCase("MO")]
        public async Task DidCategoryCodeExist_CategoryCodeNotInListInserted_ReturnFalse(string categoryCode)
        {

            //Arrange
            var categories = new List<AssetCategory>
            {
                new AssetCategory
                    {
                        Id = 1,
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    }
            };
            await _context.AssetCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            //Act
            var result = _repository.DidCategoryCodeExist(categoryCode);

            //Assert
            Assert.AreEqual(false, result);

        }
        
        [TestCase("LA")]
        public async Task DidCategoryCodeExist_CategoryCodeInListInserted_ReturnTrue(string categoryCode)
        {

            //Arrange
            var categories = new List<AssetCategory>
            {
                new AssetCategory
                    {
                        Id = 1,
                        CategoryCode = "LA",
                        CategoryName = "Laptop"
                    }
            };
            await _context.AssetCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            //Act
            var result = _repository.DidCategoryCodeExist(categoryCode);

            //Assert
            Assert.AreEqual(true, result);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeletedAsync();
            _context.DisposeAsync();
        }
    }
}