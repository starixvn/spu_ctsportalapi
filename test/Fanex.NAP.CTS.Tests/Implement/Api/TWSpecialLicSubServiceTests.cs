namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.API.TWSpecialLicSub;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    public class TWSpecialLicSubServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly TWSpecialLicSubService _service;

        public TWSpecialLicSubServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new TWSpecialLicSubService(_mockRepository.Object);
        }

        [Fact]
        public void InsertTWSpecialLicSub_WithValidModel_ReturnsExpectedResult()
        {
            // Arrange
            var model = new ForceTWLicSubCCModel
            {
                custid = 12345,        // int
                customerClass = 1       // int
            };

            var expectedResult = new InsertTWSpecialLicSubEntity
            {
                ID = 1,                // int
                InsertedTime = System.DateTime.UtcNow  // DateTime
            };

            _mockRepository.Setup(x => x.Fetch<InsertTWSpecialLicSubEntity>(It.IsAny<ICriteria>()))
                .Returns(new List<InsertTWSpecialLicSubEntity> { expectedResult });

            // Act
            var result = _service.InsertTWSpecialLicSub(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.ID, result.ID);
            Assert.Equal(expectedResult.InsertedTime, result.InsertedTime);
            _mockRepository.Verify(x => x.Fetch<InsertTWSpecialLicSubEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void InsertTWSpecialLicSub_WithNullModel_ReturnsEmptyEntity()
        {
            // Arrange
            ForceTWLicSubCCModel model = new ForceTWLicSubCCModel();

            _mockRepository.Setup(x => x.Fetch<InsertTWSpecialLicSubEntity>(It.IsAny<ICriteria>()))
                .Returns((List<InsertTWSpecialLicSubEntity>)null);

            // Act
            var result = _service.InsertTWSpecialLicSub(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ID);
            Assert.Equal(default(DateTime), result.InsertedTime);
            _mockRepository.Verify(x => x.Fetch<InsertTWSpecialLicSubEntity>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
}