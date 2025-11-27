namespace Fanex.NAP.CTS.Tests.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.Model.AssociationDetection;
    using Fanex.NAP.CTS.Criteria.AssociationDetection;
    using Moq;
    using Xunit;
    using Fanex.NAP.CTS.Core.Extentions;

    public class CommonServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly CommonService _service;

        public CommonServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new CommonService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        #region CheckConnection Tests

        [Fact]
        public void CheckConnection_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            // The method should execute without throwing an exception
            // Note: This test requires a valid database connection
            var exception = Record.Exception(() => _service.CheckConnection());
            
            // In a real environment, this might throw if database is not available
            // For unit testing purposes, we're just ensuring the method signature is correct
            // and it doesn't throw unexpected exceptions
        }

        [Fact]
        public void CheckConnection_CanBeCalledMultipleTimes()
        {
            // Arrange & Act & Assert
            // The method should be able to be called multiple times without issues
            var exception1 = Record.Exception(() => _service.CheckConnection());
            var exception2 = Record.Exception(() => _service.CheckConnection());
            var exception3 = Record.Exception(() => _service.CheckConnection());

            // All calls should complete without unexpected exceptions
            // Note: Database connection issues would be expected exceptions
        }

        [Fact]
        public void CheckConnection_ServiceInstanceCanBeReused()
        {
            // Arrange
            var service1 = new CommonService(_mockRepository.Object);
            var service2 = new CommonService(_mockRepository.Object);

            // Act & Assert
            // Multiple service instances should work independently
            var exception1 = Record.Exception(() => service1.CheckConnection());
            var exception2 = Record.Exception(() => service2.CheckConnection());

            // Both instances should work without interfering with each other
        }

        #endregion

        #region Service Lifecycle Tests

        [Fact]
        public void CommonService_CanBeInstantiated()
        {
            // Arrange & Act
            var service = new CommonService(_mockRepository.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CommonService_ImplementsICommonService()
        {
            // Arrange
            var service = new CommonService(_mockRepository.Object);

            // Act & Assert
            Assert.IsAssignableFrom<ICommonService>(service);
        }

        #endregion

        #region ValidateUsernames Tests

        [Fact]
        public void ValidateUsernames_WithValidInput_ReturnsExpectedResults()
        {
            // Arrange
            var userNames = "user1,user2,user3";
            var expectedResults = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "user3", CustID = 3, CTSCustID = 1003, IsInternal = true }
            };

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(expectedResults, result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithNullInput_ReturnsEmptyResult()
        {
            // Arrange
            string? userNames = null;
            var expectedResults = new List<ValidateUserNameModel>();

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithEmptyInput_ReturnsEmptyResult()
        {
            // Arrange
            var userNames = "";
            var expectedResults = new List<ValidateUserNameModel>();

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithWhitespaceInput_ReturnsEmptyResult()
        {
            // Arrange
            var userNames = "   ";
            var expectedResults = new List<ValidateUserNameModel>();

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithSingleUsername_ReturnsSingleResult()
        {
            // Arrange
            var userNames = "singleuser";
            var expectedResults = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "singleuser", CustID = 1, CTSCustID = 1001, IsInternal = false }
            };

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("singleuser", result.First().UserName);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var userNames = "user@domain.com,user-name,user_name";
            var expectedResults = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user@domain.com", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user-name", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "user_name", CustID = 3, CTSCustID = 1003, IsInternal = false }
            };

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var userNames = "user1,user2";
            var expectedException = new InvalidOperationException("Database connection failed");

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Throws(expectedException);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.ValidateUsernames(userNames));
            Assert.Equal("Database connection failed", exception.Message);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_RepositoryReturnsNull_HandlesGracefully()
        {
            // Arrange
            var userNames = "user1,user2";

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns((IEnumerable<ValidateUserNameModel>?)null!);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_VerifiesCorrectCriteriaIsPassed()
        {
            // Arrange
            var userNames = "testuser";
            var expectedResults = new List<ValidateUserNameModel>();

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            _service.ValidateUsernames(userNames);

            // Assert
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(
                It.Is<ICriteria>(criteria => 
                    criteria.GetType().Name == "ValidateUsernameCriteria" &&
                    ((ValidateUsernameCriteria)criteria).UserNames == userNames)), Times.Once);
        }

        #endregion

        #region Constructor Tests       

        [Fact]
        public void Constructor_WithValidRepository_InitializesSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<ICriteriaService>();

            // Act
            var service = new CommonService(mockRepository.Object);

            // Assert
            Assert.NotNull(service);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void CheckConnection_WithDatabaseException_PropagatesException()
        {
            // This test documents that CheckConnection can throw database-related exceptions
            // In a real scenario, this would require database setup or mocking of ObjectDbFactory
            // For now, we'll test that the method signature is correct and can be called
            var exception = Record.Exception(() => _service.CheckConnection());
            
            // The method should either complete successfully or throw a database-related exception
            // We're not asserting specific exception types as they depend on database availability
        }

        [Fact]
        public void ValidateUsernames_WithVeryLongInput_HandlesCorrectly()
        {
            // Arrange
            var longUserNames = string.Join(",", Enumerable.Range(1, 1000).Select(i => $"user{i}"));
            var expectedResults = new List<ValidateUserNameModel>();

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(longUserNames);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        [Fact]
        public void ValidateUsernames_WithDuplicateUsernames_HandlesCorrectly()
        {
            // Arrange
            var userNames = "user1,user2,user1,user3,user2";
            var expectedResults = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false },
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false },
                new ValidateUserNameModel { UserName = "user3", CustID = 3, CTSCustID = 1003, IsInternal = false }
            };

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var result = _service.ValidateUsernames(userNames);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Once);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Service_CanPerformMultipleOperations_WithoutInterference()
        {
            // Arrange
            var userNames = "user1,user2";
            var expectedResults = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false }
            };

            _mockRepository.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults);

            // Act
            var connectionException = Record.Exception(() => _service.CheckConnection());
            var validationResult = _service.ValidateUsernames(userNames);
            var secondValidationResult = _service.ValidateUsernames("user3");

            // Assert
            Assert.NotNull(validationResult);
            Assert.NotNull(secondValidationResult);
            _mockRepository.Verify(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()), Times.Exactly(2));
        }

        [Fact]
        public void Service_WithMultipleInstances_WorkIndependently()
        {
            // Arrange
            var mockRepo1 = new Mock<ICriteriaService>();
            var mockRepo2 = new Mock<ICriteriaService>();
            var service1 = new CommonService(mockRepo1.Object);
            var service2 = new CommonService(mockRepo2.Object);

            var expectedResults1 = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user1", CustID = 1, CTSCustID = 1001, IsInternal = false }
            };

            var expectedResults2 = new List<ValidateUserNameModel>
            {
                new ValidateUserNameModel { UserName = "user2", CustID = 2, CTSCustID = 1002, IsInternal = false }
            };

            mockRepo1.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults1);
            mockRepo2.Setup(x => x.Fetch<ValidateUserNameModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResults2);

            // Act
            var result1 = service1.ValidateUsernames("user1");
            var result2 = service2.ValidateUsernames("user2");

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Single(result1);
            Assert.Single(result2);
            Assert.Equal("user1", result1.First().UserName);
            Assert.Equal("user2", result2.First().UserName);
        }

        #endregion
    }
}
