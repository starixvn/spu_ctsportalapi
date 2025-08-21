namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.Data.Repository;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    public class MatchMonitorServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly MatchMonitorService _service;

        public MatchMonitorServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new MatchMonitorService(_mockRepository.Object);
        }

        [Fact]
        public void GetMatchFromMainInfos_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var model = new GetMatchFromMainInfoModel
            {
                MatchIDs = "123,456,789",
                SportList = "1,2,3",
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                MatchSection = "Live",
                Event = "Test Event"
            };

            var expectedResult = new List<MatchFromMainInfoModel>
            {
                new MatchFromMainInfoModel
                {
                    MatchId = 123,
                    EventDate = new DateTime(2024, 1, 15),
                    KickOffTime = new DateTime(2024, 1, 15, 20, 0, 0),
                    MatchSection = 1,
                    IsGetDataFromOrigin = true,
                    DataSource = 1,
                    LeagueName = "Premier League",
                    HomeName = "Team A",
                    AwayName = "Team B"
                },
                new MatchFromMainInfoModel
                {
                    MatchId = 456,
                    EventDate = new DateTime(2024, 1, 16),
                    KickOffTime = new DateTime(2024, 1, 16, 19, 30, 0),
                    MatchSection = 2,
                    IsGetDataFromOrigin = false,
                    DataSource = 2,
                    LeagueName = "La Liga",
                    HomeName = "Team C",
                    AwayName = "Team D"
                }
            };

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, result.Count());
            
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchFromMainInfos_WithNullDates_UsesCurrentDateTime()
        {
            // Arrange
            var model = new GetMatchFromMainInfoModel
            {
                MatchIDs = "123",
                SportList = "1",
                FromDate = null,
                ToDate = null,
                MatchSection = "Today",
                Event = "Test Event"
            };

            var expectedResult = new List<MatchFromMainInfoModel>
            {
                new MatchFromMainInfoModel
                {
                    MatchId = 123,
                    EventDate = DateTime.Now.Date,
                    KickOffTime = DateTime.Now,
                    MatchSection = 2,
                    IsGetDataFromOrigin = true,
                    DataSource = 1,
                    LeagueName = "Test League",
                    HomeName = "Home Team",
                    AwayName = "Away Team"
                }
            };

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchFromMainInfos_WithEmptyParameters_ReturnsExpectedResult()
        {
            // Arrange
            var model = new GetMatchFromMainInfoModel
            {
                MatchIDs = "",
                SportList = "",
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                MatchSection = "",
                Event = ""
            };

            var expectedResult = new List<MatchFromMainInfoModel>();

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchFromMainInfos_WithRepositoryException_ThrowsException()
        {
            // Arrange
            var model = new GetMatchFromMainInfoModel
            {
                MatchIDs = "123",
                SportList = "1",
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                MatchSection = "Live",
                Event = "Test Event"
            };

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()))
                .Throws(new System.Exception("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<System.Exception>(() => _service.GetMatchFromMainInfos(model));
            Assert.Equal("Database connection failed", exception.Message);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchFromMainInfos_WithSingleMatch_ReturnsSingleResult()
        {
            // Arrange
            var model = new GetMatchFromMainInfoModel
            {
                MatchIDs = "123",
                SportList = "1",
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                MatchSection = "Early",
                Event = "Single Match Event"
            };

            var expectedResult = new List<MatchFromMainInfoModel>
            {
                new MatchFromMainInfoModel
                {
                    MatchId = 123,
                    EventDate = new DateTime(2024, 1, 15),
                    KickOffTime = new DateTime(2024, 1, 15, 15, 0, 0),
                    MatchSection = 3,
                    IsGetDataFromOrigin = true,
                    DataSource = 1,
                    LeagueName = "Single League",
                    HomeName = "Single Home",
                    AwayName = "Single Away"
                }
            };

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var match = result.First();
            Assert.Equal(123, match.MatchId);
            Assert.Equal("Single League", match.LeagueName);
            Assert.Equal("Single Home", match.HomeName);
            Assert.Equal("Single Away", match.AwayName);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoModel>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
}
