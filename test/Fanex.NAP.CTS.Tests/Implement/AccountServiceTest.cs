namespace Fanex.NAP.CTS.Tests.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Account;
    using Fanex.NAP.CTS.Criteria.Account.CustomerCategory;
    using Fanex.NAP.CTS.Criteria.Account.CustomerInfo;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Model.Account.CustomerInfo;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Service;
    using Moq;
    using Xunit;

    public class AccountServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly AccountService _service;

        public AccountServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new AccountService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        #region GetCustInfoByCustID Tests

        [Fact]
        public void GetCustInfoByCustID_WithValidCustIDs_ReturnsCustomerInfo()
        {
            // Arrange
            var custIDs = "1,2,3";
            var expectedResult = new List<CustInfoModel>
            {
                new CustInfoModel { CustID = 1, CTSCustID = 1001 },
                new CustInfoModel { CustID = 2, CTSCustID = 1002 },
                new CustInfoModel { CustID = 3, CTSCustID = 1003 }
            };

            _mockRepository.Setup(x => x.Fetch<CustInfoModel>(It.IsAny<CustInfoByCustIdGetCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustInfoByCustID(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CustInfoModel>(It.Is<CustInfoByCustIdGetCriteria>(c => c.CustIDs == custIDs)), Times.Once);
        }

        [Fact]
        public void GetCustInfoByCustID_WithEmptyCustIDs_ReturnsEmptyList()
        {
            // Arrange
            var custIDs = "";
            var expectedResult = new List<CustInfoModel>();

            _mockRepository.Setup(x => x.Fetch<CustInfoModel>(It.IsAny<CustInfoByCustIdGetCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustInfoByCustID(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustInfoModel>(It.Is<CustInfoByCustIdGetCriteria>(c => c.CustIDs == custIDs)), Times.Once);
        }

        [Fact]
        public void GetCustInfoByCustID_WithNullCustIDs_ReturnsEmptyList()
        {
            // Arrange
            string custIDs = null;
            var expectedResult = new List<CustInfoModel>();

            _mockRepository.Setup(x => x.Fetch<CustInfoModel>(It.IsAny<CustInfoByCustIdGetCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustInfoByCustID(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustInfoModel>(It.Is<CustInfoByCustIdGetCriteria>(c => c.CustIDs == custIDs)), Times.Once);
        }

        [Fact]
        public void GetCustInfoByCustID_WithSingleCustID_ReturnsSingleCustomer()
        {
            // Arrange
            var custIDs = "123";
            var expectedResult = new List<CustInfoModel>
            {
                new CustInfoModel { CustID = 123, CTSCustID = 100123 }
            };

            _mockRepository.Setup(x => x.Fetch<CustInfoModel>(It.IsAny<CustInfoByCustIdGetCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustInfoByCustID(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CustInfoModel>(It.Is<CustInfoByCustIdGetCriteria>(c => c.CustIDs == custIDs)), Times.Once);
        }

        #endregion

        #region GetCustomerInfoAssociationAccount Tests

        [Fact]
        public void GetCustomerInfoAssociationAccount_WithValidCTSCustId_ReturnsAssociationAccount()
        {
            // Arrange
            var ctsCustId = 12345L;
            var expectedResult = new CustomerInfoAssociationAccountModel
            {
                AI = 5,
                Manual = 3,
                Device = 8,
                IP = 2
            };

            _mockRepository.Setup(x => x.Get<CustomerInfoAssociationAccountModel>(It.IsAny<GetCustomerInfoAssociatedAccountCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerInfoAssociationAccount(ctsCustId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(18, result.Total); // AI(5) + Manual(3) + Device(8) + IP(2)
            _mockRepository.Verify(x => x.Get<CustomerInfoAssociationAccountModel>(It.Is<GetCustomerInfoAssociatedAccountCriteria>(c => c.CTSCustId == ctsCustId)), Times.Once);
        }

        [Fact]
        public void GetCustomerInfoAssociationAccount_WithZeroValues_ReturnsZeroTotal()
        {
            // Arrange
            var ctsCustId = 12345L;
            var expectedResult = new CustomerInfoAssociationAccountModel
            {
                AI = 0,
                Manual = 0,
                Device = 0,
                IP = 0
            };

            _mockRepository.Setup(x => x.Get<CustomerInfoAssociationAccountModel>(It.IsAny<GetCustomerInfoAssociatedAccountCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerInfoAssociationAccount(ctsCustId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(0, result.Total);
            _mockRepository.Verify(x => x.Get<CustomerInfoAssociationAccountModel>(It.Is<GetCustomerInfoAssociatedAccountCriteria>(c => c.CTSCustId == ctsCustId)), Times.Once);
        }

        [Fact]
        public void GetCustomerInfoAssociationAccount_WithNegativeValues_CalculatesTotalCorrectly()
        {
            // Arrange
            var ctsCustId = 12345L;
            var expectedResult = new CustomerInfoAssociationAccountModel
            {
                AI = -2,
                Manual = 5,
                Device = -1,
                IP = 3
            };

            _mockRepository.Setup(x => x.Get<CustomerInfoAssociationAccountModel>(It.IsAny<GetCustomerInfoAssociatedAccountCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerInfoAssociationAccount(ctsCustId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(5, result.Total); // -2 + 5 + (-1) + 3
            _mockRepository.Verify(x => x.Get<CustomerInfoAssociationAccountModel>(It.Is<GetCustomerInfoAssociatedAccountCriteria>(c => c.CTSCustId == ctsCustId)), Times.Once);
        }

        #endregion

        #region GetCustomerClassificationForAPI Tests

        [Fact]
        public void GetCustomerClassificationForAPI_WithValidParameters_ReturnsClassifications()
        {
            // Arrange
            var custIdList = "1,2,3";
            var parentCateIdList = "10,20,30";
            var expectedResult = new List<GetCustomerClassificationModel>
            {
                new GetCustomerClassificationModel
                {
                    CategoryID = 1,
                    CategoryName = "VIP",
                    CategoryGroupID = 1,
                    ColorTypeID = 1,
                    ParentID = 10,
                    CustID = 1,
                    ParentCategoryName = "Premium",
                    IsProbation = false,
                    IsVVIP = true,
                    IsPA = false,
                    IsMultiplePA = false,
                    CategoryPriority = 1
                },
                new GetCustomerClassificationModel
                {
                    CategoryID = 2,
                    CategoryName = "Regular",
                    CategoryGroupID = 2,
                    ColorTypeID = 2,
                    ParentID = 20,
                    CustID = 2,
                    ParentCategoryName = "Standard",
                    IsProbation = false,
                    IsVVIP = false,
                    IsPA = false,
                    IsMultiplePA = false,
                    CategoryPriority = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetCustomerClassificationModel>(It.IsAny<GetCTSCustomerClassificationForAPICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationForAPI(custIdList, parentCateIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<GetCustomerClassificationModel>(It.Is<GetCTSCustomerClassificationForAPICriteria>(c => 
                c.CustIdList == custIdList && c.ParentCateIdList == parentCateIdList)), Times.Once);
        }

        [Fact]
        public void GetCustomerClassificationForAPI_WithEmptyParameters_ReturnsEmptyList()
        {
            // Arrange
            var custIdList = "";
            var parentCateIdList = "";
            var expectedResult = new List<GetCustomerClassificationModel>();

            _mockRepository.Setup(x => x.Fetch<GetCustomerClassificationModel>(It.IsAny<GetCTSCustomerClassificationForAPICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationForAPI(custIdList, parentCateIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<GetCustomerClassificationModel>(It.Is<GetCTSCustomerClassificationForAPICriteria>(c => 
                c.CustIdList == custIdList && c.ParentCateIdList == parentCateIdList)), Times.Once);
        }

        [Fact]
        public void GetCustomerClassificationForAPI_WithNullParameters_ReturnsEmptyList()
        {
            // Arrange
            string custIdList = null;
            string parentCateIdList = null;
            var expectedResult = new List<GetCustomerClassificationModel>();

            _mockRepository.Setup(x => x.Fetch<GetCustomerClassificationModel>(It.IsAny<GetCTSCustomerClassificationForAPICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationForAPI(custIdList, parentCateIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<GetCustomerClassificationModel>(It.Is<GetCTSCustomerClassificationForAPICriteria>(c => 
                c.CustIdList == custIdList && c.ParentCateIdList == parentCateIdList)), Times.Once);
        }

        #endregion

        #region GetCustomerClassificationByCategory Tests

        [Fact]
        public void GetCustomerClassificationByCategory_WithValidCategoryIds_ReturnsClassifications()
        {
            // Arrange
            var categoryIdList = "1,2,3";
            var expectedResult = new List<CustomerClassificationGeneralInfoModel>
            {
                new CustomerClassificationGeneralInfoModel
                {
                    CustID = 1,
                    SportGroupID = 1,
                    SportGroupName = "Football",
                    CategoryID = 1,
                    CategoryName = "VIP"
                },
                new CustomerClassificationGeneralInfoModel
                {
                    CustID = 2,
                    SportGroupID = 2,
                    SportGroupName = "Basketball",
                    CategoryID = 2,
                    CategoryName = "Regular"
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.IsAny<GetCustomerCategoryByCategoryIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationByCategory(categoryIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.Is<GetCustomerCategoryByCategoryIdsCriteria>(c => 
                c.CategoryIDList == categoryIdList)), Times.Once);
        }

        [Fact]
        public void GetCustomerClassificationByCategory_WithEmptyCategoryIds_ReturnsEmptyList()
        {
            // Arrange
            var categoryIdList = "";
            var expectedResult = new List<CustomerClassificationGeneralInfoModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.IsAny<GetCustomerCategoryByCategoryIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationByCategory(categoryIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.Is<GetCustomerCategoryByCategoryIdsCriteria>(c => 
                c.CategoryIDList == categoryIdList)), Times.Once);
        }

        [Fact]
        public void GetCustomerClassificationByCategory_WithNullCategoryIds_ReturnsEmptyList()
        {
            // Arrange
            string categoryIdList = null;
            var expectedResult = new List<CustomerClassificationGeneralInfoModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.IsAny<GetCustomerCategoryByCategoryIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationByCategory(categoryIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.Is<GetCustomerCategoryByCategoryIdsCriteria>(c => 
                c.CategoryIDList == categoryIdList)), Times.Once);
        }

        [Fact]
        public void GetCustomerClassificationByCategory_WithSingleCategoryId_ReturnsSingleClassification()
        {
            // Arrange
            var categoryIdList = "5";
            var expectedResult = new List<CustomerClassificationGeneralInfoModel>
            {
                new CustomerClassificationGeneralInfoModel
                {
                    CustID = 10,
                    SportGroupID = 3,
                    SportGroupName = "Tennis",
                    CategoryID = 5,
                    CategoryName = "Premium"
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.IsAny<GetCustomerCategoryByCategoryIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassificationByCategory(categoryIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CustomerClassificationGeneralInfoModel>(It.Is<GetCustomerCategoryByCategoryIdsCriteria>(c => 
                c.CategoryIDList == categoryIdList)), Times.Once);
        }

        #endregion

        #region CheckCustomerNotExisted Tests

        [Fact]
        public void CheckCustomerNotExisted_WithValidParameters_ReturnsCustomers()
        {
            // Arrange
            var custIds = "1,2,3";
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var expectedResult = new List<CTSCustomer>
            {
                new CTSCustomer
                {
                    CustID = 1,
                    CTSCustID = 1001,
                    UserName = "user1",
                    UserName2 = "user1_alt",
                    SubscriberID = 1,
                    SubscriberName = "Subscriber1"
                },
                new CTSCustomer
                {
                    CustID = 2,
                    CTSCustID = 1002,
                    UserName = "user2",
                    UserName2 = "user2_alt",
                    SubscriberID = 2,
                    SubscriberName = "Subscriber2"
                }
            };

            _mockRepository.Setup(x => x.Fetch<CTSCustomer>(It.IsAny<CheckCustNotExistedByCustIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.CheckCustomerNotExisted(custIds, fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CTSCustomer>(It.Is<CheckCustNotExistedByCustIdsCriteria>(c => 
                c.CustIds == custIds && c.FromDate == fromDate && c.ToDate == toDate)), Times.Once);
        }

        [Fact]
        public void CheckCustomerNotExisted_WithEmptyCustIds_ReturnsEmptyList()
        {
            // Arrange
            var custIds = "";
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var expectedResult = new List<CTSCustomer>();

            _mockRepository.Setup(x => x.Fetch<CTSCustomer>(It.IsAny<CheckCustNotExistedByCustIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.CheckCustomerNotExisted(custIds, fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CTSCustomer>(It.Is<CheckCustNotExistedByCustIdsCriteria>(c => 
                c.CustIds == custIds && c.FromDate == fromDate && c.ToDate == toDate)), Times.Once);
        }

        [Fact]
        public void CheckCustomerNotExisted_WithNullCustIds_ReturnsEmptyList()
        {
            // Arrange
            string custIds = null;
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var expectedResult = new List<CTSCustomer>();

            _mockRepository.Setup(x => x.Fetch<CTSCustomer>(It.IsAny<CheckCustNotExistedByCustIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.CheckCustomerNotExisted(custIds, fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CTSCustomer>(It.Is<CheckCustNotExistedByCustIdsCriteria>(c => 
                c.CustIds == custIds && c.FromDate == fromDate && c.ToDate == toDate)), Times.Once);
        }

        [Fact]
        public void CheckCustomerNotExisted_WithSameFromAndToDate_ReturnsCustomers()
        {
            // Arrange
            var custIds = "1,2,3";
            var fromDate = new DateTime(2023, 6, 15);
            var toDate = new DateTime(2023, 6, 15);
            var expectedResult = new List<CTSCustomer>
            {
                new CTSCustomer
                {
                    CustID = 1,
                    CTSCustID = 1001,
                    UserName = "user1",
                    UserName2 = "user1_alt",
                    SubscriberID = 1,
                    SubscriberName = "Subscriber1"
                }
            };

            _mockRepository.Setup(x => x.Fetch<CTSCustomer>(It.IsAny<CheckCustNotExistedByCustIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.CheckCustomerNotExisted(custIds, fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CTSCustomer>(It.Is<CheckCustNotExistedByCustIdsCriteria>(c => 
                c.CustIds == custIds && c.FromDate == fromDate && c.ToDate == toDate)), Times.Once);
        }

        [Fact]
        public void CheckCustomerNotExisted_WithFutureDates_ReturnsCustomers()
        {
            // Arrange
            var custIds = "1,2,3";
            var fromDate = new DateTime(2024, 1, 1);
            var toDate = new DateTime(2024, 12, 31);
            var expectedResult = new List<CTSCustomer>();

            _mockRepository.Setup(x => x.Fetch<CTSCustomer>(It.IsAny<CheckCustNotExistedByCustIdsCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.CheckCustomerNotExisted(custIds, fromDate, toDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CTSCustomer>(It.Is<CheckCustNotExistedByCustIdsCriteria>(c => 
                c.CustIds == custIds && c.FromDate == fromDate && c.ToDate == toDate)), Times.Once);
        }

        #endregion
    }
} 