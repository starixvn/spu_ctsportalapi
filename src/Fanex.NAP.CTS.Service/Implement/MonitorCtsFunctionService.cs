namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Core;
    using Fanex.NAP.CTS.Criteria.MonitorFunction;
    using Fanex.NAP.CTS.Service.Interface;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    public class MonitorCtsFunctionService : IMonitorCtsFunctionService
    {
        private readonly ICriteriaService _repository;

        public MonitorCtsFunctionService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public DBResult StatsAccessmentInsert(SystemId systemId, TrackingCtsFunctionId funcId, int userId)
        {
            var criteria = new StatsAccessmentInsertCriteria
            {
                SystemId = (long) systemId,
                FunctionId = (int) funcId,
                UserId = userId
            };

            _repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = string.IsNullOrEmpty(criteria.ErrorMessage) ? 0 : 1,
                ErrorMsg = criteria.ErrorMessage
            };
        }
    }
}