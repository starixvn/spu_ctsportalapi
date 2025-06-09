namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.NAP.CTS.Model.DTO;
    using Fanex.NAP.CTS.Service.Interface;
    using System.Collections.Generic;
    using Fanex.NAP.CTS.Criteria.Api;
    using Fanex.NAP.CTS.Model.Account;
    using Fanex.NAP.CTS.Core.Service;

    public class AssociatedAccountService : IAssociatedAccountService
    {
        private readonly ICriteriaService _repository;

        public AssociatedAccountService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public IEnumerable<CustomerEvidenceForTW> GetFlaggedEvidenceByDirectAssForAPI(string custIdList)
        {
            var criteria = new GetFlaggedEvidenceByDirectAssociationCriteria()
            {
                CustIds = custIdList
            };

            return _repository.Fetch<CustomerEvidenceForTW>(criteria);
        }

        public IEnumerable<CustomerEvidenceForTW> GetFlaggedEvidenceByLevel2AssForAPI(string custIdList)
        {
            var criteria = new GetFlaggedEvidenceByLevel2AssociationCriteria()
            {
                CustIds = custIdList
            };

            return _repository.Fetch<CustomerEvidenceForTW>(criteria);
        }

        public IEnumerable<AssociatedAccountModel> GetAssociationForExtAcc(int custId)
        {
            var criteria = new GetAssociationForExtAccCriteria()
            {
                CustId = custId,
                AssStatus = 1,
                AssType = 1
            };

            return _repository.Fetch<AssociatedAccountModel>(criteria);
        }
    }
}