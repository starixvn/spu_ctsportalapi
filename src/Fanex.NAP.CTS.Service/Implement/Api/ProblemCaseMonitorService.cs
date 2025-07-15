namespace Fanex.NAP.CTS.Service.Implement.Api
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.Api.ProblemCaseMonitor;
    using Fanex.NAP.CTS.Model.API.ProblemCaseMonitor;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using System.Collections.Generic;

    public class ProblemCaseMonitorService : IProblemCaseMonitorService
    {
        private readonly ICriteriaService _repository;

        public ProblemCaseMonitorService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProblemCaseModel> GetProlemCase(int listId, bool status)
        {
            var criteria = new ProblemCaseCriteria
            {
                ListId = listId,
                Status = status
            };

            return _repository.Fetch<ProblemCaseModel>(criteria);
        }
    }
}
