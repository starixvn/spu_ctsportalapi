namespace Fanex.NAP.CTS.Tests.Implement.CustomerClassification
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.CustomerClassification.BaseCustomerClassification;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Service.Implement;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class BaseCustomerClassificationServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly BaseCustomerClassificationService _service;

        public BaseCustomerClassificationServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new BaseCustomerClassificationService(_mockRepository.Object);
        }

        #region GetRescanCustomerClassification Tests

        [Fact]
        public void GetRescanCustomerClassification_WithValidCustIDs_ReturnsModelWithCustInfos()
        {
            // Arrange
            var custIDs = "12345,67890";
            var expectedCustInfos = new List<RescanCustomerClassificationModel>
            {
                new RescanCustomerClassificationModel { CustID = 12345 },
                new RescanCustomerClassificationModel { CustID = 67890 }
            };

            _mockRepository.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<GetRescanCustomerClassificationCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.GetRescanCustomerClassification(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustInfos, result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<RescanCustomerClassificationModel>(It.Is<GetRescanCustomerClassificationCriteria>(c => 
                c.CustIDs == custIDs)), Times.Once);
        }

        [Fact]
        public void GetRescanCustomerClassification_WithNullCustIDs_ReturnsEmptyModel()
        {
            // Arrange
            string custIDs = null;
            _mockRepository.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<GetRescanCustomerClassificationCriteria>()))
                .Returns(new List<RescanCustomerClassificationModel>());

            // Act
            var result = _service.GetRescanCustomerClassification(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<RescanCustomerClassificationModel>(It.Is<GetRescanCustomerClassificationCriteria>(c => 
                c.CustIDs == null)), Times.Once);
        }

        [Fact]
        public void GetRescanCustomerClassification_WithEmptyCustIDs_ReturnsEmptyModel()
        {
            // Arrange
            var custIDs = "";
            _mockRepository.Setup(x => x.Fetch<RescanCustomerClassificationModel>(It.IsAny<GetRescanCustomerClassificationCriteria>()))
                .Returns(new List<RescanCustomerClassificationModel>());

            // Act
            var result = _service.GetRescanCustomerClassification(custIDs);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<RescanCustomerClassificationModel>(It.Is<GetRescanCustomerClassificationCriteria>(c => 
                c.CustIDs == "")), Times.Once);
        }

        #endregion

        #region InsertCustomerClassification Tests

        [Fact]
        public void InsertCustomerClassification_WithValidModel_ReturnsModelWithCustInfos()
        {
            // Arrange
            var inputFlow = CCInputFlow.VVIP;
            var model = new InsertCustomerClassificationModel
            {
                CustClassificationJson = "[{\"CustID\":12345}]",
                IsToQueue = true,
                QueueID = 1,
                UplineRoleID = 2,
                IsAffectDownline = true,
                FromAction = 3
            };

            var expectedCustInfos = new List<CustInfoClassificationModel>
            {
                new CustInfoClassificationModel { CustID = 12345 }
            };

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationModel>(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.InsertCustomerClassification(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustInfos, result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<CustInfoClassificationModel>(It.Is<InsertCustomerClassificationCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson &&
                c.IsToQueue == model.IsToQueue &&
                c.QueueID == model.QueueID &&
                c.UplineRoleID == model.UplineRoleID &&
                c.IsAffectDownline == model.IsAffectDownline &&
                c.FromAction == model.FromAction)), Times.Once);
        }

        [Fact]
        public void InsertCustomerClassification_WithNullModel_ThrowsNullReferenceException()
        {
            // Arrange
            var inputFlow = CCInputFlow.VVIP;
            InsertCustomerClassificationModel model = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.InsertCustomerClassification(inputFlow, model));
        }

        [Fact]
        public void InsertCustomerClassification_WithEmptyJson_ReturnsEmptyModel()
        {
            // Arrange
            var inputFlow = CCInputFlow.VVIP;
            var model = new InsertCustomerClassificationModel
            {
                CustClassificationJson = "[]",
                IsToQueue = false,
                QueueID = 0,
                UplineRoleID = 0,
                IsAffectDownline = false,
                FromAction = 1
            };

            var expectedCustInfos = new List<CustInfoClassificationModel>();

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationModel>(It.IsAny<InsertCustomerClassificationCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.InsertCustomerClassification(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<CustInfoClassificationModel>(It.Is<InsertCustomerClassificationCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson &&
                c.IsToQueue == model.IsToQueue &&
                c.QueueID == model.QueueID &&
                c.UplineRoleID == model.UplineRoleID &&
                c.IsAffectDownline == model.IsAffectDownline &&
                c.FromAction == model.FromAction)), Times.Once);
        }

        #endregion

        #region RemoveCustomerClassification Tests

        [Fact]
        public void RemoveCustomerClassification_WithValidModel_ReturnsModelWithCustInfos()
        {
            // Arrange
            var inputFlow = CCInputFlow.RemoveVVIP;
            var model = new RemoveCustomerClassificationModel
            {
                CustClassificationJson = "[{\"CustID\":12345}]",
                FromAction = 1,
                UplineRoleID = 2,
                QueueID = 3,
                IsAffectDownline = true,
                IsFromQueue = false
            };

            var expectedCustInfos = new List<ResultRemoveCCInfoModel>
            {
                new ResultRemoveCCInfoModel { CustID = 12345, CTSCustID = 67890, IsCleanPA = true }
            };

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoModel>(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.RemoveCustomerClassification(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustInfos, result.CustInfos);
            Assert.Equal(expectedCustInfos[0].CustID, result.CustInfos.First().CustID);
            Assert.Equal(expectedCustInfos[0].CTSCustID, result.CustInfos.First().CTSCustID);
            Assert.Equal(expectedCustInfos[0].IsCleanPA, result.CustInfos.First().IsCleanPA);
            _mockRepository.Verify(x => x.Fetch<ResultRemoveCCInfoModel>(It.Is<RemoveCustomerClassificationCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson &&
                c.FromAction == model.FromAction &&
                c.UplineRoleID == model.UplineRoleID &&
                c.QueueID == model.QueueID &&
                c.IsAffectDownline == model.IsAffectDownline &&
                c.IsFromQueue == model.IsFromQueue)), Times.Once);
        }

        [Fact]
        public void RemoveCustomerClassification_WithNullModel_ThrowsNullReferenceException()
        {
            // Arrange
            var inputFlow = CCInputFlow.RemoveVVIP;
            RemoveCustomerClassificationModel model = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.RemoveCustomerClassification(inputFlow, model));
        }

        [Fact]
        public void RemoveCustomerClassification_WithEmptyJson_ReturnsEmptyModel()
        {
            // Arrange
            var inputFlow = CCInputFlow.RemoveVVIP;
            var model = new RemoveCustomerClassificationModel
            {
                CustClassificationJson = "[]",
                FromAction = 1,
                UplineRoleID = 0,
                QueueID = 0,
                IsAffectDownline = false,
                IsFromQueue = false
            };

            var expectedCustInfos = new List<ResultRemoveCCInfoModel>();

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoModel>(It.IsAny<RemoveCustomerClassificationCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.RemoveCustomerClassification(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<ResultRemoveCCInfoModel>(It.Is<RemoveCustomerClassificationCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson &&
                c.FromAction == model.FromAction &&
                c.UplineRoleID == model.UplineRoleID &&
                c.QueueID == model.QueueID &&
                c.IsAffectDownline == model.IsAffectDownline &&
                c.IsFromQueue == model.IsFromQueue)), Times.Once);
        }

        #endregion

        #region InsertCustomerClassificationBySport Tests

        [Fact]
        public void InsertCustomerClassificationBySport_WithValidModel_ReturnsModelWithCustInfos()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportInsertSpecialCC;
            var model = new InsertCustomerClassificationBySportModel
            {
                CustClassificationJson = "[{\"CustID\":12345,\"SportID\":1}]"
            };

            var expectedCustInfos = new List<CustInfoClassificationBySportModel>
            {
                new CustInfoClassificationBySportModel { CustID = 12345 }
            };

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationBySportModel>(It.IsAny<InsertCustomerClassificationBySportCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.InsertCustomerClassificationBySport(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustInfos, result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<CustInfoClassificationBySportModel>(It.Is<InsertCustomerClassificationBySportCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson)), Times.Once);
        }

        [Fact]
        public void InsertCustomerClassificationBySport_WithNullModel_ThrowsNullReferenceException()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportInsertSpecialCC;
            InsertCustomerClassificationBySportModel model = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.InsertCustomerClassificationBySport(inputFlow, model));
        }

        [Fact]
        public void InsertCustomerClassificationBySport_WithEmptyJson_ReturnsEmptyModel()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportInsertSpecialCC;
            var model = new InsertCustomerClassificationBySportModel
            {
                CustClassificationJson = "[]"
            };

            var expectedCustInfos = new List<CustInfoClassificationBySportModel>();

            _mockRepository.Setup(x => x.Fetch<CustInfoClassificationBySportModel>(It.IsAny<InsertCustomerClassificationBySportCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.InsertCustomerClassificationBySport(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<CustInfoClassificationBySportModel>(It.Is<InsertCustomerClassificationBySportCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson)), Times.Once);
        }

        #endregion

        #region RemoveCustomerClassificationBySport Tests

        [Fact]
        public void RemoveCustomerClassificationBySport_WithValidModel_ReturnsModelWithCustInfos()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportRemoveSpecialCC;
            var model = new RemoveCustomerClassificationBySportModel
            {
                CustClassificationJson = "[{\"CustID\":12345,\"SportID\":1}]"
            };

            var expectedCustInfos = new List<ResultRemoveCCInfoBySportModel>
            {
                new ResultRemoveCCInfoBySportModel 
                { 
                    CustID = 12345,
                    CTSCustID = 67890,
                    SportID = 1
                }
            };

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoBySportModel>(It.IsAny<RemoveCustomerClassificationBySportCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.RemoveCustomerClassificationBySport(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustInfos, result.CustInfos);
            Assert.Equal(expectedCustInfos[0].CustID, result.CustInfos.First().CustID);
            Assert.Equal(expectedCustInfos[0].CTSCustID, result.CustInfos.First().CTSCustID);
            Assert.Equal(expectedCustInfos[0].SportID, result.CustInfos.First().SportID);
            _mockRepository.Verify(x => x.Fetch<ResultRemoveCCInfoBySportModel>(It.Is<RemoveCustomerClassificationBySportCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson)), Times.Once);
        }

        [Fact]
        public void RemoveCustomerClassificationBySport_WithNullModel_ThrowsNullReferenceException()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportRemoveSpecialCC;
            RemoveCustomerClassificationBySportModel model = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.RemoveCustomerClassificationBySport(inputFlow, model));
        }

        [Fact]
        public void RemoveCustomerClassificationBySport_WithEmptyCustClassificationJson_ReturnsEmptyModel()
        {
            // Arrange
            var inputFlow = CCInputFlow.BySportRemoveSpecialCC;
            var model = new RemoveCustomerClassificationBySportModel
            {
                CustClassificationJson = "[]"
            };

            var expectedCustInfos = new List<ResultRemoveCCInfoBySportModel>();

            _mockRepository.Setup(x => x.Fetch<ResultRemoveCCInfoBySportModel>(It.IsAny<RemoveCustomerClassificationBySportCriteria>()))
                .Returns(expectedCustInfos);

            // Act
            var result = _service.RemoveCustomerClassificationBySport(inputFlow, model);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.CustInfos);
            _mockRepository.Verify(x => x.Fetch<ResultRemoveCCInfoBySportModel>(It.Is<RemoveCustomerClassificationBySportCriteria>(c => 
                c.InputFlowID == (int)inputFlow &&
                c.CustClassificationJson == model.CustClassificationJson)), Times.Once);
        }

        #endregion
    }
}
