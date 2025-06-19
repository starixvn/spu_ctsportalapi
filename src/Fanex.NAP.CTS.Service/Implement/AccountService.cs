namespace Fanex.NAP.CTS.Service
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Account;
    using Fanex.NAP.CTS.Criteria.Account.CustomerCategory;
    using Fanex.NAP.CTS.Criteria.Account.CustomerInfo;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Model.Account.CustomerInfo;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using System;
    using System.Collections.Generic;

    public class AccountService : IAccountService
    {
        private readonly ICriteriaService _repository;

        public AccountService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public IEnumerable<CustInfoModel> GetCustInfoByCustID(string custIDs)
        {
            var criteria = new CustInfoByCustIdGetCriteria
            {
                CustIDs = custIDs
            };

            return _repository.Fetch<CustInfoModel>(criteria);
        }

        public CustomerInfoAssociationAccountModel GetCustomerInfoAssociationAccount(long ctsCustId)
        {
            var criteria = new GetCustomerInfoAssociatedAccountCriteria
            {
                CTSCustId = ctsCustId
            };

            return _repository.Get<CustomerInfoAssociationAccountModel>(criteria);
        }

        public IEnumerable<GetCustomerClassificationModel> GetCustomerClassificationForAPI(string custIdList, string parentCateIdList)
        {
            var criteria = new GetCTSCustomerClassificationForAPICriteria
            {
                CustIdList = custIdList,
                ParentCateIdList = parentCateIdList
            };

            return _repository.Fetch<GetCustomerClassificationModel>(criteria);
        }     

        public IEnumerable<CustomerClassificationGeneralInfoModel> GetCustomerClassificationByCategory(string categoryIdList)
        {
            var criteria = new GetCustomerCategoryByCategoryIdsCriteria
            {
                CategoryIDList = categoryIdList
            };

            return _repository.Fetch<CustomerClassificationGeneralInfoModel>(criteria);
        }

        public IEnumerable<CTSCustomer> CheckCustomerNotExisted(string custIds, DateTime fromDate, DateTime toDate)
        {
            var criteria = new CheckCustNotExistedByCustIdsCriteria
            {
                CustIds = custIds,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _repository.Fetch<CTSCustomer>(criteria);
        }

    }
}