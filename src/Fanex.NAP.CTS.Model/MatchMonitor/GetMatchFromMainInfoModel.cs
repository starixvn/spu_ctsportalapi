namespace Fanex.NAP.CTS.Model.MatchMonitor
{
    public class GetMatchFromMainInfoModel
    {
        public string? MatchIDs { get; set; }

        public string? SportList { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string MatchSection { get; set; }

        public string? Event { get; set; }
    }
}
