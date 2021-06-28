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
    public class AssignmentService_Test
    {
        private static IQueryable<Assignment> Assignments
        {
            get
            {
                return new List<Assignment>
                {
                    new Assignment
                    {
                        Id = 1,
                        AssetId = 1,
                        AssignedByUserId = 1,
                        AssignedToUserId = 2,
                        Note = "Testing1",
                        State = AssignmentState.WaitingForAcceptance
                    },
                    new Assignment
                    {
                        Id = 2,
                        AssetId = 2,
                        AssignedByUserId = 1,
                        AssignedToUserId = 3,
                        Note = "Testing1",
                        State = AssignmentState.WaitingForAcceptance
                    },
                }
                .AsQueryable();
            }
        }

        private static IMapper _mapper;

        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;
        private Mock<IAsyncAssetRepository> _assetRepositoryMock;


        public AssignmentService_Test()
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
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(behavior: MockBehavior.Strict);
            _assetRepositoryMock = new Mock<IAsyncAssetRepository>(behavior: MockBehavior.Strict);
        }

        [Test]
        public async Task GetAssignments_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            var dontMatterUser = new User { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser);
            _assignmentRepositoryMock.Setup(x => x.GetAll()).Returns(Assignments);
            var assignmentService = new AssignmentService(
             _assignmentRepositoryMock.Object,
             _userRepositoryMock.Object,
             _assetRepositoryMock.Object,
             _mapper
         );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var assignmentPagedListResponse = await assignmentService.GetAllAssignments(parameters, 1);

            //Assert
            var expectedCount = Assignments.Where(u => u.State != AssignmentState.Declined).Count();
            Assert.AreEqual(expectedCount, assignmentPagedListResponse.TotalCount);
            Assert.AreEqual(1, assignmentPagedListResponse.CurrentPage);
            Assert.AreEqual(1, assignmentPagedListResponse.TotalPages);
            Assert.IsFalse(assignmentPagedListResponse.HasNext);
            Assert.IsFalse(assignmentPagedListResponse.HasPrevious);
        }
        
        [Test]
        public void GetAssignments_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _assignmentRepositoryMock.Setup(x => x.GetAll()).Returns(Assignments);

            var assignmentService = new AssignmentService(
             _assignmentRepositoryMock.Object,
             _userRepositoryMock.Object,
             _assetRepositoryMock.Object,
             _mapper
             );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await assignmentService.GetAllAssignments(parameters, 70);
                }
            );

            Assert.AreEqual("Unauthorized access", exception.Message);
        }

        [Test]
        public async Task CreateAssignments_WithValidParameters_ShouldBeSuccess()
        {
            var dontMatterUser1 = new User { };
            var dontMatterUser2 = new User { };
            var dontMatterAsset = new Asset { };
            var dontMatterAsssignment = new Assignment { };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser1);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterUser2);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(dontMatterAsset);
            _assignmentRepositoryMock.Setup(x => x.Create(dontMatterAsssignment)).ReturnsAsync(dontMatterAsssignment).Verifiable();
            _assetRepositoryMock.Setup(x => x.Update(dontMatterAsset)).Returns(Task.CompletedTask).Verifiable();

            var assignmentService = new AssignmentService(
            _assignmentRepositoryMock.Object,
            _userRepositoryMock.Object,
            _assetRepositoryMock.Object,
            _mapper
             );

            var assignmentModel = new AssignmentModel
            {
                AssetId = 1,
                AssignedToUserId = 2,
                AssignedDate = new DateTime(2022, 1, 3),
                Note = "abc"
            };

            //act
            await assignmentService.Create(It.IsAny<int>(),assignmentModel);

            //assert
            _assignmentRepositoryMock.Verify(x=> x.Create(dontMatterAsssignment), Times.Once());
        }

    }
}