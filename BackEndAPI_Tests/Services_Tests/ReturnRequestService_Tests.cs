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
    public class ReturnRequestService_Tests
    {

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
                        Type = UserType.User,
                        UserName = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User{ Id = 3, UserName = "abc001", Location = Location.HaNoi, Type = UserType.User}
                }
                .AsQueryable();
            }
        }

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
                        Asset = new Asset {
                            AssetName = "Laptop 1",
                            AssetCode = "LA000001"
                        },
                        AssignedByUserId = 1,
                        AssignedToUserId = 2,
                        Note = "Testing1",
                        AssignedDate = DateTime.Now.Date,
                        State = AssignmentState.Accepted
                    }
                }
                .AsQueryable();
            }
        }

        private static IQueryable<ReturnRequest> ReturnRequests
        {
            get
            {
                return new List<ReturnRequest>
                {
                    new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 2,
                        State = RequestState.WaitingForReturning
                    }
                }
                .AsQueryable();
            }
        }


        private Mock<IAsyncReturnRequestRepository> _returnRequestRepositoryMock;
        private Mock<IAsyncUserRepository> _userRepositoryMock;
        private Mock<IAsyncAssignmentRepository> _assignmentRepositoryMock;
        private Mock<IAsyncAssetRepository> _assetRepositoryMock;
        private readonly IMapper _mapper;

        public ReturnRequestService_Tests()
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
            _assignmentRepositoryMock = new Mock<IAsyncAssignmentRepository>(behavior: MockBehavior.Strict);
            _returnRequestRepositoryMock = new Mock<IAsyncReturnRequestRepository>(behavior: MockBehavior.Strict);
        }

        public async Task GetAll_WithDefaultValidPaginationParameters_ShouldReturnProperlyPagedListResponse()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(Users.Single(u => u.Id == 1));
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(ReturnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.GetAll(parameters, 1);

            //Assert
            var expectedCount = ReturnRequests.Count();
            Assert.AreEqual(expectedCount, returnRequestsPagedListResponse.TotalCount);
            Assert.AreEqual(1, returnRequestsPagedListResponse.CurrentPage);
            Assert.AreEqual(1, returnRequestsPagedListResponse.TotalPages);
            Assert.IsFalse(returnRequestsPagedListResponse.HasNext);
            Assert.IsFalse(returnRequestsPagedListResponse.HasPrevious);
        }

        [Test]
        public void GetAll_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
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
                    await service.GetAll(parameters, 70);
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Filter_NoFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_NoneProvided = new ReturnRequestFilterParameters
            { };
            var filterParams_OnlyRequestStateProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed
            };
            var filterParams_OnlyReturnDateProvided = new ReturnRequestFilterParameters
            {
                ReturnedDate = DateTime.Now
            };
            var filterParams_BothProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed,
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_NoneProvided
            );

            //Assert
            Assert.AreEqual(4, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_OnlyRequestStateFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_OnlyRequestStateProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_OnlyRequestStateProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_OnlyReturnedDateFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now.Date
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning,
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning,
                }
            }
            .AsQueryable();

            var filterParams_OnlyReturnDateProvided = new ReturnRequestFilterParameters
            {
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_OnlyReturnDateProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public async Task Filter_BothFilterParamsProvided_ShouldReturnCorrectFilteredResults()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.Completed,
                    ReturnedDate = DateTime.Now
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        AssignedByUserId = 1
                    },
                    State = RequestState.WaitingForReturning
                }
            }
            .AsQueryable();

            var filterParams_BothProvided = new ReturnRequestFilterParameters
            {
                RequestState = RequestState.Completed,
                ReturnedDate = DateTime.Now
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests);
            _assignmentRepositoryMock.Setup(x => x.GetCountUser(It.IsAny<int>())).Returns(1);
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var actualFilteredResponse = await service.Filter(
                parameters,
                It.IsAny<int>(),
                filterParams_BothProvided
            );

            //Assert
            Assert.AreEqual(2, actualFilteredResponse.TotalCount);
        }

        [Test]
        public void Search_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Search(
                        It.IsAny<PaginationParameters>(),
                        It.IsAny<int>(),
                        It.IsAny<string>()
                    );
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Search_ByUserName_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "bnt");

            //Assert
            Assert.AreEqual(2, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public async Task Search_ByAssetName_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                            AssignedByUserId = 1
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "printer");

            //Assert
            Assert.AreEqual(1, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public async Task Search_ByAssetCode_ShouldReturnCorrectReturnRequests()
        {
            //Arrange
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest
                    {
                        Id = 1,
                        AssignmentId = 1,
                        Assignment = new Assignment
                        {
                            Id = 1,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 02",
                                AssetCode = "LP0002"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing1",
                            State = AssignmentState.Accepted
                        },
                        AcceptedByUserId = 2,
                        RequestedByUserId = 2,
                        RequestedByUser = new User
                        {
                            UserName = "bnt01"
                        },
                        ReturnedDate = DateTime.UtcNow,
                        State = RequestState.Completed
                    },
                    new ReturnRequest
                    {
                        Id = 2,
                        AssignmentId = 2,
                        Assignment = new Assignment
                        {
                            Id = 2,
                            AssetId = 1,
                            Asset = new Asset
                            {
                                AssetName = "laptop 01",
                                AssetCode = "LP0001"
                            },
                            AssignedByUserId = 1,
                            AssignedToUserId = 2,
                            Note = "Testing2",
                            State = AssignmentState.WaitingForAcceptance
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "bnt02"
                        },
                        State = RequestState.WaitingForReturning
                    },
                    new ReturnRequest
                    {
                        Id = 3,
                        Assignment = new Assignment
                        {
                            Asset = new Asset
                            {
                                AssetName = "printer 01",
                                AssetCode = "PR0001"
                            },
                            AssignedByUserId = 1
                        },
                        RequestedByUserId = 3,
                        RequestedByUser = new User
                        {
                            UserName = "sql02"
                        },
                        State = RequestState.WaitingForReturning
                    }
            };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(returnRequests.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            PaginationParameters parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 30
            };

            //Act
            var returnRequestsPagedListResponse = await service.Search(
                parameters,
                1,
                "LP");

            //Assert
            Assert.AreEqual(2, returnRequestsPagedListResponse.TotalCount);
        }

        [Test]
        public void Filter_UserNotAdmin_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User { Type = UserType.User });
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Filter(
                        It.IsAny<PaginationParameters>(),
                        It.IsAny<int>(),
                        It.IsAny<ReturnRequestFilterParameters>()
                    );
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Create_Valid_ShouldBeSuccessful()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var userId = 1;
            var user = Users.Where(u => u.Id == userId).Single();
            var assignment = Assignments.Where(a => a.Id == inputModel.AssignmentId).Single();
            var simulatedReturnedRequest = new ReturnRequest
            {
                RequestedByUser = user,
                Assignment = assignment,
                ReturnedDate = null,
                State = RequestState.WaitingForReturning
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(Users);
            _assignmentRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(assignment);
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(simulatedReturnedRequest).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var result = await service.Create(inputModel, 1);

            //Assert
            Assert.AreEqual(simulatedReturnedRequest.RequestedByUser.UserName, result.RequestedByUser);
            Assert.AreEqual(simulatedReturnedRequest.ReturnedDate, result.ReturnedDate);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Once());
        }

        [Test]
        public void Create_NullInputModel_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<User>>());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(null, It.IsAny<int>());
                }
            );

            //Assert
            Assert.AreEqual(Message.NullInputModel, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_UserNotExists_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User> { };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, It.IsAny<int>());
                }
            );

            //Assert
            Assert.AreEqual(Message.UserNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_DisabledUser_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Status = UserStatus.Disabled
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Assignment>());
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_AssignmentNotFound_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(null as Assignment);
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.AssignmentNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_NormalUserTryToCreateReturnRequestForAssetsOfSomeoneElse_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Type = UserType.User,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Assignment
            {
                AssignedToUserId = 2,
                State = AssignmentState.Accepted
            });
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.TriedToCreateReturnRequestForSomeoneElseAssignment, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [Test]
        public void Create_AssetNotYetAccepted_ShouldThrowException()
        {
            //Arrange
            var inputModel = new CreateReturnRequestModel
            {
                AssignmentId = 1
            };
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Type = UserType.User,
                    Status = UserStatus.Active
                }
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            _assignmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Assignment
            {
                AssignedToUserId = 1,
                State = AssignmentState.WaitingForAcceptance
            });
            _returnRequestRepositoryMock.Setup(x => x.Create(It.IsAny<ReturnRequest>())).ReturnsAsync(It.IsAny<ReturnRequest>()).Verifiable();

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.Create(inputModel, 1);
                }
            );

            //Assert
            Assert.AreEqual(Message.AssignedAssetNotAccepted, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Create(It.IsAny<ReturnRequest>()), Times.Never());
        }


        [TestCase("LA000001", 1)]
        [TestCase("LA000002", 1)]
        public void GetAssociatedCount_Valid_ShouldReturnCorrectCount(string validAssetCode, int expectedCount)
        {
            //Arrange
            var rrList = new List<ReturnRequest>
            {
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        Asset = new Asset
                        {
                            AssetCode = "LA000001"
                        }
                    }
                },
                new ReturnRequest
                {
                    Assignment = new Assignment
                    {
                        Asset = new Asset
                        {
                            AssetCode = "LA000002"
                        }
                    }
                }
            };

            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(rrList.AsQueryable());
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var actualCount = service.GetAssociatedActiveCount(validAssetCode);

            //Assert
            Assert.That(actualCount == expectedCount);
        }

        [Test]
        public void GetAssociatedCount_NullAssetCode_ShouldReturnZero()
        {
            //Arrange            
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var actualCount = service.GetAssociatedActiveCount(null);

            //Assert
            Assert.That(actualCount == 0);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public void GetAssociatedCount_EmptyAssetCode_ShouldReturnZero()
        {
            //Arrange            
            _returnRequestRepositoryMock.Setup(x => x.GetAll()).Returns(It.IsAny<IQueryable<ReturnRequest>>()).Verifiable();
            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var actualCount = service.GetAssociatedActiveCount("");

            //Assert
            Assert.That(actualCount == 0);
            _returnRequestRepositoryMock.Verify(x => x.GetAll(), Times.Never());
        }

        [Test]
        public async Task Approve_Valid_ShouldBeSuccessful()
        {
            //Arrange
            var simulatedAssignment = new Assignment
            {
                AssetId = 1,
                State = AssignmentState.Accepted
            };

            var simulatedReturnRequest = new ReturnRequest
            {
                Id = 1,
                Assignment = simulatedAssignment
            };

            var simulatedAsset = new Asset
            {
                Id = 1,
                State = AssetState.Assigned
            };

            _returnRequestRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(simulatedReturnRequest);
            _assetRepositoryMock.Setup(x => x.GetById(simulatedReturnRequest.Assignment.AssetId)).ReturnsAsync(simulatedAsset);
            _userRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(new User { Id = 1, UserName = "abc", Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.Update(simulatedReturnRequest)).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(simulatedAsset)).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(simulatedReturnRequest.Assignment)).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            await service.Approve(1, 1);

            //Assert
            _assetRepositoryMock.VerifyAll();
            _assignmentRepositoryMock.VerifyAll();
            _returnRequestRepositoryMock.VerifyAll();
        }

        [Test]
        public void Approve_UserNotAdmin_ShouldThrowException()
        {
            //Arrange     
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ReturnRequest { });
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Asset>());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User {Type = UserType.User});
            _returnRequestRepositoryMock.Setup(x => x.Update(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Approve(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Update(It.IsAny<ReturnRequest>()), Times.Never());
            _assetRepositoryMock.Verify(x => x.Update(It.IsAny<Asset>()), Times.Never());
            _assignmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Assignment>()), Times.Never());
        }

        [Test]
        public void Approve_DisabledAdmin_ShouldThrowException()
        {
            //Arrange     
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ReturnRequest { });
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Asset>());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(
                new User {Type = UserType.Admin, Status = UserStatus.Disabled});
            _returnRequestRepositoryMock.Setup(x => x.Update(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Approve(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Update(It.IsAny<ReturnRequest>()), Times.Never());
            _assetRepositoryMock.Verify(x => x.Update(It.IsAny<Asset>()), Times.Never());
            _assignmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Assignment>()), Times.Never());
        }

        [Test]
        public void Approve_ReturnRequestNotExist_ShouldThrowException()
        {
            //Arrange     
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(null as ReturnRequest);
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Asset>());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            _returnRequestRepositoryMock.Setup(x => x.Update(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Approve(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.ReturnRequestNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Update(It.IsAny<ReturnRequest>()), Times.Never());
            _assetRepositoryMock.Verify(x => x.Update(It.IsAny<Asset>()), Times.Never());
            _assignmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Assignment>()), Times.Never());
        }

        [Test]
        public async Task Deny_Valid_ShouldBeSuccessful()
        {
            //Arrange
            var simulatedAssignment = new Assignment
            {
                AssetId = 1,
                State = AssignmentState.Accepted
            };

            var simulatedReturnRequest = new ReturnRequest
            {
                Id = 1,
                Assignment = simulatedAssignment
            };

            var simulatedAsset = new Asset
            {
                Id = 1,
                State = AssetState.Assigned
            };

            _userRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(new User { Id = 1, UserName = "abc", Type = UserType.Admin });
            _returnRequestRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(simulatedReturnRequest);
            _returnRequestRepositoryMock.Setup(x => x.Delete(simulatedReturnRequest)).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            await service.Deny(1, 1);

            //Assert
            _assetRepositoryMock.VerifyAll();
            _assignmentRepositoryMock.VerifyAll();
            _returnRequestRepositoryMock.VerifyAll();
        }

        [Test]
        public void Deny_UserNotAdmin_ShouldThrowException()
        {
            //Arrange     
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ReturnRequest { });
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Asset>());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new User {Type = UserType.User});
            _returnRequestRepositoryMock.Setup(x => x.Update(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Deny(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Update(It.IsAny<ReturnRequest>()), Times.Never());
            _assetRepositoryMock.Verify(x => x.Update(It.IsAny<Asset>()), Times.Never());
            _assignmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Assignment>()), Times.Never());
        }

        [Test]
        public void Deny_DisabledAdmin_ShouldThrowException()
        {
            //Arrange     
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ReturnRequest { });
            _assetRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<Asset>());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(
                new User {Type = UserType.Admin, Status = UserStatus.Disabled});
            _returnRequestRepositoryMock.Setup(x => x.Update(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);
            _assetRepositoryMock.Setup(x => x.Update(It.IsAny<Asset>())).Returns(Task.CompletedTask);
            _assignmentRepositoryMock.Setup(x => x.Delete(It.IsAny<Assignment>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Deny(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.UnauthorizedUser, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Update(It.IsAny<ReturnRequest>()), Times.Never());
            _assetRepositoryMock.Verify(x => x.Update(It.IsAny<Asset>()), Times.Never());
            _assignmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Assignment>()), Times.Never());
        }

        [Test]
        public void Deny_ReturnRequestNotExist_ShouldThrowException()
        {
            //Arrange
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            _returnRequestRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(null as ReturnRequest);
            _returnRequestRepositoryMock.Setup(x => x.Delete(It.IsAny<ReturnRequest>())).Returns(Task.CompletedTask);

            var service = new ReturnRequestService(
                _userRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _returnRequestRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _mapper
            );

            //Act
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await service.Deny(2, 1)
            );

            //Assert
            Assert.AreEqual(Message.ReturnRequestNotFound, exception.Message);
            _returnRequestRepositoryMock.Verify(x => x.Delete(It.IsAny<ReturnRequest>()), Times.Never());
        }

        [TearDown]
        public void TearDown()
        {

        }

    }
}