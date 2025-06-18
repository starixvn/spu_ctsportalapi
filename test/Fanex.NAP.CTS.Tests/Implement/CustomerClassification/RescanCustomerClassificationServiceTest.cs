using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.CTS.Core.Service;
using Fanex.NAP.CTS.Model;
using Fanex.NAP.CTS.Model.CustomerClassification;
using Fanex.NAP.CTS.Model.RescanCustClassificationModel;
using Fanex.NAP.CTS.Model.SyncCustomerToMainDb;
using Fanex.NAP.CTS.Service;
using Fanex.NAP.CTS.Service.Implement;
using Fanex.Data.Repository;
using Fanex.NAP.CTS.Criteria.CustomerClassification;
using Moq;
using Xunit;
using static Fanex.NAP.CTS.Resource.Consts.Consts;

namespace Fanex.NAP.CTS.Tests.Implement.CustomerClassification
{
    public class RescanCustomerClassificationServiceTest
    {
        private readonly Mock<ICustomerClassificationService> _mockCustomerClassificationService;
        private readonly Mock<ICriteriaService> _mockCriteriaService;
        private readonly Mock<IDataLogTrackingService> _mockDataLogTrackingService;
        private readonly RescanCustomerClassificationService _service;

        public RescanCustomerClassificationServiceTest()
        {
            _mockCustomerClassificationService = new Mock<ICustomerClassificationService>();
            _mockCriteriaService = new Mock<ICriteriaService>();
            _mockDataLogTrackingService = new Mock<IDataLogTrackingService>();
            _service = new RescanCustomerClassificationService(
                _mockCustomerClassificationService.Object,
                _mockCriteriaService.Object,
                _mockCustomerClassificationService.Object,
                _mockDataLogTrackingService.Object);
        }

        [Fact]
        public void RescanCustomerClassification_WithNormalAndProblemAccounts_ReturnsSuccess()
        {
            // Arrange
            var parameters = new RescanCustClassificationModel
            {
                CustIds = new List<long> { 1, 2, 3 },
                IsToQueue = false,
                IsUpdateDangerToMainDB = true,
                FunctionId = FunctionId.InsertPAM
            };

            var custRescans = new GetRescanCustomerClassificationModel
            {
                CustInfos = new List<RescanCustomerClassificationModel>
                {
                    new RescanCustomerClassificationModel { CustID = 1, InputFlowID = (int)CCInputFlow.RescanNormal },
                    new RescanCustomerClassificationModel { CustID = 2, InputFlowID = (int)CCInputFlow.RescanPACategory },
                    new RescanCustomerClassificationModel { CustID = 3, InputFlowID = (int)CCInputFlow.RescanPACategory }
                }
            };

            _mockCriteriaService.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(custRescans.CustInfos);

            _mockCustomerClassificationService.Setup(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()))
                .Returns(true);

            // Act
            var result = _service.RescanCustomerClassification(parameters);

            // Assert
            Assert.True(result);
            _mockCriteriaService.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Once);
            _mockCustomerClassificationService.Verify(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()), Times.Once);
        }

        [Fact]
        public void RescanCustomerClassification_WithNoCustomers_ReturnsSuccess()
        {
            // Arrange
            var parameters = new RescanCustClassificationModel
            {
                CustIds = new List<long>(),
                IsToQueue = false,
                IsUpdateDangerToMainDB = true,
                FunctionId = FunctionId.InsertPAM
            };

            var custRescans = new GetRescanCustomerClassificationModel
            {
                CustInfos = new List<RescanCustomerClassificationModel>()
            };

            _mockCriteriaService.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(custRescans.CustInfos);

            // Act
            var result = _service.RescanCustomerClassification(parameters);

            // Assert
            Assert.True(result);
            _mockCriteriaService.Verify(x => x.Execute(It.IsAny<INonQueryCommand>()), Times.Never);
            _mockCustomerClassificationService.Verify(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()), Times.Never);
        }

        [Fact]
        public void RescanCustomerClassification_WithException_ReturnsFalse()
        {
            // Arrange
            var parameters = new RescanCustClassificationModel
            {
                CustIds = new List<long> { 1 },
                IsToQueue = false,
                IsUpdateDangerToMainDB = true,
                FunctionId = FunctionId.InsertPAM
            };

            _mockCriteriaService.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _service.RescanCustomerClassification(parameters);

            // Assert
            Assert.False(result);
            _mockDataLogTrackingService.Verify(x => x.LogErrorData(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void RescanCustomerClassification_WithOnlyNormalAccounts_ExecutesNormalClassification()
        {
            // Arrange
            var parameters = new RescanCustClassificationModel
            {
                CustIds = new List<long> { 1, 2 },
                IsToQueue = false,
                IsUpdateDangerToMainDB = true,
                FunctionId = FunctionId.InsertPAM
            };

            var custRescans = new GetRescanCustomerClassificationModel
            {
                CustInfos = new List<RescanCustomerClassificationModel>
                {
                    new RescanCustomerClassificationModel { CustID = 1, InputFlowID = (int)CCInputFlow.RescanNormal },
                    new RescanCustomerClassificationModel { CustID = 2, InputFlowID = (int)CCInputFlow.RescanNormal }
                }
            };

            _mockCriteriaService.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(custRescans.CustInfos);

            // Act
            var result = _service.RescanCustomerClassification(parameters);

            // Assert
            Assert.True(result);
            _mockCriteriaService.Verify(x => x.Execute(It.Is<INonQueryCommand>(c => 
                c is NormalClassificationRescanCriteria)), Times.Once);
            _mockCustomerClassificationService.Verify(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()), Times.Never);
        }

        [Fact]
        public void RescanCustomerClassification_WithOnlyProblemAccounts_ExecutesProblemAccountClassification()
        {
            // Arrange
            var parameters = new RescanCustClassificationModel
            {
                CustIds = new List<long> { 1, 2 },
                IsToQueue = false,
                IsUpdateDangerToMainDB = true,
                FunctionId = FunctionId.InsertPAM
            };

            var custRescans = new GetRescanCustomerClassificationModel
            {
                CustInfos = new List<RescanCustomerClassificationModel>
                {
                    new RescanCustomerClassificationModel { CustID = 1, InputFlowID = (int)CCInputFlow.RescanPACategory },
                    new RescanCustomerClassificationModel { CustID = 2, InputFlowID = (int)CCInputFlow.RescanPACategory }
                }
            };

            _mockCriteriaService.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(custRescans.CustInfos);

            _mockCustomerClassificationService.Setup(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()))
                .Returns(true);

            // Act
            var result = _service.RescanCustomerClassification(parameters);

            // Assert
            Assert.True(result);
            _mockCriteriaService.Verify(x => x.Execute(It.Is<INonQueryCommand>(c => 
                c is NormalClassificationRescanCriteria)), Times.Never);
            _mockCustomerClassificationService.Verify(x => x.PushDataToExternalPackge(
                It.IsAny<IEnumerable<long>>(),
                It.IsAny<PushDataToExternalModel>(),
                It.IsAny<FromFlow>()), Times.Once);
        }
    }
}
