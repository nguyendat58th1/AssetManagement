using System;
using BackEndAPI.DBContext;
using BackEndAPI.Helpers;
using BackEndAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BackEndAPI_Tests.Services
{
    [TestFixture]
    public class AutoGenerator_Tests
    {
        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssetRepository> _assetRepositoryMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IAsyncUserRepository>(behavior: MockBehavior.Strict);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AutoGeneratedStaffCode_ZeroOrNegativeNumberInserted_ThrowsExceptionMessage(int id)
        {

            //Act
            var result = Assert.Throws<InvalidOperationException>(() => AutoGenerator.AutoGeneratedStaffCode(id));

            //Assert
            Assert.AreEqual(Message.InvalidId, result.Message);

        }

        [TestCase(1)]
        public void AutoGeneratedStaffCode_PositiveNumberInserted_ReturnsStaffCode(int id)
        {

            //Act
            var result = AutoGenerator.AutoGeneratedStaffCode(id);

            //Assert
            Assert.AreEqual("SD0001", result);

        }

        [TestCase("           ", "Nguyen Van")]
        public void AutoGeneratedUsername_ContainingOnlySpaceFirstNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<Exception>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.EmptyOrSpacesFirstName, result.Message);

        }

        [TestCase(null, "Nguyen Van")]
        public void AutoGeneratedUsername_NullFirstNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.NullFirstName, result.ParamName);

        }

        [TestCase("", "Nguyen Van")]
        public void AutoGeneratedUsername_EmptyFirstNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<Exception>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.EmptyOrSpacesFirstName, result.Message);

        }

        [TestCase("Binh", "          ")]
        public void AutoGeneratedUsername_ContainingOnlySpaceLastNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<Exception>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.EmptyOrSpacesLastName, result.Message);

        }

        [TestCase("Binh", null)]
        public void AutoGeneratedUsername_NullLastNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.NullLastName, result.ParamName);

        }

        [TestCase("Binh", "")]
        public void AutoGeneratedUsername_EmptyLastNameInserted_ThrowsExceptionMessage(string firstName, string lastName)
        {

            //Act
            var result = Assert.Throws<Exception>(() =>
                                                                AutoGenerator.AutoGeneratedUsername(
                                                                firstName, lastName, _userRepositoryMock.Object));

            //Assert
            Assert.AreEqual(Message.EmptyOrSpacesLastName, result.Message);

        }

        [TestCase("Binh", "Nguyen Van")]
        public void AutoGeneratedUsername_ValidNameInsertedAndUsernameDidNotExist_ReturnsUsername(string firstName, string lastName)
        {

            //Arrange
            _userRepositoryMock.Setup(x => x.CountUsername("binhnv")).Returns(0);

            //Act
            var result = AutoGenerator.AutoGeneratedUsername(firstName, lastName, _userRepositoryMock.Object);

            //Assert
            Assert.AreEqual("binhnv", result);

        }

        [TestCase(" Binh ", "Nguyen Van")]
        [TestCase("Binh", " Nguyen Van ")]
        [TestCase("Binh", "Nguyen       Van")]
        public void AutoGeneratedUsername_NameWithMultipleSpaceInsertedAndUsernameDidNotExist_ReturnsUsername(string firstName, string lastName)
        {

            //Arrange
            _userRepositoryMock.Setup(x => x.CountUsername("binhnv")).Returns(0);

            //Act
            var result = AutoGenerator.AutoGeneratedUsername(firstName, lastName, _userRepositoryMock.Object);

            //Assert
            Assert.AreEqual("binhnv", result);

        }

        [TestCase("Binh", "Nguyen Van")]
        public void AutoGeneratedUsername_ValidNameInsertedAndUsernameDidExist_ReturnsUsernameWithExtraNumber(string firstName, string lastName)
        {

            //Arrange
            _userRepositoryMock.Setup(x => x.CountUsername("binhnv")).Returns(1);

            //Act
            var result = AutoGenerator.AutoGeneratedUsername(firstName, lastName, _userRepositoryMock.Object);

            //Assert
            Assert.AreEqual("binhnv1", result);

        }

        [TestCase(" Binh ", "Nguyen Van")]
        [TestCase("Binh", " Nguyen Van ")]
        [TestCase("Binh", "Nguyen       Van")]
        public void AutoGeneratedUsername_NameWithMultipleSpaceInsertedAndUsernameDidExist_ReturnsUsernameWithExtraNumber(string firstName, string lastName)
        {

            //Arrange
            _userRepositoryMock.Setup(x => x.CountUsername("binhnv")).Returns(1);

            //Act
            var result = AutoGenerator.AutoGeneratedUsername(firstName, lastName, _userRepositoryMock.Object);

            //Assert
            Assert.AreEqual("binhnv1", result);

        }

        [TestCase(null)]
        [TestCase("")]
        public void AutoGeneratedPassword_NullOrEmptyUsernameInserted_ThrowsExceptionMessage(string username)
        {

            //Arrange
            DateTime dob = new DateTime(1995, 03, 06);

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => AutoGenerator.AutoGeneratedPassword(username, dob));

            //Assert
            Assert.AreEqual(Message.NullOrEmptyUsername, result.ParamName);

        }

        [TestCase("thangdv")]
        public void AutoGeneratedPassword_ValidUsernameAndDateOfBirthInserted_ReturnsPassword(string username)
        {
            //Arrange
            DateTime dob = new DateTime(1995, 06, 03);

            //Act
            var result = AutoGenerator.AutoGeneratedPassword(username, dob);

            //Assert
            Assert.AreEqual("thangdv@03061995", result);

        }

        [TestCase(-1, "LA")]
        public void AutoGeneratedAssetCode_NegativeNumberInserted_ThrowsExceptionMessage(int assetNumber, string prefix)
        {

            //Act
            var result = Assert.Throws<InvalidOperationException>(() => AutoGenerator.AutoGeneratedAssetCode(assetNumber, prefix));

            //Assert
            Assert.AreEqual(Message.AssetNumberError, result.Message);

        }

        [TestCase(0, null)]
        [TestCase(1, "")]
        [TestCase(2, "             ")]
        public void AutoGeneratedAssetCode_NullOrEmptyPrefixInserted_ThrowsExceptionMessage(int assetNumber, string prefix)
        {

            //Act
            var result = Assert.Throws<ArgumentNullException>(() => AutoGenerator.AutoGeneratedAssetCode(assetNumber, prefix));

            //Assert
            Assert.AreEqual(Message.NullOrEmptyPrefix, result.ParamName);

        }

        [TestCase(0, "LA")]
        public void AutoGeneratedAssetCode_ValidNumberAndPrefixInserted_ReturnsAssetCode(int assetNumber, string prefix)
        {

            //Act
            var result = AutoGenerator.AutoGeneratedAssetCode(assetNumber, prefix);

            //Assert
            Assert.AreEqual("LA000001", result);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}