namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api;
    using Fanex.NAP.CTS.Criteria.Api.FPS.Subscriber;
    using Fanex.NAP.CTS.Model.API.FPS.Subscriber;
    using Fanex.NAP.CTS.Model.Subscriber;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.Service.Interface;
    using Moq;
    using System;
    using Xunit;
    public class SubscriberServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly SubscriberService _service;

        public SubscriberServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new SubscriberService(_mockRepository.Object);

            LogExtension.ConfigByPassForUnitTest();
        }

        #region GetSubscriberBySubscriberName Tests

        [Fact]
        public void GetSubscriberBySubscriberName_WithValidSubscriberName_ReturnsSubscriberInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                IsTest = false,
                DCSStatus = true,
                DCSIntegrationDate = DateTime.Now.AddDays(-30),
                FPSSubscriberId = 1001,
                TerminatedDate = DateTime.MinValue
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            Assert.True(result.DCSStatus);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithEmptySubscriberName_ReturnsNull()
        {
            // Arrange
            var subscriberName = "";
            SubscriberInfo expectedResult = null;

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithNullSubscriberName_ReturnsNull()
        {
            // Arrange
            string subscriberName = null;
            SubscriberInfo expectedResult = null;

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithWhitespaceSubscriberName_ReturnsNull()
        {
            // Arrange
            var subscriberName = "   ";
            SubscriberInfo expectedResult = null;

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithLongSubscriberName_ReturnsSubscriberInfo()
        {
            // Arrange
            var subscriberName = new string('A', 1000); // Very long name
            var expectedResult = new SubscriberInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "LONG",
                SubscriberType = 2,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                IsTest = false,
                DCSStatus = true,
                DCSIntegrationDate = DateTime.Now.AddDays(-30),
                FPSSubscriberId = 1001,
                TerminatedDate = DateTime.MinValue
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithSpecialCharacters_ReturnsSubscriberInfo()
        {
            // Arrange
            var subscriberName = "Test@Subscriber#123!";
            var expectedResult = new SubscriberInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "SPEC",
                SubscriberType = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                IsTest = false,
                DCSStatus = false,
                DCSIntegrationDate = DateTime.Now.AddDays(-30),
                FPSSubscriberId = 1001,
                TerminatedDate = DateTime.MinValue
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            Assert.False(result.DCSStatus);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_WithUnicodeCharacters_ReturnsSubscriberInfo()
        {
            // Arrange
            var subscriberName = "测试订阅者123";
            var expectedResult = new SubscriberInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "UNIC",
                SubscriberType = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = "System",
                IsTest = false,
                DCSStatus = true,
                DCSIntegrationDate = DateTime.Now.AddDays(-30),
                FPSSubscriberId = 1001,
                TerminatedDate = DateTime.MinValue
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        #endregion

        #region GetSubscriberBySubscriberName_DCSExtra Tests

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithValidSubscriberName_ReturnsSubscriberFpsInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberFpsInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                SubscriberStatus = true
            };

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            Assert.True(result.SubscriberStatus);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithEmptySubscriberName_ReturnsNull()
        {
            // Arrange
            var subscriberName = "";
            SubscriberFpsInfo expectedResult = null;

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithNullSubscriberName_ReturnsNull()
        {
            // Arrange
            string subscriberName = null;
            SubscriberFpsInfo expectedResult = null;

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithFPSStatusFalse_ReturnsSubscriberFpsInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberFpsInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                SubscriberStatus = false
            };

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            Assert.False(result.SubscriberStatus);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithDCSStatusFalse_ReturnsSubscriberFpsInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberFpsInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                SubscriberStatus = true
            };

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(subscriberName, result.SubscriberName);
            Assert.True(result.SubscriberStatus);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [Fact]
        public void GetSubscriberBySubscriberName_WithZeroSubscriberId_ReturnsSubscriberInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberInfo
            {
                SubscriberId = 0,
                SubscriberName = subscriberName,
                DCSStatus = true
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(0, result.SubscriberId);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void GetSubscriberBySubscriberName_DCSExtra_WithNegativeSubscriberId_ReturnsSubscriberFpsInfo()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedResult = new SubscriberFpsInfo
            {
                SubscriberId = -1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                SubscriberStatus = true
            };

            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(-1, result.SubscriberId);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        #endregion

        #region Service Lifecycle Tests

        [Fact]
        public void SubscriberService_CanBeInstantiated()
        {
            // Arrange & Act
            var service = new SubscriberService(_mockRepository.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void SubscriberService_ImplementsISubscriberService()
        {
            // Arrange
            var service = new SubscriberService(_mockRepository.Object);

            // Act & Assert
            Assert.IsAssignableFrom<ISubscriberService>(service);
        }

        #endregion

        #region Integration Style Tests

        [Fact]
        public void BothMethods_WithSameSubscriberName_ExecutesSuccessfully()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedSubscriberInfo = new SubscriberInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                DCSStatus = true
            };
            var expectedSubscriberFpsInfo = new SubscriberFpsInfo
            {
                SubscriberId = 1,
                SubscriberName = subscriberName,
                SubscriberPrefix = "TEST",
                SubscriberType = 1,
                SubscriberStatus = true
            };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()))
                .Returns(expectedSubscriberInfo);
            _mockRepository.Setup(x => x.Get<SubscriberFpsInfo>(It.IsAny<GetSubscriberDcsExBySubscriberName>()))
                .Returns(expectedSubscriberFpsInfo);

            // Act
            var result1 = _service.GetSubscriberBySubscriberName(subscriberName);
            var result2 = _service.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(subscriberName, result1.SubscriberName);
            Assert.Equal(subscriberName, result2.SubscriberName);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
            _mockRepository.Verify(x => x.Get<SubscriberFpsInfo>(It.Is<GetSubscriberDcsExBySubscriberName>(c => 
                c.SubscriberName == subscriberName)), Times.Once);
        }

        [Fact]
        public void MultipleCalls_WithDifferentSubscriberNames_ExecutesSuccessfully()
        {
            // Arrange
            var subscriberName1 = "Subscriber1";
            var subscriberName2 = "Subscriber2";
            var expectedResult1 = new SubscriberInfo { SubscriberId = 1, SubscriberName = subscriberName1 };
            var expectedResult2 = new SubscriberInfo { SubscriberId = 2, SubscriberName = subscriberName2 };

            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName1)))
                .Returns(expectedResult1);
            _mockRepository.Setup(x => x.Get<SubscriberInfo>(It.Is<GetSubscriberInfoBySubscriberName>(c => 
                c.SubscriberName == subscriberName2)))
                .Returns(expectedResult2);

            // Act
            var result1 = _service.GetSubscriberBySubscriberName(subscriberName1);
            var result2 = _service.GetSubscriberBySubscriberName(subscriberName2);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(subscriberName1, result1.SubscriberName);
            Assert.Equal(subscriberName2, result2.SubscriberName);
            _mockRepository.Verify(x => x.Get<SubscriberInfo>(It.IsAny<GetSubscriberInfoBySubscriberName>()), Times.Exactly(2));
        }

        #endregion
    }
}
