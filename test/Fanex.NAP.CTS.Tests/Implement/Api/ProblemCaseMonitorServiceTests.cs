namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.API.ProblemCaseMonitor;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    public class ProblemCaseMonitorServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly ProblemCaseMonitorService _service;

        public ProblemCaseMonitorServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new ProblemCaseMonitorService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        [Fact]
        public void GetProlemCase_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            int listId = 1;
            bool status = true;
            var expectedResult = new List<ProblemCaseModel>
            {
                new ProblemCaseModel { /* Set required properties */ }
            };

            _mockRepository.Setup(x => x.Fetch<ProblemCaseModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetProlemCase(listId, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<ProblemCaseModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetProlemCase_WithNoResults_ReturnsEmptyList()
        {
            // Arrange
            int listId = 1;
            bool status = true;
            var emptyList = new List<ProblemCaseModel>();

            _mockRepository.Setup(x => x.Fetch<ProblemCaseModel>(It.IsAny<ICriteria>()))
                .Returns(emptyList);

            // Act
            var result = _service.GetProlemCase(listId, status);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<ProblemCaseModel>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
} 