namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.CtsAssociationAPI;
    using Fanex.NAP.CTS.Model.API;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;

    public class CtsAssociationApiServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly CtsAssociationApiService _service;

        public CtsAssociationApiServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new CtsAssociationApiService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        [Fact]
        public void DetectAssociationByCustId_WithInvalidCustId_ReturnsEmptyList()
        {
            // Arrange
            long invalidCustId = 0;

            // Act
            var result = _service.DetectAssociationByCustId(invalidCustId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void DetectAssociationByCustId_WithValidCustId_ReturnsAllAssociations()
        {
            // Arrange
            long validCustId = 123;
            var deviceAssociations = new List<DetectAssociationByTypeModel>
            {
                new DetectAssociationByTypeModel { /* Set required properties */ }
            };
            var ipAssociations = new List<DetectAssociationByTypeModel>
            {
                new DetectAssociationByTypeModel { /* Set required properties */ }
            };
            var aiAssociations = new List<DetectAssociationByTypeModel>
            {
                new DetectAssociationByTypeModel { /* Set required properties */ }
            };

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByTypeModel>(It.Is<DetectAssociationByTypeCriteria>(
                c => c.CustID == validCustId && c.IsDevice)))
                .Returns(deviceAssociations);

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByTypeModel>(It.Is<DetectAssociationByTypeCriteria>(
                c => c.CustID == validCustId && c.IsIP)))
                .Returns(ipAssociations);

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByTypeModel>(It.Is<DetectAssociationByTypeCriteria>(
                c => c.CustID == validCustId && c.IsAI)))
                .Returns(aiAssociations);

            // Act
            var result = _service.DetectAssociationByCustId(validCustId).ToList();

            // Assert
            Assert.Equal(deviceAssociations.Count + ipAssociations.Count + aiAssociations.Count, result.Count);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByTypeModel>(It.IsAny<DetectAssociationByTypeCriteria>()), Times.Exactly(3));
        }

        [Fact]
        public void DetectAssociationByCustId_WithNoAssociations_ReturnsEmptyList()
        {
            // Arrange
            long validCustId = 123;
            var emptyList = new List<DetectAssociationByTypeModel>();

            _mockRepository.Setup(x => x.Fetch<DetectAssociationByTypeModel>(It.IsAny<DetectAssociationByTypeCriteria>()))
                .Returns(emptyList);

            // Act
            var result = _service.DetectAssociationByCustId(validCustId).ToList();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<DetectAssociationByTypeModel>(It.IsAny<DetectAssociationByTypeCriteria>()), Times.Exactly(3));
        }
    }
} 