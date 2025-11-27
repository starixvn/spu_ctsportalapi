namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.TW;
    using Fanex.NAP.CTS.Model.API.TWInitialSmart;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class TWInitialSmartServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly TWInitialSmartService _service;

        public TWInitialSmartServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new TWInitialSmartService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
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

        #region AddCustTWInitialSmartsBySport

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 12345,
                    SportType = 1,
                    Probability = 0.85m,
                    PredictTime = DateTime.Now
                },
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 67890,
                    SportType = 2,
                    Probability = 0.92m,
                    PredictTime = DateTime.Now.AddHours(-1)
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = 12345, SportType = 1 },
                new IntialSmartCustInfoBySportEntity { CustID = 67890, SportType = 2 }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithEmptyInput_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<InitialSmartCustInfoBySportModel>();
            var expectedResult = new List<IntialSmartCustInfoBySportEntity>();

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(emptyList);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithNullInput_ReturnsEmptyList()
        {
            // Arrange
            IEnumerable<InitialSmartCustInfoBySportModel> nullInput = null;
            var expectedResult = new List<IntialSmartCustInfoBySportEntity>();

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(nullInput);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Theory]
        [InlineData(891723, 0)]
        [InlineData(123121, 1)]
        [InlineData(123541, 2)]
        public void AddCustTWInitialSmartsBySport_WithSingleItem_ReturnsSingleResult(long custId, int sportType)
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = custId,
                    SportType = sportType,
                    Probability = 0.75m,
                    PredictTime = DateTime.Now.AddDays(-1)
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = custId, SportType = sportType }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(custId, result.First().CustID);
            Assert.Equal(sportType, result.First().SportType);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithMultipleItems_ReturnsMultipleResults()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 11111,
                    SportType = 1,
                    Probability = 0.60m,
                    PredictTime = DateTime.Now.AddMinutes(-30)
                },
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 22222,
                    SportType = 2,
                    Probability = 0.80m,
                    PredictTime = DateTime.Now.AddMinutes(-60)
                },
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 33333,
                    SportType = 3,
                    Probability = 0.95m,
                    PredictTime = DateTime.Now.AddMinutes(-90)
                },
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 44444,
                    SportType = 1,
                    Probability = 0.70m,
                    PredictTime = DateTime.Now.AddMinutes(-120)
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = 11111, SportType = 1 },
                new IntialSmartCustInfoBySportEntity { CustID = 22222, SportType = 2 },
                new IntialSmartCustInfoBySportEntity { CustID = 33333, SportType = 3 },
                new IntialSmartCustInfoBySportEntity { CustID = 44444, SportType = 1 }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());

            var resultList = result.ToList();
            Assert.Equal(11111, resultList[0].CustID);
            Assert.Equal(1, resultList[0].SportType);
            Assert.Equal(22222, resultList[1].CustID);
            Assert.Equal(2, resultList[1].SportType);
            Assert.Equal(33333, resultList[2].CustID);
            Assert.Equal(3, resultList[2].SportType);
            Assert.Equal(44444, resultList[3].CustID);
            Assert.Equal(1, resultList[3].SportType);

            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithZeroValues_HandlesCorrectly()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 0,
                    SportType = 0,
                    Probability = 0m,
                    PredictTime = DateTime.MinValue
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = 0, SportType = 0 }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(0, result.First().CustID);
            Assert.Equal(0, result.First().SportType);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithNegativeValues_HandlesCorrectly()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = -1,
                    SportType = -5,
                    Probability = -0.1m,
                    PredictTime = DateTime.Now
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = -1, SportType = -5 }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(-1, result.First().CustID);
            Assert.Equal(-5, result.First().SportType);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_WithMaxValues_HandlesCorrectly()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = long.MaxValue,
                    SportType = int.MaxValue,
                    Probability = decimal.MaxValue,
                    PredictTime = DateTime.MaxValue
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = long.MaxValue, SportType = int.MaxValue }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(long.MaxValue, result.First().CustID);
            Assert.Equal(int.MaxValue, result.First().SportType);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 12345,
                    SportType = 1,
                    Probability = 0.85m,
                    PredictTime = DateTime.Now
                }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Throws(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos));

            Assert.Equal("Database connection failed", exception.Message);
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void AddCustTWInitialSmartsBySport_VerifyCriteriaProperties_AreSetCorrectly()
        {
            // Arrange
            var initialSmartCustInfos = new List<InitialSmartCustInfoBySportModel>
            {
                new InitialSmartCustInfoBySportModel
                {
                    CustID = 12345,
                    SportType = 1,
                    Probability = 0.85m,
                    PredictTime = DateTime.Now
                }
            };

            var expectedResult = new List<IntialSmartCustInfoBySportEntity>
            {
                new IntialSmartCustInfoBySportEntity { CustID = 12345, SportType = 1 }
            };

            _mockRepository.Setup(x => x.Fetch<IntialSmartCustInfoBySportEntity>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.AddCustTWInitialSmartsBySport(initialSmartCustInfos);

            // Assert
            _mockRepository.Verify(x => x.Fetch<IntialSmartCustInfoBySportEntity>(
                It.Is<InsertTWInitialSmartBySportCriteria>(criteria =>
                    !string.IsNullOrEmpty(criteria.CustInfoJson) &&
                    criteria.CustInfoJson.Contains("12345") &&
                    criteria.CustInfoJson.Contains("1") &&
                    criteria.CustInfoJson.Contains("0.85")
                )), Times.Once);
        }

        #endregion
    }
}