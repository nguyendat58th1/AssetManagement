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
    public class ReturnRequestRepository_Tests
    {
        private AssetsManagementDBContext _context;
        private IAsyncReturnRequestRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "ReturnRequestDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new ReturnRequestRepository(_context);

            _context.Users.AddRange(
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
                    Type = UserType.User,
                    UserName = "binhnt",
                    Password = "binhnt@12011994",
                    Location = Location.HaNoi,
                    Status = UserStatus.Active
                }
            );

            _context.AssetCategories.Add(
                new AssetCategory
                {
                    Id = 1,
                    CategoryCode = "LP",
                    CategoryName = "Laptop"
                }
            );

            _context.Assets.AddRange(
                new Asset
                {
                    Id = 1,
                    CategoryId = 1,
                    AssetCode = "LP0001",
                    AssetName = "Top of the line laptop",
                    Location = Location.HaNoi,
                    Specification = "Intel 11th-gen Core i7, 2TB SSD, 32GB RAM, RTX 3090 (24 GB)"
                },
                new Asset
                {
                    Id = 2,
                    CategoryId = 1,
                    AssetCode = "LP0002",
                    AssetName = "11-year-old hand-me-down laptop",
                    Location = Location.HaNoi,
                    Specification = "Random Intel Atom picked up on the side of the street"
                }
            );

            _context.Assignments.AddRange(
                new Assignment
                {
                    Id = 1,
                    AssetId = 1,
                    AssignedByUserId = 1,
                    AssignedToUserId = 2,
                    Note = "Should only be used for very serious work",
                    State = AssignmentState.Accepted
                },
                new Assignment
                {
                    Id = 2,
                    AssetId = 2,
                    AssignedByUserId = 1,
                    AssignedToUserId = 2,
                    Note = "Should only be used for modest work",
                    State = AssignmentState.Accepted
                }
            );

            _context.SaveChanges();
        }

        [Test]
        public async Task Create_AddNewReturnRequest_ShouldWriteToDatabase()
        {

            var newRequest = new ReturnRequest
            {
                Id = 3,
                AssignmentId = 2,
                AcceptedByUserId = 2,
                RequestedByUserId = 2,
                ReturnedDate = DateTime.UtcNow,
                State = RequestState.Completed
            };
            
            //Act
            var actualReturnRequest = await _repository.Create(newRequest);

            //Assert
            Assert.AreEqual(3, actualReturnRequest.Id);
            Assert.AreEqual(2, actualReturnRequest.AssignmentId);
            Assert.AreEqual(2, actualReturnRequest.AcceptedByUserId);
            Assert.AreEqual(2, actualReturnRequest.RequestedByUserId);
            Assert.IsNotNull(actualReturnRequest.ReturnedDate);
            Assert.That(actualReturnRequest.State == RequestState.Completed);
            
        }

        [Test]
        public async Task GetAll_Always_ReturnExpectedList()
        {
            //Arrange
            var requests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Id = 1,
                    AssignmentId = 1,
                    AcceptedByUserId = 2,
                    RequestedByUserId = 2,
                    ReturnedDate = DateTime.UtcNow,
                    State = RequestState.Completed
                },
                new ReturnRequest
                {
                    Id = 2,
                    AssignmentId = 2,
                    RequestedByUserId = 2,
                    State = RequestState.WaitingForReturning
                }
            };
            _context.ReturnRequest.AddRange(requests);
            await _context.SaveChangesAsync();

            //Act
            var actualReturnRequests = _repository.GetAll();

            //Assert
            Assert.AreEqual(2, actualReturnRequests.Count());
            Assert.That(actualReturnRequests.Any(rr => rr.Id == 1));
            Assert.That(actualReturnRequests.Any(rr => rr.Id == 2));
        }

        [Test]
        public async Task GetById_Default_ShouldGetTheRightElement()
        {
            //Arrange
            var requests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Id = 1,
                    AssignmentId = 1,
                    AcceptedByUserId = 2,
                    RequestedByUserId = 2,
                    ReturnedDate = DateTime.UtcNow,
                    State = RequestState.Completed
                },
                new ReturnRequest
                {
                    Id = 2,
                    AssignmentId = 2,
                    RequestedByUserId = 2,
                    State = RequestState.WaitingForReturning
                }
            };
            _context.ReturnRequest.AddRange(requests);
            await _context.SaveChangesAsync();

            //Act
            var request2 = await _repository.GetById(2);

            //Assert
            Assert.AreEqual(2, request2.Id);
            Assert.AreEqual(2, request2.AssignmentId);
            Assert.AreEqual(2, request2.RequestedByUserId);
            Assert.AreEqual(RequestState.WaitingForReturning, request2.State);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeletedAsync();
            _context.DisposeAsync();
        }
    }
}