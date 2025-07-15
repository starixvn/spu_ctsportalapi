using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using Fanex.NAP.CTS.Core;
using Fanex.NAP.CTS.Core.Service;
using Fanex.NAP.CTS.Criteria.CustomerClassification;
using Fanex.NAP.CTS.Criteria.SyncCustomerToMainDb;
using Fanex.NAP.CTS.Model.CustomerClassification;
using Fanex.NAP.CTS.Model.SyncCustomerToMainDb;
using Fanex.NAP.CTS.Service;
using Moq;
using Xunit;
using static Fanex.NAP.CTS.Resource.Consts.Consts;

namespace Fanex.NAP.CTS.Tests.Implement.CustomerClassification
{
    public class CustomerClassificationServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly Mock<IAntiFraudCenterService> _mockAntiFraudCenterService;
        private readonly CustomerClassificationService _service;

        public CustomerClassificationServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _mockAntiFraudCenterService = new Mock<IAntiFraudCenterService>();
            _service = new CustomerClassificationService(_mockRepository.Object, _mockAntiFraudCenterService.Object);
        }

        #region PushDataToExternalPackge Tests

        [Fact]
        public void PushDataToExternalPackge_WithValidCustIds_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new PushDataToExternalModel
            {
                ExternalBatchSize = 100,
                RoleGroup = RoleGroupType.Member,
                IsSyncAFC = true,
                IsUpdateDangerToMainDB = false
            };
            var fromFlow = FromFlow.CTSAPI;

            var latestCCData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 1,
                    CategoryID = 1,
                    DangerLevel = null,
                    DangerLevelType = null
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(latestCCData);

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 0;
                    }
                });

            _mockAntiFraudCenterService.Setup(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), It.IsAny<FromFlow>(), It.IsAny<bool>()));

            // Act
            var result = _service.PushDataToExternalPackge(custIds, paras, fromFlow);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Once);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
            _mockAntiFraudCenterService.Verify(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), fromFlow, paras.IsSyncAFC), Times.Once);
        }

        [Fact]
        public void PushDataToExternalPackge_WithNullCustIds_ReturnsTrue()
        {
            // Arrange
            IEnumerable<long> custIds = null;
            var paras = new PushDataToExternalModel();
            var fromFlow = FromFlow.CTSWEB;

            // Act
            var result = _service.PushDataToExternalPackge(custIds, paras, fromFlow);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
            _mockAntiFraudCenterService.Verify(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), It.IsAny<FromFlow>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void PushDataToExternalPackge_WithEmptyCustIds_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long>();
            var paras = new PushDataToExternalModel();
            var fromFlow = FromFlow.CTSServices;

            // Act
            var result = _service.PushDataToExternalPackge(custIds, paras, fromFlow);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
            _mockAntiFraudCenterService.Verify(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), It.IsAny<FromFlow>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void PushDataToExternalPackge_WithManualTrigger_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new PushDataToExternalModel
            {
                ExternalBatchSize = 50,
                RoleGroup = RoleGroupType.Agency,
                IsSyncAFC = false,
                IsUpdateDangerToMainDB = true
            };
            var fromFlow = FromFlow.ManualTrigger;

            var latestCCData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 2,
                    CategoryID = 3,
                    DangerLevel = 7,
                    DangerLevelType = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(latestCCData);

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 0;
                    }
                    else if (command is DangerLevelUpdateCriteria dangerCriteria)
                    {
                        dangerCriteria.ErrorCode = 0;
                    }
                });

            _mockAntiFraudCenterService.Setup(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), It.IsAny<FromFlow>(), It.IsAny<bool>()));

            // Act
            var result = _service.PushDataToExternalPackge(custIds, paras, fromFlow);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Once);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Exactly(2)); // Once for sync, once for danger level
            _mockAntiFraudCenterService.Verify(x => x.InsertAFCAccounts(It.IsAny<IEnumerable<long>>(), fromFlow, paras.IsSyncAFC), Times.Once);
        }

        #endregion

        #region PushDataToExternal Tests

        [Fact]
        public void PushDataToExternal_WithValidCustIds_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new PushDataToExternalModel
            {
                ExternalBatchSize = 100,
                RoleGroup = RoleGroupType.Member,
                IsUpdateDangerToMainDB = false
            };

            var latestCCData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 1,
                    CategoryID = 1,
                    DangerLevel = null,
                    DangerLevelType = null
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(latestCCData);

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 0;
                    }
                });

            // Act
            var result = _service.PushDataToExternal(custIds, paras);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Once);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
        }

        [Fact]
        public void PushDataToExternal_WithNullCustIds_ReturnsTrue()
        {
            // Arrange
            IEnumerable<long> custIds = null;
            var paras = new PushDataToExternalModel();

            // Act
            var result = _service.PushDataToExternal(custIds, paras);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        [Fact]
        public void PushDataToExternal_WithEmptyCustIds_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long>();
            var paras = new PushDataToExternalModel();

            // Act
            var result = _service.PushDataToExternal(custIds, paras);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        [Fact]
        public void PushDataToExternal_WithDangerLevelUpdate_ReturnsTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new PushDataToExternalModel
            {
                ExternalBatchSize = 100,
                RoleGroup = RoleGroupType.Member,
                IsUpdateDangerToMainDB = true
            };

            var latestCCData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 1,
                    CategoryID = 1,
                    DangerLevel = 5,
                    DangerLevelType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(latestCCData);

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 0;
                    }
                    else if (command is DangerLevelUpdateCriteria dangerCriteria)
                    {
                        dangerCriteria.ErrorCode = 0;
                    }
                });

            // Act
            var result = _service.PushDataToExternal(custIds, paras);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Exactly(2)); // Once for sync, once for danger level
        }

        #endregion

        #region RetryPushCCToMainDB Tests

        [Fact]
        public void RetryPushCCToMainDB_WithValidData_ReturnsTrue()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>
            {
                new LatestClassificationModel
                {
                    CustId = 1,
                    CustomerClass = 1,
                    Category = 1
                }
            };
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 3;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 0;
                        syncCriteria.ErrorMessage = string.Empty;
                    }
                });

            // Act
            var result = _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
        }

        [Fact]
        public void RetryPushCCToMainDB_WithNullData_ReturnsTrue()
        {
            // Arrange
            IEnumerable<LatestClassificationModel> customerClassifications = null;
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 3;

            // Act
            var result = _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Never);
        }

        [Fact]
        public void RetryPushCCToMainDB_WithEmptyData_ReturnsTrue()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>();
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 3;

            // Act
            var result = _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Never);
        }

        [Fact]
        public void RetryPushCCToMainDB_WithErrorCode200_ThrowsLoggingException()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>
            {
                new LatestClassificationModel { CustId = 1, CustomerClass = 1, Category = 1 }
            };
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 3;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 200;
                        syncCriteria.ErrorMessage = "Non-existing customers";
                    }
                });

            // Act & Assert - ErrorCode 200 triggers logging
            var exception = Assert.Throws<NotImplementedException>(() => 
                _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime));
            
            Assert.Contains("Please set log engines before using", exception.Message);
        }

        [Fact]
        public void RetryPushCCToMainDB_WithPersistentError_ThrowsLoggingException()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>
            {
                new LatestClassificationModel { CustId = 1, CustomerClass = 1, Category = 1 }
            };
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 2;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        syncCriteria.ErrorCode = 500; // Persistent error
                        syncCriteria.ErrorMessage = "Database error";
                    }
                });

            // Act & Assert - Persistent error triggers logging when retryTime reaches 0
            var exception = Assert.Throws<NotImplementedException>(() => 
                _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime));
            
            Assert.Contains("Please set log engines before using", exception.Message);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Exactly(3)); // Initial + 2 retries
        }

        [Fact]
        public void RetryPushCCToMainDB_WithSuccessfulRetry_ReturnsTrue()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>
            {
                new LatestClassificationModel { CustId = 1, CustomerClass = 1, Category = 1 }
            };
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 2;

            var callCount = 0;
            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is SyncCustomerClassToMainDbCriteria syncCriteria)
                    {
                        callCount++;
                        // First call fails, second call succeeds
                        syncCriteria.ErrorCode = callCount == 1 ? 500 : 0;
                        syncCriteria.ErrorMessage = callCount == 1 ? "Temporary error" : string.Empty;
                    }
                });

            // Act
            var result = _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Exactly(2));
        }

        #endregion

        #region GetLatestCCAndDangerLevel Tests

        [Fact]
        public void GetLatestCCAndDangerLevel_WithValidCustIds_ReturnsData()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var expectedData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 1,
                    CategoryID = 1,
                    DangerLevel = 5,
                    DangerLevelType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(expectedData);

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedData[0].CustID, result.First().CustID);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Once);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WithNullCustIds_ReturnsEmptyList()
        {
            // Arrange
            IEnumerable<long> custIds = null;

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WithEmptyCustIds_ReturnsEmptyList()
        {
            // Arrange
            var custIds = new List<long>();

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WithValidCustIdsString_ReturnsData()
        {
            // Arrange
            var custIds = "1,2,3";
            var expectedData = new List<LatestCCDangerLevelModel>
            {
                new LatestCCDangerLevelModel
                {
                    CustID = 1,
                    CustomerClass = 1,
                    CategoryID = 1,
                    DangerLevel = 5,
                    DangerLevelType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()))
                .Returns(expectedData);

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedData[0].CustID, result.First().CustID);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Once);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WithNullCustIdsString_ReturnsEmptyList()
        {
            // Arrange
            string custIds = null;

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WithEmptyCustIdsString_ReturnsEmptyList()
        {
            // Arrange
            var custIds = string.Empty;

            // Act
            var result = _service.GetLatestCCAndDangerLevel(custIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<LatestCCDangerLevelModel>(It.IsAny<GetLastestCCDangerLevelCriteria>()), Times.Never);
        }

        #endregion

        #region UpdateDangerLevelToMainDB Tests

        [Fact]
        public void UpdateDangerLevelToMainDB_WithValidData_ReturnsSuccessResult()
        {
            // Arrange
            var dangerLevels = new List<DangerLevelModel>
            {
                new DangerLevelModel
                {
                    CustId = 1,
                    DangerLevel = 5
                }
            };
            var dangerType = 1;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is DangerLevelUpdateCriteria dangerCriteria)
                    {
                        dangerCriteria.ErrorCode = 0;
                        dangerCriteria.ErrorMessage = string.Empty;
                    }
                });

            // Act
            var result = _service.UpdateDangerLevelToMainDB(dangerLevels, dangerType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            Assert.Equal(string.Empty, result.ErrorMsg);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
        }

        [Fact]
        public void UpdateDangerLevelToMainDB_WithNullData_ReturnsSuccessResult()
        {
            // Arrange
            IEnumerable<DangerLevelModel> dangerLevels = null;
            var dangerType = 1;

            // Act
            var result = _service.UpdateDangerLevelToMainDB(dangerLevels, dangerType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            Assert.Equal(string.Empty, result.ErrorMsg);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Never);
        }

        [Fact]
        public void UpdateDangerLevelToMainDB_WithEmptyData_ReturnsSuccessResult()
        {
            // Arrange
            var dangerLevels = new List<DangerLevelModel>();
            var dangerType = 1;

            // Act
            var result = _service.UpdateDangerLevelToMainDB(dangerLevels, dangerType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            Assert.Equal(string.Empty, result.ErrorMsg);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Never);
        }

        [Fact]
        public void UpdateDangerLevelToMainDB_WithError_ReturnsErrorResult()
        {
            // Arrange
            var dangerLevels = new List<DangerLevelModel>
            {
                new DangerLevelModel { CustId = 1, DangerLevel = 5 }
            };
            var dangerType = 1;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0)
                .Callback<INonQueryCommand>(command => 
                {
                    if (command is DangerLevelUpdateCriteria dangerCriteria)
                    {
                        dangerCriteria.ErrorCode = 500;
                        dangerCriteria.ErrorMessage = "Database error";
                    }
                });

            // Act
            var result = _service.UpdateDangerLevelToMainDB(dangerLevels, dangerType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.ErrorCode);
            Assert.Equal("Database error", result.ErrorMsg);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
        }

        #endregion

        #region Exception Handling Tests

        [Fact]
        public void RetryPushCCToMainDB_WithException_ThrowsLoggingException()
        {
            // Arrange
            var customerClassifications = new List<LatestClassificationModel>
            {
                new LatestClassificationModel { CustId = 1, CustomerClass = 1, Category = 1 }
            };
            var roleGroupType = RoleGroupType.Member;
            var retryTime = 1;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Throws(new Exception("Database connection failed"));

            // Act & Assert - Exception handling triggers logging, so expect logging exception
            var exception = Assert.Throws<NotImplementedException>(() => 
                _service.RetryPushCCToMainDB(customerClassifications, roleGroupType, retryTime));
            
            Assert.Contains("Please set log engines before using", exception.Message);
            _mockRepository.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Exactly(2)); // Initial + 1 retry
        }

        #endregion

        #region Null Parameter Tests

        [Fact]
        public void PushDataToExternalPackge_WithNullParams_ThrowsNullReferenceException()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            PushDataToExternalModel paras = null;
            var fromFlow = FromFlow.CTSAPI;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.PushDataToExternalPackge(custIds, paras, fromFlow));
        }

        [Fact]
        public void PushDataToExternal_WithNullParams_ThrowsNullReferenceException()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            PushDataToExternalModel paras = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.PushDataToExternal(custIds, paras));
        }

        #endregion
    }
}
