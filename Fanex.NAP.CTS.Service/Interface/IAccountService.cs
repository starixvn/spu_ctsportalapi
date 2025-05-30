namespace Fanex.NAP.CTS.Service
{
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Model.Account.CustomerInfo;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using System;
    using System.Collections.Generic;

    public interface IAccountService
    {
        IEnumerable<CustInfoModel> GetCustInfoByCustID(string custIDs);

        CustomerInfoAssociationAccountModel GetCustomerInfoAssociationAccount(long ctsCustId);

        IEnumerable<GetCustomerClassificationModel> GetCustomerClassificationForAPI(string custIdList, string parentCateIdList);

        IEnumerable<CustomerClassificationGeneralInfoModel> GetCustomerClassificationByCategory(string categoryIdList);

        IEnumerable<CTSCustomer> CheckCustomerNotExisted(string custIds, DateTime fromDate, DateTime toDate);
    }
}