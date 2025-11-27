namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.MonitorFunction;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.Service.Interface;
    using Moq;
    using Xunit;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    public class MonitorCtsFunctionServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly MonitorCtsFunctionService _service;

        public MonitorCtsFunctionServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new MonitorCtsFunctionService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        #region StatsAccessmentInsert Tests

        [Fact]
        public void StatsAccessmentInsert_WithValidParameters_ReturnsSuccessResult()
        {
            // Arrange
            var systemId = SystemId.CTSWeb;
            var funcId = TrackingCtsFunctionId.ProblemAccountMonitor;
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            Assert.Null(result.ErrorMsg);
            _mockRepository.Verify(x => x.Execute(It.Is<StatsAccessmentInsertCriteria>(c =>
                c.SystemId == (long)systemId &&
                c.FunctionId == (int)funcId &&
                c.UserId == userId)), Times.Once);
        }

        [Fact]
        public void StatsAccessmentInsert_WithAllSystemIdValues_ExecutesSuccessfully()
        {
            // Arrange
            var funcId = TrackingCtsFunctionId.ProblemAccountMonitor;
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act & Assert
            var result1 = _service.StatsAccessmentInsert(SystemId.CTSWeb, funcId, userId);
            var result2 = _service.StatsAccessmentInsert(SystemId.CTSAPI, funcId, userId);
            var result3 = _service.StatsAccessmentInsert(SystemId.FS, funcId, userId);
            var result4 = _service.StatsAccessmentInsert(SystemId.TGRS, funcId, userId);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.NotNull(result4);
            _mockRepository.Verify(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()), Times.Exactly(4));
        }

        [Fact]
        public void StatsAccessmentInsert_WithAllTrackingCtsFunctionIdValues_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.CTSWeb;
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act & Assert
            var result1 = _service.StatsAccessmentInsert(systemId, TrackingCtsFunctionId.ProblemAccountMonitor, userId);
            var result2 = _service.StatsAccessmentInsert(systemId, TrackingCtsFunctionId.EvidenceManagementImport, userId);
            var result3 = _service.StatsAccessmentInsert(systemId, TrackingCtsFunctionId.EvidenceManagementRemove, userId);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            _mockRepository.Verify(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()), Times.Exactly(3));
        }

        [Fact]
        public void StatsAccessmentInsert_WithZeroUserId_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.CTSAPI;
            var funcId = TrackingCtsFunctionId.AssociatedAccountMonitorRpt;
            var userId = 0;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            _mockRepository.Verify(x => x.Execute(It.Is<StatsAccessmentInsertCriteria>(c =>
                c.UserId == userId)), Times.Once);
        }

        [Fact]
        public void StatsAccessmentInsert_WithNegativeUserId_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.CTSAPI;
            var funcId = TrackingCtsFunctionId.SpecialCustomerClassRpt;
            var userId = -123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            _mockRepository.Verify(x => x.Execute(It.Is<StatsAccessmentInsertCriteria>(c =>
                c.UserId == userId)), Times.Once);
        }

        [Fact]
        public void StatsAccessmentInsert_WithMaxIntUserId_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.TGRS;
            var funcId = TrackingCtsFunctionId.ProbationManagement;
            var userId = int.MaxValue;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            _mockRepository.Verify(x => x.Execute(It.Is<StatsAccessmentInsertCriteria>(c =>
                c.UserId == userId)), Times.Once);
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [Fact]
        public void StatsAccessmentInsert_WithMinIntUserId_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.FS;
            var funcId = TrackingCtsFunctionId.AssociationDetailRpt;
            var userId = int.MinValue;

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
        }

        [Fact]
        public void StatsAccessmentInsert_WithLargeFunctionId_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.CTSWeb;
            var funcId = (TrackingCtsFunctionId)999; // Large function ID
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result = _service.StatsAccessmentInsert(systemId, funcId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            _mockRepository.Verify(x => x.Execute(It.Is<StatsAccessmentInsertCriteria>(c =>
                c.FunctionId == (int)funcId)), Times.Once);
        }

        #endregion

        #region Service Lifecycle Tests

        [Fact]
        public void MonitorCtsFunctionService_CanBeInstantiated()
        {
            // Arrange & Act
            var service = new MonitorCtsFunctionService(_mockRepository.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void MonitorCtsFunctionService_ImplementsIMonitorCtsFunctionService()
        {
            // Arrange
            var service = new MonitorCtsFunctionService(_mockRepository.Object);

            // Act & Assert
            Assert.IsAssignableFrom<IMonitorCtsFunctionService>(service);
        }

        #endregion

        #region Integration Style Tests

        [Fact]
        public void MultipleStatsAccessmentInsertCalls_ExecutesSuccessfully()
        {
            // Arrange
            var systemId = SystemId.CTSWeb;
            var funcId = TrackingCtsFunctionId.ProblemAccountMonitor;
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result1 = _service.StatsAccessmentInsert(systemId, funcId, userId);
            var result2 = _service.StatsAccessmentInsert(systemId, funcId, userId + 1);
            var result3 = _service.StatsAccessmentInsert(systemId, funcId, userId + 2);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            _mockRepository.Verify(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()), Times.Exactly(3));
        }

        [Fact]
        public void StatsAccessmentInsert_WithDifferentSystemIds_ExecutesSuccessfully()
        {
            // Arrange
            var funcId = TrackingCtsFunctionId.EvidenceManagementImport;
            var userId = 123;

            _mockRepository.Setup(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()))
                .Returns(0);

            // Act
            var result1 = _service.StatsAccessmentInsert(SystemId.CTSWeb, funcId, userId);
            var result2 = _service.StatsAccessmentInsert(SystemId.CTSAPI, funcId, userId);
            var result3 = _service.StatsAccessmentInsert(SystemId.FS, funcId, userId);
            var result4 = _service.StatsAccessmentInsert(SystemId.TGRS, funcId, userId);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.NotNull(result4);
            _mockRepository.Verify(x => x.Execute(It.IsAny<StatsAccessmentInsertCriteria>()), Times.Exactly(4));
        }

        #endregion
    }
}
