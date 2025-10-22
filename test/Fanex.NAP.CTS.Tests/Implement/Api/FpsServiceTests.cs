namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.FPS.Account;
    using Fanex.NAP.CTS.Criteria.Api.FPS.Device;
    using Fanex.NAP.CTS.Criteria.Api.FPS.Search;
    using Fanex.NAP.CTS.Criteria.Api.FPS.SearchAccount;
    using Fanex.NAP.CTS.Criteria.Api.FPS.Transaction;
    using Fanex.NAP.CTS.Model.API.FPS.Account;
    using Fanex.NAP.CTS.Model.API.FPS.Device;
    using Fanex.NAP.CTS.Model.API.FPS.Search;
    using Fanex.NAP.CTS.Model.API.FPS.SearchAccount;
    using Fanex.NAP.CTS.Model.API.FPS.Transaction;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class FpsServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly FpsService _service;

        public FpsServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new FpsService(_mockRepository.Object);

            LogExtension.ConfigByPassForUnitTest();
        }

        #region DCS Center Tests

        [Fact]
        public void SearchAccount_DCSCenter_WithPartialSearch_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchAccountParamModel
            {
                SubscriberID = 12345,
                UserName = "testuser",
                IsSearchPartial = true,
                Take = 10,
                Skip = 0
            };

            var expectedItems = new List<SearchAccountItemModel>
            {
                new SearchAccountItemModel
                {
                    CTSCustID = 1,
                    Username = "testuser",
                    RegisterName = "Test User",
                    SubscriberID = 12345
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchAccountItemModel>(It.IsAny<GetListAccountByUserNameSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.SearchAccount_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(expectedItems.Count, result.Items.Count());
            _mockRepository.Verify(x => x.Fetch<SearchAccountItemModel>(It.IsAny<GetListAccountByUserNameSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void SearchAccount_DCSCenter_WithExactSearch_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchAccountParamModel
            {
                SubscriberID = 12345,
                UserName = "testuser",
                IsSearchPartial = false
            };

            var expectedItems = new List<SearchAccountItemModel>
            {
                new SearchAccountItemModel
                {
                    CTSCustID = 1,
                    Username = "testuser",
                    RegisterName = "Test User",
                    SubscriberID = 12345
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchAccountItemModel>(It.IsAny<GetAccountByNameSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.SearchAccount_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(expectedItems.Count, result.Items.Count());
            _mockRepository.Verify(x => x.Fetch<SearchAccountItemModel>(It.IsAny<GetAccountByNameSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssAccountInfoByCustSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccountInfoByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456
            };
            var expectedResult = new GetAssAccountInfoByCustSubscriberModel
            {
                AssociatedDevices = 2,
                AssociatedAccounts = 3,
                FirstLogin = DateTime.Now,
                FirstDevice = "DeviceA"
            };

            _mockRepository.Setup(x => x.Get<GetAssAccountInfoByCustSubscriberModel>(It.IsAny<GetAssAccountInfoByCustSubscriberDCSCenterCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssAccountInfoByCustSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.AssociatedDevices, result.AssociatedDevices);
            Assert.Equal(expectedResult.AssociatedAccounts, result.AssociatedAccounts);
            _mockRepository.Verify(x => x.Get<GetAssAccountInfoByCustSubscriberModel>(It.IsAny<GetAssAccountInfoByCustSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssAccountsByCustSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccountsByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456,
                NoOfRecord = 10
            };
            var expectedItems = new List<GetAssAccountsByCustSubscriberItemModel>
            {
                new GetAssAccountsByCustSubscriberItemModel
                {
                    CTSCustID = 456,
                    UserName = "testuser",
                    RegisterName = "Test User",
                    DeviceInCommon = 2,
                    OtherDeives = 1,
                    AssociatedAccount = 5
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssAccountsByCustSubscriberItemModel>(It.IsAny<GetAssAccountsByCustSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssAccountsByCustSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<GetAssAccountsByCustSubscriberItemModel>(It.IsAny<GetAssAccountsByCustSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDevicesByCustSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDevicesByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456,
                NoOfRecord = 10
            };
            var expectedItems = new List<GetAssDevicesByCustSubscriberItemModel>
            {
                new GetAssDevicesByCustSubscriberItemModel
                {
                    DeviceID = 1,
                    DeviceCode = "DEV001",
                    FirstSeenDate = DateTime.Now,
                    AssociatedAccounts = 3,
                    AssociatedDevices = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDevicesByCustSubscriberItemModel>(It.IsAny<GetAssDevicesByCustSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssDevicesByCustSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedItems[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedItems[0].AssociatedAccounts, item.AssociatedAccounts);
            Assert.Equal(expectedItems[0].AssociatedDevices, item.AssociatedDevices);
            _mockRepository.Verify(x => x.Fetch<GetAssDevicesByCustSubscriberItemModel>(It.IsAny<GetAssDevicesByCustSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssMatrixAccountByCustSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssMatrixAccountByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456,
                NoOfCustomer = 5,
                NoOfDevice = 3
            };
            var expectedItems = new List<GetAssMatrixAccountByCustSubscriberItemModel>
            {
                new GetAssMatrixAccountByCustSubscriberItemModel
                {
                    ID = 1,
                    CTSCustID = 456,
                    UserName = "user1",
                    DeviceID = 1,
                    FirstDeviceCode = "DEV001",
                    CreatedTime = DateTime.Now
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssMatrixAccountByCustSubscriberItemModel>(It.IsAny<GetAssMatrixAccountByCustSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssMatrixAccountByCustSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<GetAssMatrixAccountByCustSubscriberItemModel>(It.IsAny<GetAssMatrixAccountByCustSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistory_DCSCenter_WithCustID_CallsByCustSubscriber()
        {
            // Arrange
            var param = new TransactionParamModel
            {
                SubscriberID = 12345,
                CTSCustID = 67890,
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionItemModel>
            {
                new TransactionItemModel
                {
                    TransID = 1,
                    CTSCustID = 67890,
                    SubscriberID = 12345,
                    Action = "Login"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionItemModel>(It.IsAny<GetTransactionHistoryByCustSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetTransactionHistory_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<TransactionItemModel>(It.IsAny<GetTransactionHistoryByCustSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistory_DCSCenter_WithoutCustID_CallsBySubscriber()
        {
            // Arrange
            var param = new TransactionParamModel
            {
                SubscriberID = 12345,
                CTSCustID = null,
                UserName = "testuser",
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                IsPartial = true,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionItemModel>
            {
                new TransactionItemModel
                {
                    TransID = 1,
                    SubscriberID = 12345,
                    UserName = "testuser",
                    Action = "Login"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionItemModel>(It.IsAny<GetTransactionHistoryBySubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetTransactionHistory_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<TransactionItemModel>(It.IsAny<GetTransactionHistoryBySubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetLastestTransactionBySubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new LatestTransactionParamModel
            {
                SubscriberID = 12345,
                Length = 100,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<LatestTransactionDCSCenterItemModel>
            {
                new LatestTransactionDCSCenterItemModel
                {
                    TransID = 1,
                    SubscriberID = 12345,
                    CTSCustID = 67890,
                    UserName = "testuser",
                    RegisterName = "Test User",
                    Action = "Login",
                    ActionResult = "Success",
                    TransTime = DateTime.Now,
                    FirstDeviceCode = "DEV001",
                    IP = "192.168.1.1",
                    OS = "Windows",
                    Browser = "Chrome",
                    URLDetails = "https://example.com",
                    RobotTracking = "No"
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestTransactionDCSCenterItemModel>(It.IsAny<GetLastestTransactionDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetLastestTransactionBySubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].TransID, item.TransID);
            Assert.Equal(expectedItems[0].UserName, item.UserName);
            Assert.Equal(expectedItems[0].Action, item.Action);
            _mockRepository.Verify(x => x.Fetch<LatestTransactionDCSCenterItemModel>(It.IsAny<GetLastestTransactionDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAccountsPerDevice_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAccountPerDeviceParamModel
            {
                SubscriberId = 123,
                TimeRange = 30,
                NoOfAccount = 10,
                PageSize = 20,
                PageIndex = 1
            };

            var expectedItems = new List<GetAccountsPerDeviceItemModel>
            {
                new GetAccountsPerDeviceItemModel
                {
                    DeviceId = 1UL,
                    DeviceNo = "DEV001",
                    NoOfAccount = 5
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAccountsPerDeviceItemModel>(It.IsAny<GetAccountsPerDeviceDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAccountsPerDevice_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].DeviceId, item.DeviceId);
            Assert.Equal(expectedItems[0].DeviceNo, item.DeviceNo);
            Assert.Equal(expectedItems[0].NoOfAccount, item.NoOfAccount);
            _mockRepository.Verify(x => x.Fetch<GetAccountsPerDeviceItemModel>(It.IsAny<GetAccountsPerDeviceDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetDevicesPerAccount_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetDevicesPerAccountParamModel
            {
                SubscriberId = 123,
                TimeRange = 30,
                NoOfDevice = 10,
                PageSize = 20,
                PageIndex = 1
            };

            var expectedItems = new List<GetDevicesPerAccountItemModel>
            {
                new GetDevicesPerAccountItemModel
                {
                    CTSCustID = 456,
                    UserName = "testuser",
                    NoOfDevice = 3
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetDevicesPerAccountItemModel>(It.IsAny<GetDevicesPerAccountDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetDevicesPerAccount_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<GetDevicesPerAccountItemModel>(It.IsAny<GetDevicesPerAccountDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void SearchDevice_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchDeviceParamModel
            {
                DeviceCode = "DEV001",
                SubscriberID = 12345
            };

            var expectedResult = new List<SearchDeviceItemModel>
            {
                new SearchDeviceItemModel
                {
                    DeviceID = 1L,
                    DeviceCode = "DEV001",
                    OS = "Windows"
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchDeviceItemModel>(It.IsAny<GetDeviceByDeviceCodeSubscriberDCSCenterCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.SearchDevice_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var item = result.First();
            Assert.Equal(expectedResult[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedResult[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedResult[0].OS, item.OS);
            _mockRepository.Verify(x => x.Fetch<SearchDeviceItemModel>(It.IsAny<GetDeviceByDeviceCodeSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDeviceInfoByIDSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDeviceInfoByIDSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456
            };

            var expectedFirstResult = new List<GetAssDeviceInfoByIDSubscriberModel>
            {
                new GetAssDeviceInfoByIDSubscriberModel
                {
                    AssociatedAccounts = 5
                }
            };

            var expectedSecondResult = new List<GetAssDeviceFirstSeen>
            {
                new GetAssDeviceFirstSeen
                {
                    SubscriberID = 456,
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDeviceInfoByIDSubscriberModel, GetAssDeviceFirstSeen>(It.IsAny<GetAssDeviceInfoByIDSubscriberDCSCenterCriteria>()))
                .Returns((expectedFirstResult, expectedSecondResult));

            // Act
            var result = _service.GetAssDeviceInfoByIDSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedFirstResult[0].AssociatedAccounts, result.AssociatedAccounts);
            Assert.Equal(expectedSecondResult, result.FirstSeenBySubscriber);
            _mockRepository.Verify(x => x.Fetch<GetAssDeviceInfoByIDSubscriberModel, GetAssDeviceFirstSeen>(It.IsAny<GetAssDeviceInfoByIDSubscriberDCSCenterCriteria>()), Times.Once);
        }


        [Fact]
        public void GetAssAccByDevSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccByDevSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456
            };

            var expectedItems = new List<GetAssAccByDevSubscriberItemDCSCenterModel>
                {
                    new GetAssAccByDevSubscriberItemDCSCenterModel
                    {
                        AccountID = 789,
                        RegisterName = "testuser",
                        UserName = "testuser",
                        DeviceInCommon = 2,
                        OtherDeives = 1,
                        AssociatedAccount = 5
                    }
                };

            _mockRepository.Setup(x => x.Fetch<GetAssAccByDevSubscriberItemDCSCenterModel>(It.IsAny<GetAssAccByDevSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssAccByDevSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Single(result);

            var item = result.First();

            Assert.Equal(expectedItems[0].AccountID, item.AccountID);
            Assert.Equal(expectedItems[0].RegisterName, item.RegisterName);
            Assert.Equal(expectedItems[0].UserName, item.UserName);
            Assert.Equal(expectedItems[0].DeviceInCommon, item.DeviceInCommon);
            Assert.Equal(expectedItems[0].OtherDeives, item.OtherDeives);
            Assert.Equal(expectedItems[0].AssociatedAccount, item.AssociatedAccount);
            _mockRepository.Verify(x => x.Fetch<GetAssAccByDevSubscriberItemDCSCenterModel>(It.IsAny<GetAssAccByDevSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDevByDevSubscriber_DCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDevByDevSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                NoOfRecord = 10
            };

            var expectedItems = new List<GetAssDevByDevSubscriberItemModel>
            {
                new GetAssDevByDevSubscriberItemModel
                {
                    DeviceID = 1L,
                    DeviceCode = "DEV001",
                    FirstSeenDate = DateTime.Now,
                    AssociatedAccounts = 3,
                    AssociatedDevices = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDevByDevSubscriberItemModel>(It.IsAny<GetAssDevByDevSubscriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssDevByDevSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Single(result);

            var item = result.First();

            Assert.Equal(expectedItems[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedItems[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedItems[0].FirstSeenDate, item.FirstSeenDate);
            Assert.Equal(expectedItems[0].AssociatedAccounts, item.AssociatedAccounts);
            Assert.Equal(expectedItems[0].AssociatedDevices, item.AssociatedDevices);
            _mockRepository.Verify(x => x.Fetch<GetAssDevByDevSubscriberItemModel>(It.IsAny<GetAssDevByDevSubscriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssMatrixByDeviceSubscriberDCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssMatrixByDeviceSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                NoOfCustomer = 10,
                NoOfDevice = 5
            };

            var expectedItems = new List<GetAssMatrixByDeviceSubscriberItemDataCenterModel>
            {
                new GetAssMatrixByDeviceSubscriberItemDataCenterModel
                {
                    AccountID = 789,
                    RegisterName = "testuser",
                    UserName = "testuser",
                    DeviceID = 123,
                    FirstDeviceCode = "DEV001",
                    CreatedTime = DateTime.Now
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssMatrixByDeviceSubscriberItemDataCenterModel>(It.IsAny<GetAssMatrixByDeviceSubsriberDCSCenterCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssMatrixByDeviceSubscriber_DCSCenter(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Single(result);

            var item = result.First();

            Assert.Equal(expectedItems[0].AccountID, item.AccountID);
            Assert.Equal(expectedItems[0].RegisterName, item.RegisterName);
            Assert.Equal(expectedItems[0].UserName, item.UserName);
            Assert.Equal(expectedItems[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedItems[0].FirstDeviceCode, item.FirstDeviceCode);

            _mockRepository.Verify(x => x.Fetch<GetAssMatrixByDeviceSubscriberItemDataCenterModel>(It.IsAny<GetAssMatrixByDeviceSubsriberDCSCenterCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistoryByDeviceDCSCenter_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new TransactionHistoryByDeviceParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionHistoryByDeviceDCSCenterItemModel>
            {
                new TransactionHistoryByDeviceDCSCenterItemModel
                {
                    TransID = 1,
                    TransTime = DateTime.Now,
                    RegisterName = "Test Register",
                    UserName = "testuser",
                    AccountID = 789,
                    Action = "Login",
                    ActionResult = "Success",
                    OS = "Windows",
                    Browser = "Chrome",
                    URLDetails = "https://example.com",
                    IP = "8.8.8.8",
                    RobotTracking = "No"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionHistoryByDeviceDCSCenterItemModel>(It.IsAny<GetTransHistoryByDevSubDCSCenterCriteria>()))
                .Returns(expectedItems);
            // Act

            var result = _service.GetTransactionHistoryByDeviceDCSCenter(param);
            // Assert

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);

            var item = result.Items.First();

            Assert.Equal(expectedItems[0].TransID, item.TransID);
            Assert.Equal(expectedItems[0].TransTime, item.TransTime);
            Assert.Equal(expectedItems[0].RegisterName, item.RegisterName);
            Assert.Equal(expectedItems[0].UserName, item.UserName);
            Assert.Equal(expectedItems[0].AccountID, item.AccountID);
            Assert.Equal(expectedItems[0].Action, item.Action);
            Assert.Equal(expectedItems[0].ActionResult, item.ActionResult);
            Assert.Equal(expectedItems[0].OS, item.OS);
            Assert.Equal(expectedItems[0].Browser, item.Browser);
            Assert.Equal(expectedItems[0].URLDetails, item.URLDetails);
            Assert.Equal(expectedItems[0].IP, item.IP);
            Assert.Equal(expectedItems[0].RobotTracking, item.RobotTracking);

            _mockRepository.Verify(x => x.Fetch<TransactionHistoryByDeviceDCSCenterItemModel>(It.IsAny<GetTransHistoryByDevSubDCSCenterCriteria>()), Times.Once);
        }

        #endregion

        #region DCS Extra Tests

        [Fact]
        public void SearchAccount_DCSExtra_WithPartialSearch_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchAccountParamModel
            {
                SubscriberID = 12345,
                UserName = "testuser",
                IsSearchPartial = true,
                Take = 10,
                Skip = 0
            };

            var expectedItems = new List<SearchAccountItemDCSExtraModel>
            {
                new SearchAccountItemDCSExtraModel
                {
                    AccountID = 1,
                    LoginName = "testuser",
                    SubscriberID = 12345
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchAccountItemDCSExtraModel>(It.IsAny<GetListAccountByUserNameSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.SearchAccount_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].AccountID, item.CTSCustID);
            Assert.Equal(expectedItems[0].LoginName, item.Username);
            _mockRepository.Verify(x => x.Fetch<SearchAccountItemDCSExtraModel>(It.IsAny<GetListAccountByUserNameSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void SearchAccount_DCSExtra_WithExactSearch_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchAccountParamModel
            {
                SubscriberID = 12345,
                UserName = "testuser",
                IsSearchPartial = false
            };

            var expectedItems = new List<SearchAccountItemDCSExtraModel>
            {
                new SearchAccountItemDCSExtraModel
                {
                    AccountID = 1,
                    LoginName = "testuser",
                    SubscriberID = 12345
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchAccountItemDCSExtraModel>(It.IsAny<GetAccountByNameSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.SearchAccount_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            _mockRepository.Verify(x => x.Fetch<SearchAccountItemDCSExtraModel>(It.IsAny<GetAccountByNameSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void SearchDevice_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new SearchDeviceParamModel
            {
                SubscriberID = 12345,
                DeviceCode = "DEV001"
            };

            var expectedResult = new List<SearchDeviceItemDCSExtraModel>
            {
                new SearchDeviceItemDCSExtraModel
                {
                    DeviceID = 1L,
                    DeviceCode = "DEV001",
                    OS = "Windows"
                }
            };

            _mockRepository.Setup(x => x.Fetch<SearchDeviceItemDCSExtraModel>(It.IsAny<GetListDeviceByCodeSubscriberDCSExtraCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.SearchDevice_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var item = result.First();
            Assert.Equal(expectedResult[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedResult[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedResult[0].OS, item.OS);
            _mockRepository.Verify(x => x.Fetch<SearchDeviceItemDCSExtraModel>(It.IsAny<GetListDeviceByCodeSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssAccountInfoByCustSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccountInfoByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456
            };
            var expectedResult = new GetAssAccountInfoByCustSubscriberModel
            {
                AssociatedDevices = 2,
                AssociatedAccounts = 3,
                FirstLogin = DateTime.Now,
                FirstDevice = "DeviceA"
            };

            _mockRepository.Setup(x => x.Get<GetAssAccountInfoByCustSubscriberModel>(It.IsAny<GetAssAccountInfoByCustSubscriberDCSExtraCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssAccountInfoByCustSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.AssociatedDevices, result.AssociatedDevices);
            Assert.Equal(expectedResult.AssociatedAccounts, result.AssociatedAccounts);
            _mockRepository.Verify(x => x.Get<GetAssAccountInfoByCustSubscriberModel>(It.IsAny<GetAssAccountInfoByCustSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssAccountsByCustSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccountsByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456,
                NoOfRecord = 10
            };
            var expectedItems = new List<GetAssAccountsByCustSubscriberItemDcsModel>
            {
                new GetAssAccountsByCustSubscriberItemDcsModel
                {
                    AccountID = 456L,
                    LoginName = "user1",
                    DeviceInCommon = 2,
                    OtherDeives = 1,
                    AssociatedAccount = 5
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssAccountsByCustSubscriberItemDcsModel>(It.IsAny<GetAssAccountsByCustSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssAccountsByCustSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].AccountID, item.CTSCustID);
            Assert.Equal(expectedItems[0].LoginName, item.UserName);
            _mockRepository.Verify(x => x.Fetch<GetAssAccountsByCustSubscriberItemDcsModel>(It.IsAny<GetAssAccountsByCustSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDevicesByCustSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDevicesByCustSubscriberParamModel
            {
                SubscriberID = 123,
                CTSCustID = 456,
                NoOfRecord = 10
            };
            var expectedItems = new List<GetAssDevicesByCustSubscriberItemModel>
            {
                new GetAssDevicesByCustSubscriberItemModel
                {
                    DeviceID = 1,
                    DeviceCode = "DEV001",
                    FirstSeenDate = DateTime.Now,
                    AssociatedAccounts = 3,
                    AssociatedDevices = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDevicesByCustSubscriberItemModel>(It.IsAny<GetAssDevicesByCustSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssDevicesByCustSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedItems[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedItems[0].AssociatedAccounts, item.AssociatedAccounts);
            Assert.Equal(expectedItems[0].AssociatedDevices, item.AssociatedDevices);
            _mockRepository.Verify(x => x.Fetch<GetAssDevicesByCustSubscriberItemModel>(It.IsAny<GetAssDevicesByCustSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssMatrixAccountByCustSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssMatrixAccountByCustSubscriberParamModel
            {
                SubscriberID = 456,
                CTSCustID = 789,
                NoOfCustomer = 10,
                NoOfDevice = 5
            };

            var expectedItems = new List<GetAssMatrixAccountByCustSubscriberItemDCSExtraModel>
            {
                new GetAssMatrixAccountByCustSubscriberItemDCSExtraModel
                {
                    ID = 1,
                    AccountID = 789L,
                    LoginName = "testuser",
                    DeviceID = 1,
                    FirstDeviceCode = "DEV001",
                    CreatedTime = DateTime.Now
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssMatrixAccountByCustSubscriberItemDCSExtraModel>(It.IsAny<GetAssMatrixAccountByCustSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetAssMatrixAccountByCustSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].AccountID, item.CTSCustID);
            Assert.Equal(expectedItems[0].LoginName, item.UserName);
            _mockRepository.Verify(x => x.Fetch<GetAssMatrixAccountByCustSubscriberItemDCSExtraModel>(It.IsAny<GetAssMatrixAccountByCustSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDeviceInfoByIDSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDeviceInfoByIDSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456
            };

            var expectedFirstResult = new List<GetAssDeviceInfoByIDSubscriberModel>
            {
                new GetAssDeviceInfoByIDSubscriberModel
                {
                    AssociatedAccounts = 5
                }
            };

            var expectedSecondResult = new List<GetAssDeviceFirstSeen>
            {
                new GetAssDeviceFirstSeen
                {
                    SubscriberID = 456,
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDeviceInfoByIDSubscriberModel, GetAssDeviceFirstSeen>(It.IsAny<GetAssDeviceInfoByIDSubscriberDCSExtraCriteria>()))
                .Returns((expectedFirstResult, expectedSecondResult));

            // Act
            var result = _service.GetAssDeviceInfoByIDSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedFirstResult[0].AssociatedAccounts, result.AssociatedAccounts);
            Assert.Equal(expectedSecondResult, result.FirstSeenBySubscriber);
            _mockRepository.Verify(x => x.Fetch<GetAssDeviceInfoByIDSubscriberModel, GetAssDeviceFirstSeen>(It.IsAny<GetAssDeviceInfoByIDSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssAccByDevSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssAccByDevSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                NoOfRecord = 10
            };

            var expectedResult = new List<GetAssAccByDevSubscriberItemModel>
            {
                new GetAssAccByDevSubscriberItemModel
                {
                    AccountID = 1,
                    LoginName = "testuser",
                    DeviceInCommon = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssAccByDevSubscriberItemModel>(It.IsAny<GetAssAccByDevSubscriberDCSExtraCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssAccByDevSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult[0].AccountID, result.First().AccountID);
            _mockRepository.Verify(x => x.Fetch<GetAssAccByDevSubscriberItemModel>(It.IsAny<GetAssAccByDevSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssDevByDevSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssDevByDevSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                NoOfRecord = 10
            };

            var expectedResult = new List<GetAssDevByDevSubscriberItemModel>
            {
                new GetAssDevByDevSubscriberItemModel
                {
                    DeviceID = 1L,
                    DeviceCode = "DEV001",
                    FirstSeenDate = DateTime.Now,
                    AssociatedAccounts = 3,
                    AssociatedDevices = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssDevByDevSubscriberItemModel>(It.IsAny<GetAssDevByDevSubscriberDCSExtraCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssDevByDevSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var item = result.First();
            Assert.Equal(expectedResult[0].DeviceID, item.DeviceID);
            Assert.Equal(expectedResult[0].DeviceCode, item.DeviceCode);
            Assert.Equal(expectedResult[0].AssociatedAccounts, item.AssociatedAccounts);
            Assert.Equal(expectedResult[0].AssociatedDevices, item.AssociatedDevices);
            _mockRepository.Verify(x => x.Fetch<GetAssDevByDevSubscriberItemModel>(It.IsAny<GetAssDevByDevSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetAssMatrixByDeviceSubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new GetAssMatrixByDeviceSubscriberParamModel
            {
                DeviceID = 123,
                SubscriberID = 456,
                NoOfCustomer = 5,
                NoOfDevice = 3
            };

            var expectedResult = new List<GetAssMatrixByDeviceSubscriberItemModel>
            {
                new GetAssMatrixByDeviceSubscriberItemModel
                {
                    FirstDeviceCode = "1",
                    DeviceID = 123,
                    AccountID = 456
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetAssMatrixByDeviceSubscriberItemModel>(It.IsAny<GetAssMatrixByDeviceSubsriberDCSExtraCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssMatrixByDeviceSubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult[0].FirstDeviceCode, result.First().FirstDeviceCode);
            _mockRepository.Verify(x => x.Fetch<GetAssMatrixByDeviceSubscriberItemModel>(It.IsAny<GetAssMatrixByDeviceSubsriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistoryByDevice_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new TransactionHistoryByDeviceParamModel
            {
                DeviceID = 12345,
                SubscriberID = 1,
                FromDate = DateTime.UtcNow.AddDays(-1),
                ToDate = DateTime.UtcNow,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionHistoryByDeviceDCSExtraItemModel>
            {
                new TransactionHistoryByDeviceDCSExtraItemModel
                {
                    TransID = 1,
                    TransTime = DateTime.UtcNow,
                    LoginName = "testUser",
                    AccountID = 12345,
                    Action = "Login",
                    ActionResult = "Success"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionHistoryByDeviceDCSExtraItemModel>(It.IsAny<GetTransHistoryByDevSubDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetTransactionHistoryByDevice_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].TransID, item.TransID);
            Assert.Equal(expectedItems[0].LoginName, item.LoginName);
            Assert.Equal(expectedItems[0].Action, item.Action);
            _mockRepository.Verify(x => x.Fetch<TransactionHistoryByDeviceDCSExtraItemModel>(It.IsAny<GetTransHistoryByDevSubDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistoryByAccount_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new TransactionHistoryByAccountParamModel
            {
                SubscriberID = 12345,
                CTSCustID = 67890,
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionHistoryByAccountDCSExtraItemModel>
            {
                new TransactionHistoryByAccountDCSExtraItemModel
                {
                    TransID = 1,
                    CTSCustID = 67890,
                    SubscriberID = 12345,
                    UserName = "testuser",
                    RegisterName = "Test User",
                    FirstDeviceCode = "DEV001",
                    TransTime = DateTime.Now,
                    Action = "Login",
                    ActionResult = "Success",
                    OS = "Windows",
                    Browser = "Chrome",
                    IP = "192.168.1.1",
                    Country = "US",
                    Region = "CA",
                    City = "San Francisco",
                    ISP = "Test ISP"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionHistoryByAccountDCSExtraItemModel>(It.IsAny<GetTransactionHistoryByCustSubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetTransactionHistoryByAccount_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].TransID, item.TransID);
            Assert.Equal(expectedItems[0].CTSCustID, item.CTSCustID);
            _mockRepository.Verify(x => x.Fetch<TransactionHistoryByAccountDCSExtraItemModel>(It.IsAny<GetTransactionHistoryByCustSubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetTransactionHistory_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new TransactionParamModel
            {
                SubscriberID = 12345,
                UserName = "testuser",
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                City = "New York",
                Country = "US",
                DeviceCode = "DEV001",
                IP = "192.168.1.1",
                Region = "NY",
                IsPartial = true,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<TransactionDCSExtraItemModel>
            {
                new TransactionDCSExtraItemModel
                {
                    TransID = 1,
                    SubscriberID = 12345,
                    LoginName = "testuser",
                    Action = "Login",
                    AccountID = 67890,
                    ActionResult = "Success",
                    Browser = "Chrome",
                    FirstDeviceCode = "DEV001",
                    IP = "192.168.1.1",
                    OS = "Windows",
                    RobotTracking = "No",
                    TransTime = DateTime.Now,
                    URLDetails = "https://example.com",
                    City = "New York",
                    Country = "US",
                    ISP = "Test ISP",
                    Region = "NY",
                    UserAgent = "Mozilla/5.0"
                }
            };

            _mockRepository.Setup(x => x.Fetch<TransactionDCSExtraItemModel>(It.IsAny<GetTransactionHistoryBySubscriberDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetTransactionHistory_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].AccountID, item.CTSCustID);
            Assert.Equal(expectedItems[0].LoginName, item.UserName);
            Assert.Equal(expectedItems[0].Action, item.Action);
            Assert.Equal(expectedItems[0].City, item.City);
            Assert.Equal(expectedItems[0].Country, item.Country);
            _mockRepository.Verify(x => x.Fetch<TransactionDCSExtraItemModel>(It.IsAny<GetTransactionHistoryBySubscriberDCSExtraCriteria>()), Times.Once);
        }

        [Fact]
        public void GetLastestTransactionBySubscriber_DCSExtra_WithValidParams_ReturnsExpectedResult()
        {
            // Arrange
            var param = new LatestTransactionParamModel
            {
                SubscriberID = 12345,
                Length = 100,
                Skip = 0,
                Take = 10
            };

            var expectedItems = new List<LatestTransactionDCSExtraItemModel>
            {
                new LatestTransactionDCSExtraItemModel
                {
                    TransID = 1,
                    SubscriberID = 12345,
                    AccountID = 67890,
                    LoginName = "testuser",
                    Action = "Login",
                    ActionResult = "Success",
                    TransTime = DateTime.Now,
                    FirstDeviceCode = "DEV001",
                    IP = "192.168.1.1",
                    OS = "Windows",
                    Browser = "Chrome",
                    URLDetails = "https://example.com",
                    RobotTracking = "No"
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestTransactionDCSExtraItemModel>(It.IsAny<GetLastestTransactionDCSExtraCriteria>()))
                .Returns(expectedItems);

            // Act
            var result = _service.GetLastestTransactionBySubscriber_DCSExtra(param);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            var item = result.Items.First();
            Assert.Equal(expectedItems[0].TransID, item.TransID);
            Assert.Equal(expectedItems[0].LoginName, item.UserName);
            Assert.Equal(expectedItems[0].Action, item.Action);
            _mockRepository.Verify(x => x.Fetch<LatestTransactionDCSExtraItemModel>(It.IsAny<GetLastestTransactionDCSExtraCriteria>()), Times.Once);
        }

        #endregion

        #region Null Parameter Tests

        [Fact]
        public void SearchAccount_DCSCenter_WithNullParam_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.SearchAccount_DCSCenter(null));
        }

        [Fact]
        public void GetAssAccountInfoByCustSubscriber_DCSCenter_WithNullParam_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.GetAssAccountInfoByCustSubscriber_DCSCenter(null));
        }

        [Fact]
        public void GetTransactionHistory_DCSCenter_WithNullParam_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.GetTransactionHistory_DCSCenter(null));
        }

        [Fact]
        public void SearchDevice_DCSExtra_WithNullParam_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.SearchDevice_DCSExtra(null));
        }

        #endregion
    }
} 