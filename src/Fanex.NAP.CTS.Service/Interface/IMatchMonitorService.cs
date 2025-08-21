namespace Fanex.NAP.CTS.Service.Interface
{
    using Fanex.NAP.CTS.Model.MatchMonitor;

    public interface IMatchMonitorService
    {
        IEnumerable<MatchFromMainInfoModel> GetMatchFromMainInfos(GetMatchFromMainInfoModel model);
    }
}
