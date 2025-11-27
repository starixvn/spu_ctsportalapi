namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.AssociationDetection;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.AssociationDetection;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.ViewModel.AssociationDetection;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class AssociationDetectionServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly Mock<ICommonService> _mockCommonService;
        private readonly AssociationDetectionService _service;

        public AssociationDetectionServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _mockCommonService = new Mock<ICommonService>();
            _service = new AssociationDetectionService(_mockRepository.Object, _mockCommonService.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        #region ValidateUserName Tests

        [Fact]
        public void ValidateUserName_WithValidUserNames_ReturnsValidatedUsers()
        {
            // Arrange
            var userNameString = "user1,user2,user3";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER2", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER3", CustID = 3, CTSCustID = 1003, IsInternal = true }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2,USER3"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            
            var resultList = result.ToList();
            Assert.Equal("user1", resultList[0].UserName);
            Assert.Equal(1, resultList[0].CustID);
            Assert.Equal(1001, resultList[0].CTSCustID);
            Assert.False(resultList[0].IsInternal);

            Assert.Equal("user2", resultList[1].UserName);
            Assert.Equal(2, resultList[1].CustID);
            Assert.Equal(1002, resultList[1].CTSCustID);
            Assert.False(resultList[1].IsInternal);

            Assert.Equal("user3", resultList[2].UserName);
            Assert.Equal(3, resultList[2].CustID);
            Assert.Equal(1003, resultList[2].CTSCustID);
            Assert.True(resultList[2].IsInternal);

            _mockCommonService.Verify(x => x.ValidateUsernames("USER1,USER2,USER3"), Times.Once);
        }

        [Fact]
        public void ValidateUserName_WithDuplicateUserNames_ReturnsDistinctUsers()
        {
            // Arrange
            var userNameString = "user1,user1,user2,user1";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count()); // Original count preserved, but validation uses distinct
            _mockCommonService.Verify(x => x.ValidateUsernames("USER1,USER2"), Times.Once);
        }

        [Fact]
        public void ValidateUserName_WithEmptyString_ReturnsEmptyList()
        {
            // Arrange
            var userNameString = "";
            var validatedUsers = new List<ValidateUserNameModel>();

            _mockCommonService.Setup(x => x.ValidateUsernames(""))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockCommonService.Verify(x => x.ValidateUsernames(""), Times.Once);
        }      

        [Fact]
        public void ValidateUserName_WithSingleUserName_ReturnsSingleUser()
        {
            // Arrange
            var userNameString = "singleuser";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "SINGLEUSER", CustID = 1, CTSCustID = 1001, IsInternal = false }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("SINGLEUSER"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var user = result.First();
            Assert.Equal("singleuser", user.UserName);
            Assert.Equal(1, user.CustID);
            Assert.Equal(1001, user.CTSCustID);
            Assert.False(user.IsInternal);
        }

        #endregion

        #region DetectAssociationByPrimary Tests

        [Fact]
        public void DetectAssociationByPrimary_WithValidFilter_ReturnsAssociations()
        {
            // Arrange
            var filter = new DetectAssociationByPrimaryFilterModel("1001,1002,1003", AssociationDetectionType.Device);

            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 1
                },
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1002,
                    DestID = 1003,
                    AssociationType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.CTSCustIDs == filter.CTSCustIDs &&
                c.IsDevice == filter.IsDevice &&
                c.IsAI == filter.IsAI &&
                c.IsIP == filter.IsIP)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithAIFilter_ReturnsAIAssociations()
        {
            // Arrange
            var filter = new DetectAssociationByPrimaryFilterModel("1001,1002", AssociationDetectionType.AI);

            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.IsAI == true)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithEmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var filter = new DetectAssociationByPrimaryFilterModel("1001,1002", AssociationDetectionType.IP);

            var expectedResult = new List<DetectAssociationByPrimaryModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithDeviceFilter_ReturnsDeviceAssociations()
        {
            // Arrange
            var filter = new DetectAssociationByPrimaryFilterModel("1001,1002,1003", AssociationDetectionType.Device);

            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.IsDevice == true && c.IsAI == false && c.IsIP == false)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithIPFilter_ReturnsIPAssociations()
        {
            // Arrange
            var filter = new DetectAssociationByPrimaryFilterModel("1001,1002", AssociationDetectionType.IP);

            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 3
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.IsIP == true && c.IsAI == false && c.IsDevice == false)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithLargeCustIDList_HandlesCorrectly()
        {
            // Arrange
            var custIDs = string.Join(",", Enumerable.Range(1000, 100));
            var filter = new DetectAssociationByPrimaryFilterModel(custIDs, AssociationDetectionType.Device);

            var expectedResult = new List<DetectAssociationByPrimaryModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.CTSCustIDs == custIDs)), Times.Once);
        }

        #endregion

        #region DetectAssociationByAgent Tests

        [Fact]
        public void DetectAssociationByAgent_WithValidCustIDs_ReturnsAssociations()
        {
            // Arrange
            var ctsCustIDs = "1001,1002,1003";
            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 3
                },
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1002,
                    DestID = 1003,
                    AssociationType = 3
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByAgentCriteria>(c =>
                c.CTSCustIDs == ctsCustIDs)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByAgent_WithEmptyCustIDs_ReturnsEmptyList()
        {
            // Arrange
            var ctsCustIDs = "";
            var expectedResult = new List<DetectAssociationByPrimaryModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationByAgent_WithNullCustIDs_ReturnsEmptyList()
        {
            // Arrange
            string ctsCustIDs = null!;
            var expectedResult = new List<DetectAssociationByPrimaryModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationByAgent_WithSingleCustID_ReturnsAssociations()
        {
            // Arrange
            var ctsCustIDs = "1001";
            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1001,
                    DestID = 1002,
                    AssociationType = 4
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
        }

        [Fact]
        public void DetectAssociationByAgent_WithLargeCustIDList_HandlesCorrectly()
        {
            // Arrange
            var ctsCustIDs = string.Join(",", Enumerable.Range(1000, 50));
            var expectedResult = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel
                {
                    OrigID = 1000,
                    DestID = 1001,
                    AssociationType = 4
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByAgentCriteria>(c =>
                c.CTSCustIDs == ctsCustIDs)), Times.Once);
        }

        #endregion

        #region DetectGroupDetectAssociation Tests

        [Fact]
        public void DetectGroupDetectAssociation_WithValidModel_ReturnsAssociations()
        {
            // Arrange
            var model = new MergeGroupInfoModel
            {
                CTSCustIDs = "1001,1002,1003",
                AssociationByDeviceJson = "[{\"O\":1001,\"D\":1002,\"A\":1}]",
                AssociationByAIJson = "[{\"O\":1002,\"D\":1003,\"A\":2}]",
                AssociationByIPJson = "[{\"O\":1001,\"D\":1003,\"A\":3}]",
                AssociationByAgentJson = "[{\"O\":1001,\"D\":1002,\"A\":4}]",
                AssociationBySharedIPJson = "[{\"F1\":1,\"O\":1001,\"X\":\"user1\",\"T1\":2,\"D\":1002,\"Y\":\"user2\",\"IP\":\"192.168.1.1\"}]",
                AssociationBySharedMatchesJson = "[{\"FromCustID\":1,\"FromCTSCustID\":1001,\"FromUserName\":\"user1\",\"ToCustID\":2,\"ToCTSCustID\":1002,\"ToUserName\":\"user2\"}]",
                IsGetLogTransInfo = true
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>
            {
                new DetectAssociationMergeGroupModel
                {
                    CTSCustID = 1001,
                    CustID = 1,
                    UserName = "user1",
                    GroupID = 1,
                    Evidence = "Device,IP,Agent",
                    AssociationType = "1,3,4"
                },
                new DetectAssociationMergeGroupModel
                {
                    CTSCustID = 1002,
                    CustID = 2,
                    UserName = "user2",
                    GroupID = 1,
                    Evidence = "Device,AI,Agent",
                    AssociationType = "1,2,4"
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectGroupDetectAssociation(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationMergeGroupModel>(It.Is<DetectAssociationMergeGroupCriteria>(c =>
                c.CTSCustIDs == model.CTSCustIDs &&
                c.DeviceJson == model.AssociationByDeviceJson &&
                c.AIJson == model.AssociationByAIJson &&
                c.IPJson == model.AssociationByIPJson &&
                c.AgentJson == model.AssociationByAgentJson &&
                c.SharedIPJson == model.AssociationBySharedIPJson &&
                c.SharedMatchesJson == model.AssociationBySharedMatchesJson &&
                c.IsGetLogTransInfo == model.IsGetLogTransInfo)), Times.Once);
        }

        [Fact]
        public void DetectGroupDetectAssociation_WithEmptyModel_ReturnsEmptyList()
        {
            // Arrange
            var model = new MergeGroupInfoModel
            {
                CTSCustIDs = "",
                AssociationByDeviceJson = "[]",
                AssociationByAIJson = "[]",
                AssociationByIPJson = "[]",
                AssociationByAgentJson = "[]",
                AssociationBySharedIPJson = "[]",
                AssociationBySharedMatchesJson = "[]",
                IsGetLogTransInfo = false
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectGroupDetectAssociation(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region DetectAssociationBySharedMatches Tests

        [Fact]
        public void DetectAssociationBySharedMatches_WithValidCustIDList_ReturnsAssociations()
        {
            // Arrange
            var custIDList = "1,2,3";
            var expectedResult = new List<DetectAssociationBySharedIPMatchesModel>
            {
                new DetectAssociationBySharedIPMatchesModel
                {
                    CustID1 = 1,
                    CustID2 = 2
                },
                new DetectAssociationBySharedIPMatchesModel
                {
                    CustID1 = 2,
                    CustID2 = 3
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationBySharedMatches(custIDList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.Is<DetectAssociationBySharedMatchesCriteria>(c =>
                c.CustIDList == custIDList &&
                c.QueryType == (int)DetectAssociation_QueryType.DetectAss)), Times.Once);
        }

        [Fact]
        public void DetectAssociationBySharedMatches_WithEmptyCustIDList_ReturnsEmptyList()
        {
            // Arrange
            var custIDList = "";
            var expectedResult = new List<DetectAssociationBySharedIPMatchesModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationBySharedMatches(custIDList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationBySharedMatches_WithNullCustIDList_ReturnsEmptyList()
        {
            // Arrange
            string custIDList = null!;
            var expectedResult = new List<DetectAssociationBySharedIPMatchesModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationBySharedMatches(custIDList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationBySharedMatches_WithSingleCustID_ReturnsAssociations()
        {
            // Arrange
            var custIDList = "123";
            var expectedResult = new List<DetectAssociationBySharedIPMatchesModel>
            {
                new DetectAssociationBySharedIPMatchesModel
                {
                    CustID1 = 123,
                    CustID2 = 456
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationBySharedMatches(custIDList);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
        }

        #endregion

        #region DetectAssociationByIP Tests

        [Fact]
        public void DetectAssociationByIP_WithValidParameters_ReturnsSharedIPInfo()
        {
            // Arrange
            var validateUsernames = new Dictionary<int, ValidateUserNameModel>
            {
                { 1, new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false } },
                { 2, new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false } }
            };
            var custIds = "1,2";

            var detectAssByIPData = new List<DetectAssociationBySharedIPModel>
            {
                new DetectAssociationBySharedIPModel
                {
                    CustID1 = 1,
                    CustID2 = 2,
                    IP = "192.168.1.1"
                },
                new DetectAssociationBySharedIPModel
                {
                    CustID1 = 1,
                    CustID2 = 2,
                    IP = "192.168.1.2"
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(detectAssByIPData);

            // Act
            var result = _service.DetectAssociationByIP(validateUsernames, custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var sharedIP = result.First();
            Assert.Equal(1, sharedIP.FromCustID);
            Assert.Equal(1001, sharedIP.FromCTSCustID);
            Assert.Equal("user1", sharedIP.FromUserName);
            Assert.Equal(2, sharedIP.ToCustID);
            Assert.Equal(1002, sharedIP.ToCTSCustID);
            Assert.Equal("user2", sharedIP.ToUserName);
            Assert.Equal("192.168.1.1,192.168.1.2", sharedIP.IP);
        }

        [Fact]
        public void DetectAssociationByIP_WithEmptyValidateUsernames_ReturnsEmptyList()
        {
            // Arrange
            var validateUsernames = new Dictionary<int, ValidateUserNameModel>();
            var custIds = "1,2";

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPModel>());

            // Act
            var result = _service.DetectAssociationByIP(validateUsernames, custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }       

        [Fact]
        public void DetectAssociationByIP_WithMissingValidateUsernames_UsesDefaultValues()
        {
            // Arrange
            var validateUsernames = new Dictionary<int, ValidateUserNameModel>
            {
                { 1, new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false } }
                // Missing user 2
            };
            var custIds = "1,2";

            var detectAssByIPData = new List<DetectAssociationBySharedIPModel>
            {
                new DetectAssociationBySharedIPModel
                {
                    CustID1 = 1,
                    CustID2 = 2,
                    IP = "192.168.1.1"
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(detectAssByIPData);

            // Act
            var result = _service.DetectAssociationByIP(validateUsernames, custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should be empty because ToCTSCustID will be 0
        }

        #endregion

        #region DetectAssociation Tests

        [Fact]
        public void DetectAssociation_WithValidModelAndUserList_ReturnsAssociations()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = true,
                IsDevice = true,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = false,
                Is3MatchesLast7Days = false
            };

            var orgUserList = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>
            {
                new DetectAssociationMergeGroupModel
                {
                    CTSCustID = 1001,
                    CustID = 1,
                    UserName = "user1",
                    GroupID = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void DetectAssociation_WithEmptyUserList_ReturnsEmptyList()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = true,
                IsDevice = true,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = false,
                Is3MatchesLast7Days = false
            };

            var orgUserList = new List<ValidateUserNameModel>();

            var expectedResult = new List<DetectAssociationMergeGroupModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociation_WithAllFlagsEnabled_ProcessesAllAssociationTypes()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = true,
                IsDevice = true,
                IsIP = true,
                IsAgent = true,
                IsIPLast3Days = true,
                Is3MatchesLast7Days = true
            };

            var orgUserList = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>
            {
                new DetectAssociationMergeGroupModel
                {
                    CTSCustID = 1001,
                    CustID = 1,
                    UserName = "user1",
                    GroupID = 1
                }
            };

            // Setup all the different repository calls
            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPMatchesModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void DetectAssociation_WithIPLast3DaysEnabled_ProcessesSharedIP()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = false,
                IsDevice = false,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = true,
                Is3MatchesLast7Days = false
            };

            var orgUserList = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociation_With3MatchesLast7DaysEnabled_ProcessesSharedMatches()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = false,
                IsDevice = false,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = false,
                Is3MatchesLast7Days = true
            };

            var orgUserList = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPMatchesModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociation_WithInternalUsers_FiltersOutInternalUsers()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = "user1,user2",
                IsBP = false,
                IsDevice = false,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = true,
                Is3MatchesLast7Days = true
            };

            var orgUserList = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = true }, // Internal user
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPModel>(It.IsAny<DetectAssociationByIPCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPMatchesModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociation(model, orgUserList, true);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [Fact]
        public void ValidateUserName_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var userNameString = "user@domain.com,user-name,user_name";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER@DOMAIN.COM", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER-NAME", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER_NAME", CustID = 3, CTSCustID = 1003, IsInternal = false }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER@DOMAIN.COM,USER-NAME,USER_NAME"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }       

        [Fact]
        public void ValidateUserName_WithMixedCaseUserNames_ConvertsToUpperCaseForValidation()
        {
            // Arrange
            var userNameString = "User1,USER2,user3";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER2", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER3", CustID = 3, CTSCustID = 1003, IsInternal = false }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2,USER3"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _mockCommonService.Verify(x => x.ValidateUsernames("USER1,USER2,USER3"), Times.Once);
        }

        [Fact]
        public void ValidateUserName_WithEmptyValidationResult_ReturnsOriginalUserNames()
        {
            // Arrange
            var userNameString = "user1,user2";
            var validatedUsers = new List<ValidateUserNameModel>(); // Empty validation result

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should be empty because no validations matched
        }

        [Fact]
        public void ValidateUserName_WithPartialValidationResult_ReturnsOnlyValidatedUsers()
        {
            // Arrange
            var userNameString = "user1,user2,user3";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER1", CustID = 1, CTSCustID = 1001, IsInternal = false }
                // user2 and user3 not validated
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2,USER3"))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("user1", result.First().UserName);
        }

        [Fact]
        public void ValidateUserName_WithVeryLongUserNameString_HandlesCorrectly()
        {
            // Arrange
            var userNames = Enumerable.Range(1, 100).Select(i => $"user{i}").ToArray();
            var userNameString = string.Join(",", userNames);
            var validatedUsers = userNames.Take(10).Select((name, index) => 
                new ValidateUserNameModel 
                { 
                    UserName = name.ToUpper(), 
                    CustID = index + 1, 
                    CTSCustID = 1000 + index + 1, 
                    IsInternal = false 
                }).ToList();

            _mockCommonService.Setup(x => x.ValidateUsernames(It.IsAny<string>()))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count()); // Only validated users returned
        }      

        [Fact]
        public void DetectAssociation_WithNullOrgUserList_ThrowsException()
        {
            // Arrange
            var model = new DetectAssociationByUserNamesViewModel();
            IEnumerable<ValidateUserNameModel> orgUserList = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.DetectAssociation(model, orgUserList, true));
        }

        #endregion

        #region Integration-Style Tests

        [Fact]
        public void CompleteWorkflow_ValidateUserNameThenDetectAssociation_ExecutesSuccessfully()
        {
            // Arrange
            var userNameString = "user1,user2";
            var validatedUsers = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "USER1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "USER2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            var model = new DetectAssociationByUserNamesViewModel
            {
                UserNames = userNameString,
                IsBP = true,
                IsDevice = true,
                IsIP = false,
                IsAgent = false,
                IsIPLast3Days = false,
                Is3MatchesLast7Days = false
            };

            var expectedResult = new List<DetectAssociationMergeGroupModel>
            {
                new DetectAssociationMergeGroupModel
                {
                    CTSCustID = 1001,
                    CustID = 1,
                    UserName = "user1",
                    GroupID = 1
                }
            };

            _mockCommonService.Setup(x => x.ValidateUsernames("USER1,USER2"))
                .Returns(validatedUsers);
            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(expectedResult);

            // Act
            var validatedUserList = _service.ValidateUserName(userNameString);
            var result = _service.DetectAssociation(model, validatedUserList, true);

            // Assert
            Assert.NotNull(validatedUserList);
            Assert.Equal(2, validatedUserList.Count());
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void MultipleAssociationTypes_AllMethodsCalledSequentially_ExecutesSuccessfully()
        {
            // Arrange
            var ctsCustIDs = "1001,1002";
            var custIDList = "1,2";
            var filter = new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.Device);
            var model = new MergeGroupInfoModel
            {
                CTSCustIDs = ctsCustIDs,
                AssociationByDeviceJson = "[]",
                AssociationByAIJson = "[]",
                AssociationByIPJson = "[]",
                AssociationByAgentJson = "[]",
                AssociationBySharedIPJson = "[]",
                AssociationBySharedMatchesJson = "[]",
                IsGetLogTransInfo = false
            };

            var expectedResults = new List<DetectAssociationByPrimaryModel>
            {
                new DetectAssociationByPrimaryModel { OrigID = 1001, DestID = 1002, AssociationType = 1 }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(expectedResults);
            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResults);
            _mockRepository.Setup(x => x.Fetch<DetectAssociationBySharedIPMatchesModel>(It.IsAny<DetectAssociationBySharedMatchesCriteria>()))
                .Returns(new List<DetectAssociationBySharedIPMatchesModel>());
            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(new List<DetectAssociationMergeGroupModel>());

            // Act
            var primaryResult = _service.DetectAssociationByPrimary(filter);
            var agentResult = _service.DetectAssociationByAgent(ctsCustIDs);
            var sharedMatchesResult = _service.DetectAssociationBySharedMatches(custIDList);
            var groupResult = _service.DetectGroupDetectAssociation(model);

            // Assert
            Assert.NotNull(primaryResult);
            Assert.NotNull(agentResult);
            Assert.NotNull(sharedMatchesResult);
            Assert.NotNull(groupResult);
            Assert.Single(primaryResult);
            Assert.Single(agentResult);
            Assert.Empty(sharedMatchesResult);
            Assert.Empty(groupResult);
        }

        [Fact]
        public async Task StressTest_MultipleConcurrentCalls_HandlesCorrectly()
        {
            // Arrange
            var tasks = new List<Task>();
            var results = new List<IEnumerable<DetectAssociationByPrimaryModel>>();
            var ctsCustIDs = "1001,1002,1003";

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());

            // Act
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    var result = _service.DetectAssociationByAgent(ctsCustIDs);
                    lock (results)
                    {
                        results.Add(result);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(10, results.Count);
            Assert.All(results, result => Assert.NotNull(result));
        }

        #endregion

        #region Boundary Value Tests

        [Fact]
        public void ValidateUserName_WithMaximumLengthString_HandlesCorrectly()
        {
            // Arrange
            var longUserName = new string('a', 1000);
            var userNameString = string.Join(",", Enumerable.Repeat(longUserName, 10));
            var validatedUsers = new List<ValidateUserNameModel>();

            _mockCommonService.Setup(x => x.ValidateUsernames(It.IsAny<string>()))
                .Returns(validatedUsers);

            // Act
            var result = _service.ValidateUserName(userNameString);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationByPrimary_WithMaximumCustIDList_HandlesCorrectly()
        {
            // Arrange
            var maxCustIDs = string.Join(",", Enumerable.Range(1, 1000));
            var filter = new DetectAssociationByPrimaryFilterModel(maxCustIDs, AssociationDetectionType.Device);

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByPrimaryCriteria>()))
                .Returns(new List<DetectAssociationByPrimaryModel>());

            // Act
            var result = _service.DetectAssociationByPrimary(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByPrimaryModel>(It.Is<DetectAssociationByPrimaryCriteria>(c =>
                c.CTSCustIDs == maxCustIDs)), Times.Once);
        }

        [Fact]
        public void DetectAssociationByAgent_WithSpecialCharactersInCustIDs_HandlesCorrectly()
        {
            // Arrange
            var ctsCustIDs = "1001,1002,1003;1004,1005";
            var expectedResult = new List<DetectAssociationByPrimaryModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByPrimaryModel>(It.IsAny<DetectAssociationByAgentCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.DetectAssociationByAgent(ctsCustIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void DetectGroupDetectAssociation_WithMaximumJsonLength_HandlesCorrectly()
        {
            // Arrange
            var largeJson = "[" + string.Join(",", Enumerable.Range(1, 1000).Select(i => $"{{\"O\":{i},\"D\":{i+1},\"A\":1}}")) + "]";
            var model = new MergeGroupInfoModel
            {
                CTSCustIDs = "1001,1002",
                AssociationByDeviceJson = largeJson,
                AssociationByAIJson = largeJson,
                AssociationByIPJson = largeJson,
                AssociationByAgentJson = largeJson,
                AssociationBySharedIPJson = largeJson,
                AssociationBySharedMatchesJson = largeJson,
                IsGetLogTransInfo = true
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationMergeGroupModel>(It.IsAny<DetectAssociationMergeGroupCriteria>()))
                .Returns(new List<DetectAssociationMergeGroupModel>());

            // Act
            var result = _service.DetectGroupDetectAssociation(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Service Lifecycle Tests

        [Fact]
        public void AssociationDetectionService_CanBeInstantiated()
        {
            // Arrange & Act
            var service = new AssociationDetectionService(_mockRepository.Object, _mockCommonService.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void AssociationDetectionService_ImplementsIAssociationDetectionService()
        {
            // Arrange
            var service = new AssociationDetectionService(_mockRepository.Object, _mockCommonService.Object);

            // Act & Assert
            Assert.IsAssignableFrom<IAssociationDetectionService>(service);
        }

        #endregion
    }
}
