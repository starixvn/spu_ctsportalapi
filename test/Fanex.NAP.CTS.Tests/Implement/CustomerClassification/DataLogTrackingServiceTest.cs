namespace Fanex.NAP.CTS.Tests.Implement.CustomerClassification
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.CustomerClassification;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Service;
    using Moq;

    public class DataLogTrackingServiceTest
    {
        private readonly Mock<ICriteriaService> _mockCriteriaService;
        private readonly DataLogTrackingService _service;

        public DataLogTrackingServiceTest()
        {
            _mockCriteriaService = new Mock<ICriteriaService>();
            _service = new DataLogTrackingService(_mockCriteriaService.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        [Fact]
        public void LogErrorData_WithExceptionAndServiceName_ShouldLogErrorData()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var serviceName = "TestService";
            var expectedLogInfo = new LogErrorDataModel
            {
                Message = It.IsAny<string>(),
                FromFlow = serviceName,
                SPName = null,
                Parameters = null
            };

            _mockCriteriaService
                .Setup(x => x.Execute(It.IsAny<InputErrorLogInsertCriteria>()))
                .Verifiable();

            // Act
            _service.LogErrorData(exception, serviceName);

            // Assert
            _mockCriteriaService.Verify(x => x.Execute(It.Is<InputErrorLogInsertCriteria>(
                criteria => criteria.LogInfo.Contains(serviceName))), Times.Once);
        }

        [Fact]
        public void LogErrorData_WithDbException_ShouldIncludeDbInfo()
        {
            // Arrange
            var exception = new Exception("DB Error");
            var serviceName = "TestService";

            _mockCriteriaService
                .Setup(x => x.Execute(It.IsAny<InputErrorLogInsertCriteria>()))
                .Verifiable();

            // Act
            _service.LogErrorData(exception, serviceName);

            // Assert
            _mockCriteriaService.Verify(x => x.Execute(It.Is<InputErrorLogInsertCriteria>(
                criteria => criteria.LogInfo.Contains("DB Error"))), Times.Once);
        }
    }
}
