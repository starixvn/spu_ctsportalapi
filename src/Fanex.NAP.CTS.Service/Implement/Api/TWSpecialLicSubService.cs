namespace Fanex.NAP.CTS.Service.Implement.Api
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.TWSpecialLicSub;
    using Fanex.NAP.CTS.Model.API.TWSpecialLicSub;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using System.Linq;

    public class TWSpecialLicSubService : ITWSpecialLicSubService
    {
        private readonly ICriteriaService _repository;

        public TWSpecialLicSubService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public InsertTWSpecialLicSubEntity InsertTWSpecialLicSub(ForceTWLicSubCCModel model)
        {
            var criteria = new InsertTWSpecialLicSubCriteria
            {
                Custid = model.custid,
                CustomerClass = model.customerClass
            };

            return _repository.Fetch<InsertTWSpecialLicSubEntity>(criteria)?.FirstOrDefault() ?? new InsertTWSpecialLicSubEntity();
        }
    }
}
