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
    public class UserRepository_Tests
    {
        private AssetsManagementDBContext _context;
        private IAsyncUserRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AssetsManagementDBContext>()
                   .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;

            _context = new AssetsManagementDBContext(options);
            _repository = new UserRepository(_context);
        }

        [Test]
        public async Task Create_AddNewUser_WritesToDatabase()
        {

            //Arrange
            User user = new User()
            {
                StaffCode = "SD0003",
                FirstName = "Thang",
                LastName = "Doan Viet",
                DateOfBirth = new DateTime(1995, 06, 03),
                JoinedDate = new DateTime(2021, 12, 05),
                Gender = Gender.Male,
                Type = UserType.Admin,
                UserName = "thangdv",
                Password = "thangdv@30601995",
                Location = Location.HaNoi,
                Status = UserStatus.Active
            };

            //Act
            var _user = await _repository.Create(user);
            await _context.SaveChangesAsync();

            //Assert
            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreEqual(_user, user);

        }

        [TestCase(null)]
        [TestCase("")]
        public void CountUsername_NullUsernameInserted_ThrowExceptionMessage(string username)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => _repository.CountUsername(username));

            //Assert
            Assert.AreEqual(Message.NullOrEmptyUsername, result.ParamName);

        }

        [TestCase("thangdv")]
        public void CountUsername_ValidUsernameInserted_ReturnNumberOfGivenUsername(string username)
        {

            //Act
            var result = _repository.CountUsername(username);

            //Assert
            Assert.AreEqual(0, result);

        }

        [Test]
        public async Task Update_write_to_database()
        {

            //Arrange
            User user = new User
            {
                Id = 1,
                StaffCode = "SD0001",
                FirstName = "Nguyen Van",
                LastName = "Binh",
                DateOfBirth = new DateTime(01 / 20 / 1993),
                JoinedDate = new DateTime(12 / 05 / 2021),
                Gender = Gender.Male,
                Type = UserType.Admin,
                UserName = "binhnv",
                PasswordHash = "binhnv@20011993",
                Location = Location.HaNoi,
                Status = UserStatus.Active,
                NormalizedUserName = "Admin",
                Email = null,
                NormalizedEmail = null,
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.DateOfBirth = new DateTime(01 / 20 / 1999);
            user.JoinedDate = new DateTime(12 / 05 / 2020);
            user.Gender = Gender.Female;


            //Act
            await _repository.Update(user);
            await _context.SaveChangesAsync();
            var result = _context.Users.Count();

            //Assert

            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreEqual(new DateTime(01 / 20 / 1999), _context.Users.SingleOrDefault(x => x.Id == 1).DateOfBirth);
            Assert.AreEqual(new DateTime(12 / 05 / 2020), _context.Users.SingleOrDefault(x => x.Id == 1).JoinedDate);
            Assert.AreEqual(Gender.Female, _context.Users.SingleOrDefault(x => x.Id == 1).Gender);
        }

        [Test]
        public async Task Find_searches_byId()
        {
             //Arrange
            User user = new User
            {
                Id = 1,
                StaffCode = "SD0001",
                FirstName = "Nguyen Van",
                LastName = "Binh",
                DateOfBirth = new DateTime(01 / 20 / 1993),
                JoinedDate = new DateTime(12 / 05 / 2021),
                Gender = Gender.Male,
                Type = UserType.Admin,
                UserName = "binhnv",
                PasswordHash = "binhnv@20011993",
                Location = Location.HaNoi,
                Status = UserStatus.Active,
                NormalizedUserName = "Admin",
                Email = null,
                NormalizedEmail = null,
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //Act
            var userInfo = await _repository.GetById(1);
            var result = _context.Users.Count();
             //Assert
            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreEqual(1, userInfo.Id);
            Assert.AreEqual("SD0001", userInfo.StaffCode);
            Assert.AreEqual("Nguyen Van", userInfo.FirstName);
            Assert.AreEqual("Binh", userInfo.LastName);
            Assert.AreEqual(new DateTime(01 / 20 / 1993), userInfo.DateOfBirth);
            Assert.AreEqual(new DateTime(12 / 05 / 2021), userInfo.JoinedDate);
            Assert.AreEqual(Gender.Male, userInfo.Gender);
            Assert.AreEqual(UserType.Admin, userInfo.Type);
            Assert.AreEqual("binhnv", userInfo.UserName);
            Assert.AreEqual("binhnv@20011993", userInfo.PasswordHash);
            Assert.AreEqual(Location.HaNoi, userInfo.Location);
            Assert.AreEqual(UserStatus.Active, userInfo.Status);
            Assert.AreEqual("Admin", userInfo.NormalizedUserName);
        }

        [Test]
        public async Task GetAllUsers_Default_ShouldGetAllAvailableUsers()
        {
            //Arrange
            var users = new List<User>
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
                        Type = UserType.User,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            //Act
            var actualUsers = _repository.GetAll();

            //Assert
            Assert.That(actualUsers.Count() == 2);
            Assert.That(actualUsers.Any(u => u.StaffCode == "SD0001"));
            Assert.That(actualUsers.Any(u => u.StaffCode == "SD0002"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeletedAsync();
            _context.DisposeAsync();
        }
    }
}