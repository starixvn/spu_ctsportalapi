namespace Fanex.NAP.CTS.Tests.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Model.DTO;
    using Fanex.NAP.CTS.Service.Implement;
    using Moq;
    using Xunit;

    public class AssociatedAccountServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly AssociatedAccountService _service;

        public AssociatedAccountServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new AssociatedAccountService(_mockRepository.Object);
        }

        #region GetFlaggedEvidenceByDirectAssForAPI Tests

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithValidCustIdList_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "1,2,3";
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 1,
                    EvidenceCode = "DEVICE_SHARED",
                    AssociatedLevel = 1
                },
                new CustomerEvidenceForTW
                {
                    Level0CustId = 2,
                    EvidenceCode = "IP_SHARED",
                    AssociatedLevel = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithEmptyCustIdList_ReturnsEmptyList()
        {
            // Arrange
            var custIdList = "";
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithNullCustIdList_ReturnsEmptyList()
        {
            // Arrange
            string custIdList = null;
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithSingleCustId_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "123";
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 123,
                    EvidenceCode = "BETTING_PATTERN",
                    AssociatedLevel = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithLargeCustIdList_ReturnsEvidence()
        {
            // Arrange
            var custIdList = string.Join(",", Enumerable.Range(1, 100));
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 1,
                    EvidenceCode = "DEVICE_SHARED",
                    AssociatedLevel = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        #endregion

        #region GetFlaggedEvidenceByLevel2AssForAPI Tests

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithValidCustIdList_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "1,2,3";
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 1,
                    EvidenceCode = "DEVICE_SHARED_LEVEL2",
                    AssociatedLevel = 2
                },
                new CustomerEvidenceForTW
                {
                    Level0CustId = 2,
                    EvidenceCode = "IP_SHARED_LEVEL2",
                    AssociatedLevel = 2
                },
                new CustomerEvidenceForTW
                {
                    Level0CustId = 3,
                    EvidenceCode = "BETTING_PATTERN_LEVEL2",
                    AssociatedLevel = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithEmptyCustIdList_ReturnsEmptyList()
        {
            // Arrange
            var custIdList = "";
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithNullCustIdList_ReturnsEmptyList()
        {
            // Arrange
            string custIdList = null;
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithSingleCustId_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "456";
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 456,
                    EvidenceCode = "MANUAL_ASSOCIATION",
                    AssociatedLevel = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithLargeCustIdList_ReturnsEvidence()
        {
            // Arrange
            var custIdList = string.Join(",", Enumerable.Range(100, 50));
            var expectedResult = new List<CustomerEvidenceForTW>
            {
                new CustomerEvidenceForTW
                {
                    Level0CustId = 100,
                    EvidenceCode = "DEVICE_SHARED_LEVEL2",
                    AssociatedLevel = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        #endregion

        #region GetAssociationForExtAcc Tests

        [Fact]
        public void GetAssociationForExtAcc_WithValidCustId_ReturnsAssociations()
        {
            // Arrange
            var custId = 123;
            var expectedResult = new List<AssociatedAccountModel>
            {
                new AssociatedAccountModel
                {
                    CTSCustID = 1001,
                    CustId = 123,
                    CustSubId = 1,
                    SubscriberID = 1,
                    SubscriberName = "Subscriber1",
                    Username = "user1",
                    Username2 = "user1_alt",
                    StatusId = 1,
                    StatusName = "Active",
                    AssociationStatus = 1,
                    AssociationDate = DateTime.Now,
                    ParentIDs = "1,2,3",
                    CategoryIDs = "10,20,30",
                    CategoryNames = "VIP,Regular,Premium",
                    AssociationType = new List<int> { 1, 2 },
                    AssociationTypeName = "Device,IP",
                    SharedDevices = 5,
                    SharedIPs = 3,
                    CustomerClasses = "1,2,3",
                    IsProbation = false,
                    GroupIDs = "1,2",
                    OriginGroupIDs = "1,2",
                    IsPA = false,
                    IsPotentialPA = true,
                    ColorTypeIDs = "1,2,3"
                },
                new AssociatedAccountModel
                {
                    CTSCustID = 1002,
                    CustId = 124,
                    CustSubId = 2,
                    SubscriberID = 2,
                    SubscriberName = "Subscriber2",
                    Username = "user2",
                    Username2 = "user2_alt",
                    StatusId = 1,
                    StatusName = "Active",
                    AssociationStatus = 1,
                    AssociationDate = DateTime.Now.AddDays(-1),
                    ParentIDs = "4,5,6",
                    CategoryIDs = "40,50,60",
                    CategoryNames = "Standard,Basic,Advanced",
                    AssociationType = new List<int> { 3, 4 },
                    AssociationTypeName = "Manual,BettingPattern",
                    SharedDevices = 2,
                    SharedIPs = 1,
                    CustomerClasses = "4,5,6",
                    IsProbation = true,
                    GroupIDs = "3,4",
                    OriginGroupIDs = "3,4",
                    IsPA = true,
                    IsPotentialPA = false,
                    ColorTypeIDs = "4,5,6"
                }
            };

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        [Fact]
        public void GetAssociationForExtAcc_WithZeroCustId_ReturnsAssociations()
        {
            // Arrange
            var custId = 0;
            var expectedResult = new List<AssociatedAccountModel>();

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        [Fact]
        public void GetAssociationForExtAcc_WithNegativeCustId_ReturnsAssociations()
        {
            // Arrange
            var custId = -123;
            var expectedResult = new List<AssociatedAccountModel>();

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        [Fact]
        public void GetAssociationForExtAcc_WithMaxIntCustId_ReturnsAssociations()
        {
            // Arrange
            var custId = int.MaxValue;
            var expectedResult = new List<AssociatedAccountModel>
            {
                new AssociatedAccountModel
                {
                    CTSCustID = long.MaxValue,
                    CustId = int.MaxValue,
                    CustSubId = 1,
                    SubscriberID = 1,
                    SubscriberName = "MaxSubscriber",
                    Username = "maxuser",
                    Username2 = "maxuser_alt",
                    StatusId = 1,
                    StatusName = "Active",
                    AssociationStatus = 1,
                    AssociationDate = DateTime.Now,
                    ParentIDs = "1",
                    CategoryIDs = "1",
                    CategoryNames = "MaxCategory",
                    AssociationType = new List<int> { 1 },
                    AssociationTypeName = "Device",
                    SharedDevices = 1,
                    SharedIPs = 1,
                    CustomerClasses = "1",
                    IsProbation = false,
                    GroupIDs = "1",
                    OriginGroupIDs = "1",
                    IsPA = false,
                    IsPotentialPA = false,
                    ColorTypeIDs = "1"
                }
            };

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        [Fact]
        public void GetAssociationForExtAcc_WithSingleAssociation_ReturnsSingleAssociation()
        {
            // Arrange
            var custId = 999;
            var expectedResult = new List<AssociatedAccountModel>
            {
                new AssociatedAccountModel
                {
                    CTSCustID = 100999,
                    CustId = 999,
                    CustSubId = 1,
                    SubscriberID = 999,
                    SubscriberName = "SingleSubscriber",
                    Username = "singleuser",
                    Username2 = "singleuser_alt",
                    StatusId = 1,
                    StatusName = "Active",
                    AssociationStatus = 1,
                    AssociationDate = DateTime.Now,
                    ParentIDs = "999",
                    CategoryIDs = "999",
                    CategoryNames = "SingleCategory",
                    AssociationType = new List<int> { 1 },
                    AssociationTypeName = "Device",
                    SharedDevices = 1,
                    SharedIPs = 1,
                    CustomerClasses = "999",
                    IsProbation = false,
                    GroupIDs = "999",
                    OriginGroupIDs = "999",
                    IsPA = false,
                    IsPotentialPA = false,
                    ColorTypeIDs = "999"
                }
            };

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithSpecialCharacters_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "1,2,3;4,5,6";
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithSpecialCharacters_ReturnsEvidence()
        {
            // Arrange
            var custIdList = "1,2,3;4,5,6";
            var expectedResult = new List<CustomerEvidenceForTW>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetAssociationForExtAcc_WithNullReturnFromRepository_ReturnsEmptyList()
        {
            // Arrange
            var custId = 123;
            IEnumerable<AssociatedAccountModel> expectedResult = null;

            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<AssociatedAccountModel>(It.Is<GetAssociationForExtAccCriteria>(c =>
                c.CustId == custId && c.AssStatus == 1 && c.AssType == 1)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByDirectAssForAPI_WithNullReturnFromRepository_ReturnsNull()
        {
            // Arrange
            var custIdList = "1,2,3";
            IEnumerable<CustomerEvidenceForTW> expectedResult = null;

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByDirectAssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        [Fact]
        public void GetFlaggedEvidenceByLevel2AssForAPI_WithNullReturnFromRepository_ReturnsNull()
        {
            // Arrange
            var custIdList = "1,2,3";
            IEnumerable<CustomerEvidenceForTW> expectedResult = null;

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<CustomerEvidenceForTW>(It.Is<GetFlaggedEvidenceByLevel2AssociationCriteria>(c =>
                c.CustIds == custIdList)), Times.Once);
        }

        #endregion

        #region Integration Style Tests

        [Fact]
        public void GetAllMethods_WithSameCustId_ExecutesSuccessfully()
        {
            // Arrange
            var custIdList = "1,2,3";
            var custId = 1;
            var expectedEvidence = new List<CustomerEvidenceForTW>();
            var expectedAssociations = new List<AssociatedAccountModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedEvidence);
            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedEvidence);
            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedAssociations);

            // Act
            var directEvidence = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList);
            var level2Evidence = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList);
            var associations = _service.GetAssociationForExtAcc(custId);

            // Assert
            Assert.NotNull(directEvidence);
            Assert.NotNull(level2Evidence);
            Assert.NotNull(associations);
            Assert.Empty(directEvidence);
            Assert.Empty(level2Evidence);
            Assert.Empty(associations);
        }

        [Fact]
        public void MultipleCalls_WithDifferentParameters_ExecutesSuccessfully()
        {
            // Arrange
            var custIdList1 = "1,2,3";
            var custIdList2 = "4,5,6";
            var custId1 = 1;
            var custId2 = 2;
            var expectedEvidence = new List<CustomerEvidenceForTW>();
            var expectedAssociations = new List<AssociatedAccountModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByDirectAssociationCriteria>()))
                .Returns(expectedEvidence);
            _mockRepository.Setup(x => x.Fetch<CustomerEvidenceForTW>(It.IsAny<GetFlaggedEvidenceByLevel2AssociationCriteria>()))
                .Returns(expectedEvidence);
            _mockRepository.Setup(x => x.Fetch<AssociatedAccountModel>(It.IsAny<GetAssociationForExtAccCriteria>()))
                .Returns(expectedAssociations);

            // Act
            var directEvidence1 = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList1);
            var directEvidence2 = _service.GetFlaggedEvidenceByDirectAssForAPI(custIdList2);
            var level2Evidence1 = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList1);
            var level2Evidence2 = _service.GetFlaggedEvidenceByLevel2AssForAPI(custIdList2);
            var associations1 = _service.GetAssociationForExtAcc(custId1);
            var associations2 = _service.GetAssociationForExtAcc(custId2);

            // Assert
            Assert.NotNull(directEvidence1);
            Assert.NotNull(directEvidence2);
            Assert.NotNull(level2Evidence1);
            Assert.NotNull(level2Evidence2);
            Assert.NotNull(associations1);
            Assert.NotNull(associations2);
        }

        #endregion
    }
}
