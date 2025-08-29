namespace Fanex.NAP.CTS.Service.Interface
{
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.ViewModel.MatchMonitor;

    public interface IMatchMonitorService
    {
        IEnumerable<MatchFromMainInfoViewModel> GetMatchFromMainInfos(GetMatchFromMainInfoModel model);

        IEnumerable<GetMatchMonitorViewModel> GetMatchMonitor(MatchMonitorGetModel model);

        IEnumerable<MMFraudTicketsSummaryViewModel> GetMMFraudTicketsSummary(MMFraudTicketsSummaryModel model);

        IEnumerable<MMFraudTicketsSummaryViewModel> GetMMParlayFraudTicketsSummary(MMParlayFraudTicketsSummaryModel model);
    }
}
