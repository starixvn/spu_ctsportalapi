namespace Fanex.NAP.CTS.Service.Implement.Api
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.TW;
    using Fanex.NAP.CTS.Model.API.TWInitialSmart;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class TWInitialSmartService : ITWInitialSmartService
    {
        private readonly ICriteriaService _repository;

        public TWInitialSmartService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public IEnumerable<long> AddCustTWInitialSmarts(IEnumerable<InitialSmartCustInfoModel> initialSmartCustInfos)
        {
            var criteria = new InsertTWInitialSmartCriteria
            {
                CustInfoJson = JsonConvert.SerializeObject(initialSmartCustInfos)
            };

            return _repository.Fetch<long>(criteria);
        }

    }
}
