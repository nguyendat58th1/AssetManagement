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
    public class AssetRepository_Tests
    {
        private AssetsManagementDBContext _context;
        private IAsyncAssetRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "AssetDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new AssetRepository(_context);
        }

        [Test]
        public async Task Create_AddNewAsset_WritesToDatabase()
        {

            //Arrange
            var asset = new Asset
            {
                AssetName = "Asus 2021",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            //Act
            var _asset = await _repository.Create(asset);
            await _context.SaveChangesAsync();

            //Assert
            Assert.AreEqual(1, _context.Assets.Count());
            Assert.AreEqual(_asset, asset);

        }

        [TestCase(1)]
        public async Task GetById_GetDataOfAssetByAnId_ReturnsAnAsset(int id)
        {

            //Arrange
            var assets = new List<Asset>
            {
                new Asset
                {
                    AssetName = "Asus 2021",
                    CategoryId = 1,
                    AssetCode = "LA0000001",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2022",
                    AssetCode = "LA0000002",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2023",
                    CategoryId = 1,
                    AssetCode = "LA0000003",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                }
            };
            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();

            //Act
            var _asset = await _repository.GetById(id);

            //Assert
            Assert.AreEqual(_asset.Id, id);

        }

        [TestCase(1)]
        public async Task Update_UpdateAnAsset_WritesToDatabase(int id)
        {

            //Arrange
            var assets = new List<Asset>
            {
                new Asset
                {
                    AssetName = "Asus 2021",
                    CategoryId = 1,
                    AssetCode = "LA0000001",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2022",
                    AssetCode = "LA0000002",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2023",
                    CategoryId = 1,
                    AssetCode = "LA0000003",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                }
            };
            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();

            //Act
            var _asset = await _repository.GetById(id);
            _asset.State = AssetState.WaitingForRecycling;
            await _repository.Update(_asset);

            //Assert
            Assert.AreEqual(3, _context.Assets.Count());
            Assert.AreEqual(_asset.State, AssetState.WaitingForRecycling);

        }

        [TestCase(1)]
        public async Task Delete_DeleteAnAsset_WritesToDatabase(int id)
        {

            //Arrange
            var assets = new List<Asset>
            {
                new Asset
                {
                    AssetName = "Asus 2021",
                    CategoryId = 1,
                    AssetCode = "LA0000001",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2022",
                    AssetCode = "LA0000002",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2023",
                    CategoryId = 1,
                    AssetCode = "LA0000003",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                }
            };
            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();

            //Act
            var _asset = await _repository.GetById(id);
            await _repository.Delete(_asset);
            var asset = await _repository.GetById(id);

            //Assert
            Assert.AreEqual(2, _context.Assets.Count());
            Assert.AreEqual(asset, null);

        }

        [TestCase(-1)]
        [TestCase(0)]
        public void CountingAssetNumber_ZeroOrNegativeNumberInserted_ThrowExceptionMessage(int cateId)
        {

            //Act
            var result = Assert.Throws<InvalidOperationException>(() => _repository.CountingAssetNumber(cateId));

            //Assert
            Assert.AreEqual(Message.InvalidId, result.Message);

        }

        [TestCase(1)]
        public async Task CountingAssetNumber_PositiveNumberInserted_ThrowExceptionMessage(int cateId)
        {
            //Arrange
            var assets = new List<Asset>
            {
                new Asset
                {
                    AssetName = "Asus 2021",
                    CategoryId = 1,
                    AssetCode = "LA0000001",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2022",
                    AssetCode = "LA0000002",
                    CategoryId = 1,
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                },
                new Asset
                {
                    AssetName = "Asus 2023",
                    CategoryId = 1,
                    AssetCode = "LA0000003",
                    InstalledDate = DateTime.Now,
                    Location = 0,
                    Specification = "None",
                    State = 0
                }
            };
            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();


            //Act
            var result = _repository.CountingAssetNumber(cateId);

            //Assert
            Assert.AreEqual(3, result);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeletedAsync();
            _context.DisposeAsync();
        }
    }
}