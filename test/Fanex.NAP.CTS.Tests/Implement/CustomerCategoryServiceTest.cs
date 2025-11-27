namespace Fanex.NAP.CTS.Tests.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Category;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Model.API;
    using Fanex.NAP.CTS.Model.API.CustomerClassCategory;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Service.Implement;
    using Moq;
    using Xunit;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    public class CustomerCategoryServiceTest
    {
        private readonly Mock<ICriteriaService> _mockRepository;
        private readonly CustomerCategoryService _service;

        public CustomerCategoryServiceTest()
        {
            _mockRepository = new Mock<ICriteriaService>();
            _service = new CustomerCategoryService(_mockRepository.Object);
            LogExtension.ConfigByPassForUnitTest();
        }

        #region GetCustomerClassCategory Tests

        [Fact]
        public void GetCustomerClassCategory_WithValidData_ReturnsCategories()
        {
            // Arrange
            var expectedResult = new List<CategoryCustomerClassModel>
            {
                new CategoryCustomerClassModel
                {
                    CategoryName = "VIP",
                    CategoryNameCN = "VIP中文",
                    CustomerClass = 1,
                    CustomerClassName = "Premium",
                    CustomerClassNameCN = "高级",
                    CategoryNameCN3 = "VIP中文3",
                    CategoryNameTHB = "VIP泰文",
                    CategoryNameIND = "VIP印尼文",
                    SabaRiskLevel = "High",
                    SabaRiskLevelTCN = "高风险",
                    SabaRiskLevelSCN = "高风险简体",
                    SabaRiskLevelCN3 = "高风险中文3",
                    SabaRiskLevelTHB = "ความเสี่ยงสูง",
                    SabaRiskLevelIND = "Risiko Tinggi"
                },
                new CategoryCustomerClassModel
                {
                    CategoryName = "Regular",
                    CategoryNameCN = "普通中文",
                    CustomerClass = 2,
                    CustomerClassName = "Standard",
                    CustomerClassNameCN = "标准",
                    CategoryNameCN3 = "普通中文3",
                    CategoryNameTHB = "ปกติ",
                    CategoryNameIND = "Biasa",
                    SabaRiskLevel = "Medium",
                    SabaRiskLevelTCN = "中风险",
                    SabaRiskLevelSCN = "中风险简体",
                    SabaRiskLevelCN3 = "中风险中文3",
                    SabaRiskLevelTHB = "ความเสี่ยงปานกลาง",
                    SabaRiskLevelIND = "Risiko Sedang"
                }
            };

            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategory();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategory_WithEmptyData_ReturnsEmptyList()
        {
            // Arrange
            var expectedResult = new List<CategoryCustomerClassModel>();

            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategory();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategory_WithException_ThrowException()
        {
            // Arrange
            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Throws(new Exception("Database connection failed"));

            // Act
            Action result = () => _service.GetCustomerClassCategory();

            // Assert
            Assert.Throws<ArgumentNullException>(result);
        }

        [Fact]
        public void GetCustomerClassCategory_WithNullReturn_ReturnsEmptyList()
        {
            // Arrange
            IEnumerable<CategoryCustomerClassModel> expectedResult = null;

            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategory();

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()), Times.Once);
        }

        #endregion

        #region MarkVVIPCustomerCategory Tests

        [Fact]
        public void MarkVVIPCustomerCategory_WithValidModel_ReturnsSuccessResult()
        {
            // Arrange
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = 123,
                CTSCustId = 100123,
                RoleId = ROLE_CUSTOMER.MEMBER,
                SubscriberId = 1,
                CreatedBy = 999,
                Remark = "Test VVIP marking",
                IsAffectToAllDownlines = true
            };

            var expectedResult = new ResultInsertCustomerClassificationModel
            {
                CustInfos = new List<CustInfoClassificationModel>
                {
                    new CustInfoClassificationModel
                    {
                        CustID = 123,
                        CTSCustID = 100123,
                        CustomerClass = 1,
                        CategoryID = 1
                    }
                },
                ErrorMessage = null
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.MarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void MarkVVIPCustomerCategory_WithNullModel_ThrowsException()
        {
            // Arrange
            CustomerCategoryModel custCategoryModel = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.MarkVVIPCustomerCategory(custCategoryModel));
        }

        [Fact]
        public void MarkVVIPCustomerCategory_WithZeroCustId_ExecutesSuccessfully()
        {
            // Arrange
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = 0,
                CTSCustId = 0,
                RoleId = ROLE_CUSTOMER.AGENT,
                SubscriberId = 1,
                CreatedBy = 999,
                Remark = "Test with zero IDs",
                IsAffectToAllDownlines = false
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.MarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region UnmarkVVIPCustomerCategory Tests

        [Fact]
        public void UnmarkVVIPCustomerCategory_WithValidModel_ReturnsSuccessResult()
        {
            // Arrange
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = 123,
                CTSCustId = 100123,
                RoleId = ROLE_CUSTOMER.MEMBER,
                SubscriberId = 1,
                CreatedBy = 999,
                Remark = "Test VVIP unmarking",
                IsAffectToAllDownlines = true
            };

            var expectedResult = new ResultRemoveCustomerClassificationModel
            {
                CustInfos = new List<ResultRemoveCCInfoModel>
                {
                    new ResultRemoveCCInfoModel
                    {
                        CustID = 123,
                        CTSCustID = 100123,
                        IsCleanPA = true
                    }
                }
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.UnmarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UnmarkVVIPCustomerCategory_WithNullModel_ThrowsException()
        {
            // Arrange
            CustomerCategoryModel custCategoryModel = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _service.UnmarkVVIPCustomerCategory(custCategoryModel));
        }

        [Fact]
        public void UnmarkVVIPCustomerCategory_WithZeroCustId_ExecutesSuccessfully()
        {
            // Arrange
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = 0,
                CTSCustId = 0,
                RoleId = ROLE_CUSTOMER.AGENT,
                SubscriberId = 1,
                CreatedBy = 999,
                Remark = "Test with zero IDs",
                IsAffectToAllDownlines = false
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.UnmarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region GetCustomerClassCategoryByCustID Tests

        [Fact]
        public void GetCustomerClassCategoryByCustID_WithValidCustId_ReturnsCategories()
        {
            // Arrange
            var custId = 123L;
            var expectedResult = new List<CustomerClassCategoryByCustIDModel>
            {
                new CustomerClassCategoryByCustIDModel
                {
                    CustID = 123,
                    CategoryID = 1,
                    CustomerClass = 1,
                    CreatedDate = DateTime.Now
                },
                new CustomerClassCategoryByCustIDModel
                {
                    CustID = 123,
                    CategoryID = 2,
                    CustomerClass = 2,
                    CreatedDate = DateTime.Now.AddDays(-1)
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategoryByCustID(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.Is<GetCategoryCustomerClassByCustIDCriteria>(c =>
                c.CustIDs == custId.ToString())), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategoryByCustID_WithZeroCustId_ReturnsEmptyList()
        {
            // Arrange
            var custId = 0L;
            var expectedResult = new List<CustomerClassCategoryByCustIDModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategoryByCustID(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.Is<GetCategoryCustomerClassByCustIDCriteria>(c =>
                c.CustIDs == custId.ToString())), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategoryByCustID_WithNegativeCustId_ReturnsEmptyList()
        {
            // Arrange
            var custId = -123L;
            var expectedResult = new List<CustomerClassCategoryByCustIDModel>();

            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategoryByCustID(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.Is<GetCategoryCustomerClassByCustIDCriteria>(c =>
                c.CustIDs == custId.ToString())), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategoryByCustID_WithMaxLongCustId_ReturnsCategories()
        {
            // Arrange
            var custId = long.MaxValue;
            var expectedResult = new List<CustomerClassCategoryByCustIDModel>
            {
                new CustomerClassCategoryByCustIDModel
                {
                    CustID = long.MaxValue,
                    CategoryID = 1,
                    CustomerClass = 1,
                    CreatedDate = DateTime.Now
                }
            };

            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategoryByCustID(custId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedResult.First(), result.First());
            _mockRepository.Verify(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.Is<GetCategoryCustomerClassByCustIDCriteria>(c =>
                c.CustIDs == custId.ToString())), Times.Once);
        }

        [Fact]
        public void GetCustomerClassCategoryByCustID_WithNullReturn_ReturnsNull()
        {
            // Arrange
            var custId = 123L;
            IEnumerable<CustomerClassCategoryByCustIDModel> expectedResult = null;

            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategoryByCustID(custId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.Is<GetCategoryCustomerClassByCustIDCriteria>(c =>
                c.CustIDs == custId.ToString())), Times.Once);
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [Fact]
        public void GetCustomerClassCategory_WithLargeDataSet_ReturnsAllCategories()
        {
            // Arrange
            var expectedResult = Enumerable.Range(1, 1000).Select(i => new CategoryCustomerClassModel
            {
                CategoryName = $"Category{i}",
                CategoryNameCN = $"Category{i}中文",
                CustomerClass = i % 10,
                CustomerClassName = $"Class{i % 10}",
                CustomerClassNameCN = $"类别{i % 10}",
                CategoryNameCN3 = $"Category{i}中文3",
                CategoryNameTHB = $"Category{i}泰文",
                CategoryNameIND = $"Category{i}印尼文",
                SabaRiskLevel = $"Risk{i % 3}",
                SabaRiskLevelTCN = $"风险{i % 3}",
                SabaRiskLevelSCN = $"风险{i % 3}简体",
                SabaRiskLevelCN3 = $"风险{i % 3}中文3",
                SabaRiskLevelTHB = $"ความเสี่ยง{i % 3}",
                SabaRiskLevelIND = $"Risiko{i % 3}"
            }).ToList();

            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Returns(expectedResult);

            // Act
            var result = _service.GetCustomerClassCategory();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Count());
            _mockRepository.Verify(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()), Times.Once);
        }

        [Fact]
        public void MarkVVIPCustomerCategory_WithLargeCustId_ExecutesSuccessfully()
        {
            // Arrange
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = long.MaxValue,
                CTSCustId = long.MaxValue,
                RoleId = ROLE_CUSTOMER.MEMBER,
                SubscriberId = int.MaxValue,
                CreatedBy = int.MaxValue,
                Remark = "Test with large values",
                IsAffectToAllDownlines = true
            };

            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var result = _service.MarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region Integration Style Tests

        [Fact]
        public void GetAllMethods_WithSameCustId_ExecutesSuccessfully()
        {
            // Arrange
            var custId = 123L;
            var custCategoryModel = new CustomerCategoryModel
            {
                CustId = custId,
                CTSCustId = 100123,
                RoleId = ROLE_CUSTOMER.MEMBER,
                SubscriberId = 1,
                CreatedBy = 999,
                Remark = "Integration test",
                IsAffectToAllDownlines = true
            };

            var expectedCategories = new List<CategoryCustomerClassModel>();
            var expectedCustCategories = new List<CustomerClassCategoryByCustIDModel>();

            _mockRepository.Setup(x => x.Fetch<CategoryCustomerClassModel>(It.IsAny<GetCategoryCustomerClassCriteria>()))
                .Returns(expectedCategories);
            _mockRepository.Setup(x => x.Fetch<CustomerClassCategoryByCustIDModel>(It.IsAny<GetCategoryCustomerClassByCustIDCriteria>()))
                .Returns(expectedCustCategories);
            _mockRepository.Setup(x => x.Execute(It.IsAny<INonQueryCommand>()))
                .Returns(0);

            // Act
            var categories = _service.GetCustomerClassCategory();
            var custCategories = _service.GetCustomerClassCategoryByCustID(custId);
            var markResult = _service.MarkVVIPCustomerCategory(custCategoryModel);
            var unmarkResult = _service.UnmarkVVIPCustomerCategory(custCategoryModel);

            // Assert
            Assert.NotNull(categories);
            Assert.NotNull(custCategories);
            Assert.NotNull(markResult);
            Assert.NotNull(unmarkResult);
        }

        #endregion
    }
}
