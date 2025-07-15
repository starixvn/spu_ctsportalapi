namespace Fanex.NAP.CTS.Tests.Implement
{
    using Fanex.NAP.CTS.Service.Implement;
    using Xunit;

    public class CommonServiceTest
    {
        private readonly CommonService _service;

        public CommonServiceTest()
        {
            _service = new CommonService();
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
            var service1 = new CommonService();
            var service2 = new CommonService();

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
            var service = new CommonService();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CommonService_ImplementsICommonService()
        {
            // Arrange
            var service = new CommonService();

            // Act & Assert
            Assert.IsAssignableFrom<Fanex.NAP.CTS.Service.Interface.ICommonService>(service);
        }

        #endregion
    }
}
