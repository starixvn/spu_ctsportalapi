namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.MatchMonitor;
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Service.Interface;

    public class MatchMonitorService : IMatchMonitorService
    {
        private readonly ICriteriaService _repository;

        public MatchMonitorService(ICriteriaService repository)
        {
            _repository = repository;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S6566:Use DateTimeOffset instead of DateTime", Justification = "Legacy code or specific reason")]
        public IEnumerable<MatchFromMainInfoModel> GetMatchFromMainInfos(GetMatchFromMainInfoModel model)
        {
            var criteria = new GetMatchFromMainInfoCriteria
            {
                MatchIDs = model.MatchIDs,
                SportList = model.SportList,
                FromDate = model?.FromDate ?? DateTime.Now,
                ToDate = model?.ToDate ?? DateTime.Now,
                MatchSection = model.MatchSection,
                Event = model.Event
            };

            return _repository.Fetch<MatchFromMainInfoModel>(criteria);
        }
    }
}
