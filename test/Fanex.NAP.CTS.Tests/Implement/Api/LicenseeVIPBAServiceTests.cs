namespace Fanex.NAP.CTS.Tests.Implement.Api
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.CustomerClassification.BaseCustomerClassification;
    using Fanex.NAP.CTS.Model.API;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Model.RescanCustClassificationModel;
    using Fanex.NAP.CTS.Model.SyncCustomerToMainDb;
    using Fanex.NAP.CTS.Service;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    public class LicenseeVIPBAServiceTests
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly Mock<ICustomerClassificationService> _mockCustClassificationService;
        private readonly Mock<IRescanCustomerClassificationService> _mockRescanCustomerClassificationService;
        private readonly LicenseeVIPBAService _service;

        public LicenseeVIPBAServiceTests()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _mockCustClassificationService = new Mock<ICustomerClassificationService>();
            _mockRescanCustomerClassificationService = new Mock<IRescanCustomerClassificationService>();
            _service = new LicenseeVIPBAService(
                _mockRepository.Object,
                _mockCustClassificationService.Object,
                _mockRescanCustomerClassificationService.Object);
        }

        [Fact]
        public void CheckValid_WithValidParameter_ReturnsTrue()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = "12345",
                CreatedBy = 1
            };

            // Act
            var result = _service.CheckValid(parameter);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckValid_WithInvalidParameter_ReturnsFalse()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = null,
                CreatedBy = 1
            };

            // Act
            var result = _service.CheckValid(parameter);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void InsertLicVIP_WithValidParameter_CallsRepositoryAndClassificationServices()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = "12345",
                CreatedBy = 1
            };

            var expectedResult = new ResultInsertCustomerClassificationModel
            {
                CustInfos = new List<CustInfoClassificationModel>
                {
                    new CustInfoClassificationModel { CustID = 12345 }
                }
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(1);

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationModel>(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(expectedResult.CustInfos);

            // Act
            _service.InsertLicVIP(parameter);

            // Assert
            _mockRepository.Verify(
                x => x.Fetch<CustInfoClassificationModel>(It.Is<InsertCustomerClassificationCriteria>(m => 
                    m.FromAction == (int)FromFlow.CTSAPI && 
                    m.CustClassificationJson.Contains("12345"))), 
                Times.Once);

            _mockCustClassificationService.Verify(
                x => x.PushDataToExternal(
                    It.Is<IEnumerable<long>>(ids => ids.Contains(12345)),
                    It.Is<PushDataToExternalModel>(m => 
                        !m.IsUpdateDangerToMainDB && 
                        !m.IsSyncAFC)), 
                Times.Once);
        }

        [Fact]
        public void RemoveLicVIP_WithValidParameter_CallsRepositoryAndRescanService()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = "12345",
                CreatedBy = 1
            };

            var expectedResult = new ResultRemoveCustomerClassificationModel
            {
                CustInfos = new List<ResultRemoveCCInfoModel>
                {
                    new ResultRemoveCCInfoModel { CustID = 12345 }
                }
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(1);

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoModel>(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(expectedResult.CustInfos);

            // Act
            _service.RemoveLicVIP(parameter);

            // Assert
            _mockRepository.Verify(
                x => x.Fetch<ResultRemoveCCInfoModel>(It.Is<RemoveCustomerClassificationCriteria>(m => 
                    m.FromAction == (int)FromFlow.CTSAPI && 
                    m.CustClassificationJson.Contains("12345"))), 
                Times.Once);

            _mockRescanCustomerClassificationService.Verify(
                x => x.RescanCustomerClassification(
                    It.Is<RescanCustClassificationModel>(m => 
                        m.CustIds.Contains(12345) && 
                        m.IsUpdateDangerToMainDB && 
                        m.FunctionId == FunctionId.RemoveLicVIP_API)), 
                Times.Once);
        }

        [Fact]
        public void InsertLicBA_WithValidParameter_CallsRepositoryAndClassificationServices()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = "12345",
                CreatedBy = 1
            };

            var expectedResult = new ResultInsertCustomerClassificationModel
            {
                CustInfos = new List<CustInfoClassificationModel>
                {
                    new CustInfoClassificationModel { CustID = 12345 }
                }
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(1);

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationModel>(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(expectedResult.CustInfos);

            // Act
            _service.InsertLicBA(parameter);

            // Assert
            _mockRepository.Verify(
                x => x.Fetch<CustInfoClassificationModel>(It.Is<InsertCustomerClassificationCriteria>(m => 
                    m.FromAction == (int)FromFlow.CTSAPI && 
                    m.CustClassificationJson.Contains("12345"))), 
                Times.Once);

            _mockCustClassificationService.Verify(
                x => x.PushDataToExternal(
                    It.Is<IEnumerable<long>>(ids => ids.Contains(12345)),
                    It.Is<PushDataToExternalModel>(m => 
                        !m.IsUpdateDangerToMainDB && 
                        !m.IsSyncAFC)), 
                Times.Once);
        }

        [Fact]
        public void RemoveLicBA_WithValidParameter_CallsRepositoryAndRescanService()
        {
            // Arrange
            var parameter = new LicenseeVIPBAInputModel
            {
                CustIDs = "12345",
                CreatedBy = 1
            };

            var expectedResult = new ResultRemoveCustomerClassificationModel
            {
                CustInfos = new List<ResultRemoveCCInfoModel>
                {
                    new ResultRemoveCCInfoModel { CustID = 12345 }
                }
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(1);

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoModel>(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(expectedResult.CustInfos);

            // Act
            _service.RemoveLicBA(parameter);

            // Assert
            _mockRepository.Verify(
                x => x.Fetch<ResultRemoveCCInfoModel>(It.Is<RemoveCustomerClassificationCriteria>(m => 
                    m.FromAction == (int)FromFlow.CTSAPI && 
                    m.CustClassificationJson.Contains("12345"))), 
                Times.Once);

            _mockRescanCustomerClassificationService.Verify(
                x => x.RescanCustomerClassification(
                    It.Is<RescanCustClassificationModel>(m => 
                        m.CustIds.Contains(12345) && 
                        !m.IsUpdateDangerToMainDB && 
                        m.FunctionId == FunctionId.RemoveLicBA_API)), 
                Times.Once);
        }
    }
}