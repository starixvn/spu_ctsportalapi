namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.API.TWInitialSmart;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    public class TWInitialSmartServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly TWInitialSmartService _service;

        public TWInitialSmartServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new TWInitialSmartService(_mockRepository.Object);
        }

        [Fact]
        public void AddCustTWInitialSmarts_WithValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoModel>
            {
                new InitialSmartCustInfoModel { /* Set required properties */ }
            };
            var expectedResult = new List<long> { 1, 2, 3 };

            _mockRepository.Setup(x => x.Fetch<long>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmarts(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<long>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmarts_WithEmptyInput_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<InitialSmartCustInfoModel>();
            var expectedResult = new List<long>();

            _mockRepository.Setup(x => x.Fetch<long>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmarts(emptyList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<long>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
} 