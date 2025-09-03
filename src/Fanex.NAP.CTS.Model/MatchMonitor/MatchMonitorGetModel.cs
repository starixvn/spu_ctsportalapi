namespace Fanex.NAP.CTS.Model.MatchMonitor
{
    public class MatchMonitorGetModel
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? SportType { get; set; }

        public int? Market { get; set; }

        public int? IsVerified { get; set; }

        public string? BetTypes { get; set; }

        public string? Reasons { get; set; }

        public string? MatchInfosJson { get; set; }
    }
}
