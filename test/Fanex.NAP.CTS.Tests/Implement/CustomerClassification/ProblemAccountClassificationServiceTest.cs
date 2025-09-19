namespace Fanex.NAP.CTS.Tests.Implement.CustomerClassification
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Model.CustomerClassification.ProblemAccount;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.Data.Repository;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Fanex.NAP.CTS.Service;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    public class ProblemAccountClassificationServiceTest
    {
        private readonly Mock<ICriteriaService> _mockCriteriaService;
        private readonly Mock<ICustomerClassificationService> _mockCustomerClassificationService;
        private readonly Mock<IDataLogTrackingService> _mockDataLogTrackingService;
        private readonly ProblemAccountClassificationService _service;

        public ProblemAccountClassificationServiceTest()
        {
            _mockCriteriaService = new Mock<ICriteriaService>();
            _mockCustomerClassificationService = new Mock<ICustomerClassificationService>();
            _mockDataLogTrackingService = new Mock<IDataLogTrackingService>();
            _service = new ProblemAccountClassificationService(
                _mockCriteriaService.Object,
                _mockCustomerClassificationService.Object,
                _mockDataLogTrackingService.Object);
        }

        [Fact]
        public void RescanCustomerClassification_ValidInput_ReturnsCustomerIds()
        {
            // Arrange
            var custRescans = new List<RescanCustomerClassificationModel>
            {
                new RescanCustomerClassificationModel { CustID = 1 },
                new RescanCustomerClassificationModel { CustID = 2 }
            };

            var expectedResult = new List<CustInfoClassificationModel>
            {
                new CustInfoClassificationModel { CustID = 1 },
                new CustInfoClassificationModel { CustID = 2 }
            };

            _mockCriteriaService.Setup(x => x.Fetch<CustInfoClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.RescanCustomerClassification(custRescans, CCInputFlow.Normal, false);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void InsertProblemAccountPackage_ValidInput_ReturnsSuccessResult()
        {
            // Arrange
            var problemAccounts = new List<InsertProblemAccountModel>
            {
                new InsertProblemAccountModel { CustID = 1 },
                new InsertProblemAccountModel { CustID = 2 }
            };

            var expectedResult = new List<ResultInsertCustomerClassificationModel>{
                new ResultInsertCustomerClassificationModel
                {
                    CustInfos = new List<CustInfoClassificationModel>
                    {
                        new CustInfoClassificationModel { CustID = 1 },
                        new CustInfoClassificationModel { CustID = 2 }
                    }
                }
            };

            _mockCriteriaService.Setup(x => x.Fetch<ResultInsertCustomerClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.InsertProblemAccountPackage(problemAccounts, CCInputFlow.Normal, "TestService", FromFlow.CTSWEB, false);

            // Assert
            Assert.Equal(0, result.ErrorCode);
            Assert.Empty(result.ErrorMsg);
        }

        [Fact]
        public void GetProblemAccountLosingClassification_ValidInput_ReturnsClassificationData()
        {
            // Arrange
            var custIds = new List<long> { 1, 2 };
            var expectedResult = new List<ProblemAccountLosingClassificationModel>
            {
                new ProblemAccountLosingClassificationModel { CustId = 1, WinlossStatus = 1 },
                new ProblemAccountLosingClassificationModel { CustId = 2, WinlossStatus = 2 }
            };

            _mockCriteriaService.Setup(x => x.Fetch<ProblemAccountLosingClassificationModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetProblemAccountLosingClassification(custIds, FunctionId.InsertPAM);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.CustId == 1 && x.WinlossStatus == 1);
            Assert.Contains(result, x => x.CustId == 2 && x.WinlossStatus == 2);
        }

        [Fact]
        public void UnmarkProblemAccount_ValidInput_ReturnsSuccessResult()
        {
            // Arrange
            var paUnmark = new List<UnmarkProblemAccountModel>
            {
                new UnmarkProblemAccountModel { CustID = 1 },
                new UnmarkProblemAccountModel { CustID = 2 }
            };

            var expectedResult = new List<ResultRemoveCCInfoModel>
            {
                new ResultRemoveCCInfoModel { CustID = 1 },
                new ResultRemoveCCInfoModel { CustID = 2 }
            };

            _mockCriteriaService.Setup(x => x.Fetch<ResultRemoveCCInfoModel>(It.IsAny<ICriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.UnmarkProblemAccount(paUnmark, false);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateBetLimitOfFlaggedCustomer_ValidInput_ExecutesSuccessfully()
        {
            // Arrange
            var custIds = new List<long> { 1, 2 };

            // Act & Assert
            var exception = Record.Exception(() => _service.UpdateBetLimitOfFlaggedCustomer(custIds));
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateBetLimitOfUnflaggedCustomer_ValidInput_ExecutesSuccessfully()
        {
            // Arrange
            var custIds = new List<long> { 1, 2 };

            // Act & Assert
            var exception = Record.Exception(() => _service.UpdateBetLimitOfUnflaggedCustomer(custIds));
            Assert.Null(exception);
        }        
    }
}
