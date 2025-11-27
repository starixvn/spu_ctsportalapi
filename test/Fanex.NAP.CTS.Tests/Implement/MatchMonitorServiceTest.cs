namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.ViewModel.MatchMonitor;
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
            LogExtension.ConfigByPassForUnitTest();
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

        [Fact]
        public void GetMMFraudTicketsSummary_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var model = new MMFraudTicketsSummaryModel
            {
                FraudTickets = "TICKET001,TICKET002,TICKET003"
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>
            {
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 123,
                    IsLive = true,
                    Bettype = 1,
                    Betteam = "Team A vs Team B",
                    BetId = 1001,
                    Turnover = 50000,
                    BetCount = 25
                },
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 456,
                    IsLive = false,
                    Bettype = 2,
                    Betteam = "Team C vs Team D",
                    BetId = 1002,
                    Turnover = 75000,
                    BetCount = 30
                }
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, result.Count());
            
            var firstTicket = result.First();
            var secondTicket = result.Skip(1).First();
            
            Assert.Equal(123, firstTicket.MatchId);
            Assert.True(firstTicket.IsLive);
            Assert.Equal(1, firstTicket.Bettype);
            Assert.Equal("Team A vs Team B", firstTicket.Betteam);
            Assert.Equal(1001, firstTicket.BetId);
            Assert.Equal(50000, firstTicket.Turnover);
            Assert.Equal(25, firstTicket.BetCount);
            
            Assert.Equal(456, secondTicket.MatchId);
            Assert.False(secondTicket.IsLive);
            Assert.Equal(2, secondTicket.Bettype);
            Assert.Equal("Team C vs Team D", secondTicket.Betteam);
            Assert.Equal(1002, secondTicket.BetId);
            Assert.Equal(75000, secondTicket.Turnover);
            Assert.Equal(30, secondTicket.BetCount);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMFraudTicketsSummary_WithEmptyFraudTickets_ReturnsEmptyResult()
        {
            // Arrange
            var model = new MMFraudTicketsSummaryModel
            {
                FraudTickets = ""
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>();

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMFraudTicketsSummary_WithNullFraudTickets_ReturnsEmptyResult()
        {
            // Arrange
            var model = new MMFraudTicketsSummaryModel
            {
                FraudTickets = null
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>();

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMFraudTicketsSummary_WithRepositoryException_ThrowsException()
        {
            // Arrange
            var model = new MMFraudTicketsSummaryModel
            {
                FraudTickets = "TICKET001"
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Throws(new System.Exception("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<System.Exception>(() => _service.GetMMFraudTicketsSummary(model));
            Assert.Equal("Database connection failed", exception.Message);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMFraudTicketsSummary_WithSingleTicket_ReturnsSingleResult()
        {
            // Arrange
            var model = new MMFraudTicketsSummaryModel
            {
                FraudTickets = "SINGLE_TICKET"
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>
            {
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 789,
                    IsLive = true,
                    Bettype = 3,
                    Betteam = "Single Match",
                    BetId = 1003,
                    Turnover = 25000,
                    BetCount = 15
                }
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            
            var ticket = result.First();
            Assert.Equal(789, ticket.MatchId);
            Assert.True(ticket.IsLive);
            Assert.Equal(3, ticket.Bettype);
            Assert.Equal("Single Match", ticket.Betteam);
            Assert.Equal(1003, ticket.BetId);
            Assert.Equal(25000, ticket.Turnover);
            Assert.Equal(15, ticket.BetCount);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = "PARLAY_TICKET001,PARLAY_TICKET002"
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>
            {
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 111,
                    IsLive = true,
                    Bettype = 4,
                    Betteam = "Parlay Match 1",
                    BetId = 2001,
                    Turnover = 100000,
                    BetCount = 50
                },
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 222,
                    IsLive = false,
                    Bettype = 5,
                    Betteam = "Parlay Match 2",
                    BetId = 2002,
                    Turnover = 150000,
                    BetCount = 75
                }
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMParlayFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(2, result.Count());
            
            var firstTicket = result.First();
            var secondTicket = result.Skip(1).First();
            
            Assert.Equal(111, firstTicket.MatchId);
            Assert.True(firstTicket.IsLive);
            Assert.Equal(4, firstTicket.Bettype);
            Assert.Equal("Parlay Match 1", firstTicket.Betteam);
            Assert.Equal(2001, firstTicket.BetId);
            Assert.Equal(100000, firstTicket.Turnover);
            Assert.Equal(50, firstTicket.BetCount);
            
            Assert.Equal(222, secondTicket.MatchId);
            Assert.False(secondTicket.IsLive);
            Assert.Equal(5, secondTicket.Bettype);
            Assert.Equal("Parlay Match 2", secondTicket.Betteam);
            Assert.Equal(2002, secondTicket.BetId);
            Assert.Equal(150000, secondTicket.Turnover);
            Assert.Equal(75, secondTicket.BetCount);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithEmptyFraudTickets_ReturnsEmptyResult()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = ""
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>();

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMParlayFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithNullFraudTickets_ReturnsEmptyResult()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = null
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>();

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMParlayFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithRepositoryException_ThrowsException()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = "PARLAY_TICKET001"
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Throws(new System.Exception("Database connection failed"));

            // Act & Assert
            var exception = Assert.Throws<System.Exception>(() => _service.GetMMParlayFraudTicketsSummary(model));
            Assert.Equal("Database connection failed", exception.Message);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithSingleTicket_ReturnsSingleResult()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = "SINGLE_PARLAY_TICKET"
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>
            {
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 333,
                    IsLive = true,
                    Bettype = 6,
                    Betteam = "Single Parlay Match",
                    BetId = 2003,
                    Turnover = 80000,
                    BetCount = 40
                }
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMParlayFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            
            var ticket = result.First();
            Assert.Equal(333, ticket.MatchId);
            Assert.True(ticket.IsLive);
            Assert.Equal(6, ticket.Bettype);
            Assert.Equal("Single Parlay Match", ticket.Betteam);
            Assert.Equal(2003, ticket.BetId);
            Assert.Equal(80000, ticket.Turnover);
            Assert.Equal(40, ticket.BetCount);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void GetMMParlayFraudTicketsSummary_WithMultipleTickets_ReturnsAllResults()
        {
            // Arrange
            var model = new MMParlayFraudTicketsSummaryModel
            {
                FraudTickets = "PARLAY_TICKET001,PARLAY_TICKET002,PARLAY_TICKET003,PARLAY_TICKET004"
            };

            var expectedResult = new List<MMFraudTicketsSummaryViewModel>
            {
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 401,
                    IsLive = true,
                    Bettype = 7,
                    Betteam = "Parlay Match 1",
                    BetId = 2004,
                    Turnover = 120000,
                    BetCount = 60
                },
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 402,
                    IsLive = false,
                    Bettype = 8,
                    Betteam = "Parlay Match 2",
                    BetId = 2005,
                    Turnover = 180000,
                    BetCount = 90
                },
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 403,
                    IsLive = true,
                    Bettype = 9,
                    Betteam = "Parlay Match 3",
                    BetId = 2006,
                    Turnover = 95000,
                    BetCount = 45
                },
                new MMFraudTicketsSummaryViewModel
                {
                    MatchId = 404,
                    IsLive = false,
                    Bettype = 10,
                    Betteam = "Parlay Match 4",
                    BetId = 2007,
                    Turnover = 200000,
                    BetCount = 100
                }
            };

            _mockRepository.Setup(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetMMParlayFraudTicketsSummary(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            
            var tickets = result.ToList();
            
            Assert.Equal(401, tickets[0].MatchId);
            Assert.Equal("Parlay Match 1", tickets[0].Betteam);
            Assert.Equal(120000, tickets[0].Turnover);
            
            Assert.Equal(402, tickets[1].MatchId);
            Assert.Equal("Parlay Match 2", tickets[1].Betteam);
            Assert.Equal(180000, tickets[1].Turnover);
            
            Assert.Equal(403, tickets[2].MatchId);
            Assert.Equal("Parlay Match 3", tickets[2].Betteam);
            Assert.Equal(95000, tickets[2].Turnover);
            
            Assert.Equal(404, tickets[3].MatchId);
            Assert.Equal("Parlay Match 4", tickets[3].Betteam);
            Assert.Equal(200000, tickets[3].Turnover);
            
            _mockRepository.Verify(x => x.Fetch<MMFraudTicketsSummaryViewModel>(It.IsAny<ICriteria>()), Times.Once);
        }
    }
}
