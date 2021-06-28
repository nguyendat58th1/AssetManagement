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
    public class AssetService_Tests
    {

        private static IQueryable<Asset> Assets
        {
            get
            {
                return new List<Asset>
                {
                    new Asset
                    {
                        Id=1,
                        AssetName = "Asus 2021",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = Location.HoChiMinh,
                        Specification = "None",
                        State = 0
                    },
                    new Asset
                    {
                        Id=2,
                        AssetName = "Asus 2022",
                        AssetCode = "LA0000001",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = 0,
                        Specification = "None",
                        State = 0
                    },
                    new Asset
                    {
                        Id=3,
                        AssetName = "Asus 2023",
                        AssetCode = "LA0000002",
                        CategoryId = 1,
                        InstalledDate = DateTime.Now,
                        Location = 0,
                        Specification = "None",
                        State = 0
                    },
                    new Asset{Id = 4, Location = 0, State = 0},
                    new Asset{Id = 5, Location = 0, State = 0},
                    new Asset{Id = 6, Location = 0, State = 0},
                    new Asset{Id = 7, Location = 0, State = 0},
                    new Asset{Id = 8, Location = 0, State = 0},
                    new Asset{Id = 9, Location = 0, State = 0},
                    new Asset{Id = 10, Location = 0, State = 0},
                    new Asset{Id = 11, Location = 0, State = 0},
                    new Asset{Id = 12, Location = 0, State = 0},
                    new Asset{Id = 13, Location = 0, State = 0},
                    new Asset{Id = 14, Location = 0, State = 0},
                    new Asset{Id = 15, Location = 0, State = 0},
                    new Asset{Id = 16, Location = 0, State = 0},
                    new Asset{Id = 17, Location = 0, State = 0},
                    new Asset{Id = 18, Location = 0, State = 0},
                    new Asset{Id = 19, Location = 0, State = 0},
                    new Asset{Id = 20, Location = 0, State = 0},
                    new Asset{Id = 21, Location = 0, State = 0},
                    new Asset{Id = 22, Location = 0, State = 0},
                    new Asset{Id = 23, Location = 0, State = 0},
                    new Asset{Id = 24, Location = 0, State = 0},
                    new Asset{Id = 25, Location = 0, State = 0},
                    new Asset{Id = 26, Location = 0, State = 0},
                    new Asset{Id = 27, Location = 0, State = 0},
                    new Asset{Id = 28, Location = 0, State = 0},
                    new Asset{Id = 29, Location = 0, State = 0},
                    new Asset{Id = 30, Location = 0, State = 0}
                }
                .AsQueryable();
            }
        }

        private static IQueryable<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User
                    {
                        Id = 1,
                        StaffCode = "SD0001",
                        FirstName = "Binh",
                        LastName = "Nguyen Van",
                        DateOfBirth = new DateTime(1993, 01, 20),
                        JoinedDate = new DateTime(2021, 12, 05),
                        Gender = Gender.Male,
                        Type = UserType.Admin,
                        UserName = "binhnv",
                        Password = "binhnv@20011993",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 2,
                        StaffCode = "SD0002",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1994, 01, 12).Date,
                        JoinedDate = new DateTime(2021, 12, 05).Date,
                        Gender = Gender.Female,
                        Type = UserType.Admin,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HoChiMinh,
                        Status = UserStatus.Active
                    },
                    new User{ Id = 3, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 4, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 5, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 6, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 7, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 8, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 9, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 10, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 11, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 12, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 13, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 14, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 15, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 16, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 17, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 18, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 19, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 20, Location = Location.HaNoi, Type = UserType.User},
                    new User{ Id = 21, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 22, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 23, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 24, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 25, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 26, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 27, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 28, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 29, Location = Location.HoChiMinh, Type = UserType.User},
                    new User{ Id = 30, Location = Location.HoChiMinh, Type = UserType.User}
                }
                .AsQueryable();
            }
        }

        private static IMapper _mapper;

        private Mock<IAsyncAssetRepository> _assetRepositoryMock;

        private Mock<IAsyncUserRepository> _userRepositoryMock;

        private Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;

        private Mock<IAsyncAssetCategoryRepository> _categoryRepositoryMock;

        public AssetService_Tests()
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
            _userRepositoryMock = new Mock<IAsyncUserRepository>(behavior: MockBehavior.Strict);
            _assetRepositoryMock = new Mock<IAsyncAssetRepository>(behavior: MockBehavior.Strict);
            _userRepositoryMock = new Mock<IAsyncUserRepository>(behavior: MockBehavior.Strict);
            _categoryRepositoryMock = new Mock<IAsyncAssetCategoryRepository>(behavior: MockBehavior.Strict);
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(behavior: MockBehavior.Strict);
        }

        [Test]
        public async Task GetAssets_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Assets.Single(u => u.Id == 1));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var assetsPagedListResponse = await assetService.GetAssets(parameters, 1);

            //Assert
            var expectedCount = Assets.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, assetsPagedListResponse.TotalCount);
            Assert.AreEqual(1, assetsPagedListResponse.CurrentPage);
            Assert.AreEqual(1, assetsPagedListResponse.TotalPages);
            Assert.IsFalse(assetsPagedListResponse.HasNext);
            Assert.IsFalse(assetsPagedListResponse.HasPrevious);
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public async Task GetAssets_WithDifferentValidPageSize_ShouldReturnProperlyPagedListResponse(int pageSize)
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Assets.Single(u => u.Id == 1));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );
            var expectedCount = Assets.Where(u => u.Location == Location.HaNoi).Count();


            var expectedTotalPages = (int)Math.Ceiling(expectedCount / (double)pageSize);
            List<PaginationParameters> parametersList = new List<PaginationParameters>();
            for (int i = 1; i <= expectedTotalPages; i++)
            {
                parametersList.Add(
                    new PaginationParameters
                    {
                        PageNumber = i,
                        PageSize = pageSize
                    }
                );
            }

            int actualCount = 0;
            foreach (var parameters in parametersList)
            {
                //Act
                var assetsPagedListResponse = await assetService.GetAssets(parameters, 1);
                actualCount += assetsPagedListResponse.Items.Count();

                //Assert
                Assert.AreEqual(parameters.PageNumber, assetsPagedListResponse.CurrentPage);
                Assert.AreEqual(expectedTotalPages, assetsPagedListResponse.TotalPages);
                Assert.AreEqual(parameters.PageNumber < expectedTotalPages, assetsPagedListResponse.HasNext);
                Assert.AreEqual(parameters.PageNumber > 1, assetsPagedListResponse.HasPrevious);
            }

            Assert.AreEqual(expectedCount, actualCount);

        }

        [Test]
        public async Task GetAssets_WithNegativePageNumber_ShouldReturnPagedListResponseWithDefaultPageNumberOf1()
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Assets.Single(u => u.Id == 1));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = -1,
                PageSize = 30
            };

            //Act
            var assetsPagedListResponse = await assetService.GetAssets(parameters, 1);

            //Assert
            var expectedCount = Assets.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, assetsPagedListResponse.TotalCount);
            Assert.AreEqual(1, assetsPagedListResponse.CurrentPage);
            Assert.AreEqual(1, assetsPagedListResponse.TotalPages);
            Assert.IsFalse(assetsPagedListResponse.HasNext);
            Assert.IsFalse(assetsPagedListResponse.HasPrevious);
        }

        [Test]
        public async Task GetAssets_WithPageSizeSmallerThanMinOf10_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf10()
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Assets.Single(u => u.Id == 1));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 0
            };

            //Act
            var assetsPagedListResponse = await assetService.GetAssets(parameters, 1);

            //Assert
            var expectedCount = Assets.Where(u => u.Location == Location.HaNoi).Count();
            var expectedTotalPages = (int)Math.Ceiling(expectedCount / 10.0);
            Assert.AreEqual(expectedCount, assetsPagedListResponse.TotalCount);
            Assert.AreEqual(1, assetsPagedListResponse.CurrentPage);
            Assert.AreEqual(expectedTotalPages, assetsPagedListResponse.TotalPages);
            Assert.IsTrue(assetsPagedListResponse.HasNext);
            Assert.IsFalse(assetsPagedListResponse.HasPrevious);
        }

        [Test]
        public async Task GetAssets_WithPageSizeBiggerThanMaxOf50_ShouldReturnPagedListResponseWithDefaultMinPageSizeOf50()
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Assets.Single(u => u.Id == 1));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 100
            };

            //Act
            var assetsPagedListResponse = await assetService.GetAssets(parameters, 1);

            //Assert
            var expectedCount = Assets.Where(u => u.Location == Location.HaNoi).Count();
            Assert.AreEqual(expectedCount, assetsPagedListResponse.TotalCount);
            Assert.AreEqual(1, assetsPagedListResponse.CurrentPage);
            Assert.AreEqual(1, assetsPagedListResponse.TotalPages);
            Assert.IsFalse(assetsPagedListResponse.HasNext);
            Assert.IsFalse(assetsPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetAssets_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _assetRepositoryMock.Setup(x => x.GetAll()).Returns(Assets);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 100
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await assetService.GetAssets(parameters, 70);
                }
            );

            Assert.AreEqual("Unauthorized access", exception.Message);
        }

        [Test]
        public void Create_NullAssetInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = null;

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var result = Assert.ThrowsAsync<ArgumentNullException>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.NullAsset, result.ParamName);

        }

        [Test]
        public void Create_NegativeAssetNumberInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(-1);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.AssetNumberError, result.Message);

        }

        [Test]
        public void Create_InvalidAssetCategoryIdInserted_ThrowsExceptionMessage()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            AssetCategory invalidCategory = null;
            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(0);
            _categoryRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(invalidCategory);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await assetService.Create(assetModel));

            //Assert
            Assert.AreEqual(Message.InvalidId, result.Message);

        }

        [Test]
        public async Task Create_ValidAssetInserted_ReturnsCreatedAsset()
        {

            //Arrange
            CreateAssetModel assetModel = new CreateAssetModel
            {
                AssetName = "Asus 2029",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            var category = new AssetCategory
            {
                Id = 1,
                CategoryCode = "LA",
                CategoryName = "Laptop"
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            Asset asset = _mapper.Map<Asset>(assetModel);

            Asset createdAsset = new Asset
            {
                AssetName = "Asus 2029",
                AssetCode = "LA000001",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = 0,
                Specification = "None",
                State = 0
            };

            _assetRepositoryMock.Setup(x => x.CountingAssetNumber(It.IsAny<int>())).Returns(0);
            _categoryRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(category);
            _assetRepositoryMock.Setup(x => x.Create(It.IsAny<Asset>())).ReturnsAsync(createdAsset);

            //Act
            var result = await assetService.Create(assetModel);

            //Assert
            Assert.AreEqual(result.AssetName, createdAsset.AssetName);
            Assert.AreEqual(result.AssetCode, createdAsset.AssetCode);
            Assert.AreEqual(result.CategoryId, createdAsset.CategoryId);
            Assert.AreEqual(result.InstalledDate, createdAsset.InstalledDate);
            Assert.AreEqual(result.Location, createdAsset.Location);
            Assert.AreEqual(result.Specification, createdAsset.Specification);
            Assert.AreEqual(result.State, createdAsset.State);

        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(31)]
        public void GetById_InvalidIdInserted_ThrowsExceptionMessage(int id)
        {

            //Arrange
            Asset asset = null;

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(asset);

            //Act
            var result = Assert.ThrowsAsync<InvalidOperationException>(async () => await assetService.GetById(id));

            //Assert
            Assert.AreEqual(Message.InvalidId, result.Message);

        }

        [TestCase(1)]
        public async Task GetById_ValidIdInserted_ReturnsAsset(int id)
        {

            //Arrange
            Asset asset = new Asset
            {
                Id = 1,
                AssetName = "Asus 2021",
                CategoryId = 1,
                InstalledDate = DateTime.Now,
                Location = Location.HoChiMinh,
                Specification = "None",
                State = 0
            };

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(asset);

            //Act
            var result = await assetService.GetById(id);

            //Assert
            Assert.AreEqual(result, asset);

        }

        [Test]
        public void Update_InvalidIdInserted_ReturnsExceptionMessage()
        {

            //Arrange
            Asset nullAsset = null;
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(nullAsset);

            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            var editModel = new EditAssetModel
            {
                State = AssetState.WaitingForRecycling
            };

            //Act
            var exception = Assert.ThrowsAsync<InvalidOperationException>(
               async () =>
               {
                   await assetService.Update(It.IsAny<int>(), editModel);
               }
           );

            //Assert
            Assert.AreEqual(Message.NullAsset, exception.Message);
        }

        [Test]
        public async Task Update_Valid_ShouldBeSuccessful()
        {

            //Arrage
            var dontMatterAsset = new Asset { };
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterAsset);
            _assetRepositoryMock.Setup(x => x.Update(dontMatterAsset)).Returns(Task.CompletedTask).Verifiable();
            
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            var editModel = new EditAssetModel
            {
                State = AssetState.WaitingForRecycling
            };

            //Act
            await assetService.Update(It.IsAny<int>(), editModel);

            //Assert
            _assetRepositoryMock.Verify(x => x.Update(dontMatterAsset), Times.Once());
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Delete_NotFoundId_ReturnsExceptionMessage(int id)
        {

            //Arrange
            var asset = new Asset { };
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<Asset>(null));
            
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                {
                    await assetService.Delete(id);
                }
             );


            //Assert
            Assert.AreSame(Message.InvalidId, exception.Message);
        }
        
        [TestCase(1)]
        public void Delete_ValidInAssignment_ReturnsExceptionMessage(int id)
        {

            //Arrange
            var Asset = new Asset { };
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Asset);
            _assignmentRepositoryMock.Setup(x => x.GetCountAsset(It.IsAny<int>())).Returns(1);
            
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await assetService.Delete(id);
                }
             );

            //Assert
            Assert.AreSame(Message.AssetHadHistoricalAssignment, exception.Message);

        }

        [TestCase(2)]
        public async Task Delete_Valid_ShouldBeSuccessful(int id)
        {
            //Arrange
            var Asset = new Asset { };
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Asset);
            _assetRepositoryMock.Setup(x => x.Delete(Asset)).Returns(Task.CompletedTask).Verifiable();
            _assignmentRepositoryMock.Setup(x => x.GetCountAsset(It.IsAny<int>())).Returns(0);
            
            var assetService = new AssetService(
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            await assetService.Delete(id);

            //Assert
            _assetRepositoryMock.Verify(x => x.Delete(Asset), Times.Once());
        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}