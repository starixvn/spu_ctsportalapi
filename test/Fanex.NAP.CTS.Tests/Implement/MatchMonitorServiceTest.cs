namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.Data.Repository;
    using Moq;
    using System.Collections.Generic;
    using Xunit;
    using Fanex.NAP.CTS.ViewModel.MatchMonitor;

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

            var expectedResult = new List<MatchFromMainInfoViewModel>
            {
                new MatchFromMainInfoViewModel
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
                new MatchFromMainInfoViewModel
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

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, result.Count());
            
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()), Times.Once);
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

            var expectedResult = new List<MatchFromMainInfoViewModel>
            {
                new MatchFromMainInfoViewModel
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

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()), Times.Once);
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

            var expectedResult = new List<MatchFromMainInfoViewModel>();

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchFromMainInfos(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()), Times.Once);
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

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()))
                .Throws(new System.Exception("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<System.Exception>(() => _service.GetMatchFromMainInfos(model));
            Assert.Equal("Database connection failed", exception.Message);
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()), Times.Once);
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

            var expectedResult = new List<MatchFromMainInfoViewModel>
            {
                new MatchFromMainInfoViewModel
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

            _mockRepository.Setup(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()))
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
            _mockRepository.Verify(x => x.Fetch<MatchFromMainInfoViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                SportType = "Football",
                Market = 1,
                BetTypes = "1,2,3",
                IsVerified = 1,
                Reasons = "Suspicious activity",
                MatchInfosJson = "[{\"matchId\":123,\"eventDate\":\"2024-01-15\"}]"
            };

            var expectedResult = new List<GetMatchMonitorViewModel>
            {
                new GetMatchMonitorViewModel
                {
                    ID = 1,
                    MatchID = 123,
                    EventDate = new DateTime(2024, 1, 15),
                    Market = 1,
                    HomeID = 10,
                    HomeName = "Team A",
                    AwayID = 20,
                    AwayName = "Team B",
                    LeagueID = 100,
                    LeagueName = "Premier League",
                    SportTypeId = 1,
                    BettypeID = 1,
                    BetID = 1001,
                    Reason = "Suspicious activity",
                    MatchSection = 1,
                    IsVerified = true,
                    VerifiedBy = "Admin",
                    KickOffTime = new DateTime(2024, 1, 15, 20, 0, 0),
                    IsSupportShowAllTickets = true,
                    FraudTrans = "TRX001",
                    FraudRefno = "REF001",
                    DataSource = 1,
                    BettypeNameDisplay = "1X2",
                    DetailsX = 1,
                    DetailsY = 2,
                    TicketType = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchMonitor(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Single(result);
            
            var match = result.First();
            Assert.Equal(1, match.ID);
            Assert.Equal(123, match.MatchID);
            Assert.Equal("Team A", match.HomeName);
            Assert.Equal("Team B", match.AwayName);
            Assert.Equal("Premier League", match.LeagueName);
            Assert.True(match.IsVerified);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithNullDates_UsesCurrentDateTime()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = null,
                ToDate = null,
                SportType = "Basketball",
                Market = 2,
                BetTypes = "4,5",
                IsVerified = 0,
                Reasons = "High odds",
                MatchInfosJson = null
            };

            var expectedResult = new List<GetMatchMonitorViewModel>
            {
                new GetMatchMonitorViewModel
                {
                    ID = 2,
                    MatchID = 456,
                    EventDate = DateTime.Now.Date,
                    Market = 2,
                    HomeID = 30,
                    HomeName = "Basketball Team A",
                    AwayID = 40,
                    AwayName = "Basketball Team B",
                    LeagueID = 200,
                    LeagueName = "NBA",
                    SportTypeId = 2,
                    BettypeID = 4,
                    Reason = "High odds",
                    MatchSection = 2,
                    IsVerified = false,
                    KickOffTime = DateTime.Now,
                    IsSupportShowAllTickets = false,
                    DataSource = 2,
                    BettypeNameDisplay = "Handicap",
                    DetailsX = 3,
                    DetailsY = 4,
                    TicketType = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchMonitor(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var match = result.First();
            Assert.Equal(456, match.MatchID);
            Assert.Equal("Basketball Team A", match.HomeName);
            Assert.Equal("Basketball Team B", match.AwayName);
            Assert.Equal("NBA", match.LeagueName);
            Assert.False(match.IsVerified);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithEmptySportTypeAndBetTypes_UsesEmptyArrays()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                SportType = "",
                Market = 1,
                BetTypes = "",
                IsVerified = 1,
                Reasons = "Test reason",
                MatchInfosJson = "[]"
            };

            var expectedResult = new List<GetMatchMonitorViewModel>();

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchMonitor(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithNullSportTypeAndBetTypes_UsesEmptyArrays()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                SportType = null,
                Market = 1,
                BetTypes = null,
                IsVerified = 1,
                Reasons = "Test reason",
                MatchInfosJson = "[]"
            };

            var expectedResult = new List<GetMatchMonitorViewModel>();

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchMonitor(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithRepositoryException_ThrowsException()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                SportType = "Football",
                Market = 1,
                BetTypes = "1,2",
                IsVerified = 1,
                Reasons = "Test reason",
                MatchInfosJson = "[]"
            };

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Throws(new System.Exception("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<System.Exception>(() => _service.GetMatchMonitor(model));
            Assert.Equal("Database connection failed", exception.Message);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMatchMonitor_WithMultipleResults_ReturnsAllResults()
        {
            // Arrange
            var model = new MatchMonitorGetModel
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 31),
                SportType = "Football",
                Market = 1,
                BetTypes = "1,2,3",
                IsVerified = 1,
                Reasons = "Multiple matches",
                MatchInfosJson = "[{\"matchId\":123},{\"matchId\":456}]"
            };

            var expectedResult = new List<GetMatchMonitorViewModel>
            {
                new GetMatchMonitorViewModel
                {
                    ID = 1,
                    MatchID = 123,
                    EventDate = new DateTime(2024, 1, 15),
                    Market = 1,
                    HomeID = 10,
                    HomeName = "Team A",
                    AwayID = 20,
                    AwayName = "Team B",
                    LeagueID = 100,
                    LeagueName = "League 1",
                    SportTypeId = 1,
                    BettypeID = 1,
                    Reason = "Multiple matches",
                    MatchSection = 1,
                    IsVerified = true,
                    KickOffTime = new DateTime(2024, 1, 15, 20, 0, 0),
                    DataSource = 1,
                    BettypeNameDisplay = "1X2",
                    DetailsX = 1,
                    DetailsY = 2,
                    TicketType = 1
                },
                new GetMatchMonitorViewModel
                {
                    ID = 2,
                    MatchID = 456,
                    EventDate = new DateTime(2024, 1, 16),
                    Market = 1,
                    HomeID = 30,
                    HomeName = "Team C",
                    AwayID = 40,
                    AwayName = "Team D",
                    LeagueID = 200,
                    LeagueName = "League 2",
                    SportTypeId = 1,
                    BettypeID = 2,
                    Reason = "Multiple matches",
                    MatchSection = 2,
                    IsVerified = true,
                    KickOffTime = new DateTime(2024, 1, 16, 19, 30, 0),
                    DataSource = 1,
                    BettypeNameDisplay = "Over/Under",
                    DetailsX = 3,
                    DetailsY = 4,
                    TicketType = 2
                }
            };

            _mockRepository.Setup(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMatchMonitor(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            
            var firstMatch = result.First();
            var secondMatch = result.Skip(1).First();
            
            Assert.Equal(123, firstMatch.MatchID);
            Assert.Equal("Team A", firstMatch.HomeName);
            Assert.Equal("Team B", firstMatch.AwayName);
            Assert.Equal("League 1", firstMatch.LeagueName);
            
            Assert.Equal(456, secondMatch.MatchID);
            Assert.Equal("Team C", secondMatch.HomeName);
            Assert.Equal("Team D", secondMatch.AwayName);
            Assert.Equal("League 2", secondMatch.LeagueName);
            
            _mockRepository.Verify(x => x.Fetch<GetMatchMonitorViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
}
