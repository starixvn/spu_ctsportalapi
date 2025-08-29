namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.MatchMonitor;
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Service.Interface;
    using Fanex.NAP.CTS.ViewModel.MatchMonitor;

    public class MatchMonitorService : IMatchMonitorService
    {
        private readonly ICriteriaService _repository;

        public MatchMonitorService(ICriteriaService repository)
        {
            _repository = repository;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S6566:Use DateTimeOffset instead of DateTime", Justification = "Legacy code or specific reason")]
        public IEnumerable<MatchFromMainInfoViewModel> GetMatchFromMainInfos(GetMatchFromMainInfoModel model)
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

            return _repository.Fetch<MatchFromMainInfoViewModel>(criteria);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S6566:Use DateTimeOffset instead of DateTime", Justification = "Legacy code or specific reason")]
        public IEnumerable<GetMatchMonitorViewModel> GetMatchMonitor(MatchMonitorGetModel model)
        {
            var criteria = new MatchMonitorGetCriteria
            {
                FromDate = model?.FromDate ?? DateTime.Now,
                ToDate = model?.ToDate ?? DateTime.Now,
                SportType = model.SportType.IsNotNullOrEmpty() ? model.SportType : "[]",
                Market = model.Market,
                BetTypes = model.BetTypes.IsNotNullOrEmpty() ? model.BetTypes : "[]",
                IsVerified = model.IsVerified,
                Reasons = model.Reasons,
                MatchInfosJson = model.MatchInfosJson
            };

            return _repository.Fetch<GetMatchMonitorViewModel>(criteria);
        }

        public IEnumerable<MMFraudTicketsSummaryViewModel> GetMMFraudTicketsSummary(MMFraudTicketsSummaryModel model)
        {
            var criteria = new MMFraudTicketsSummaryCriteria
            {
                FraudTickets = model.FraudTickets
            };

            return _repository.Fetch<MMFraudTicketsSummaryViewModel>(criteria);
        }

        public IEnumerable<MMFraudTicketsSummaryViewModel> GetMMParlayFraudTicketsSummary(MMParlayFraudTicketsSummaryModel model)
        {
            var criteria = new MMParlayFraudTicketsSummaryCriteria
            {
                FraudTickets = model.FraudTickets
            };

            return _repository.Fetch<MMFraudTicketsSummaryViewModel>(criteria);
        }
    }
}
