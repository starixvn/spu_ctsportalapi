namespace Fanex.NAP.CTS.ViewModel.MatchMonitor
{
    public class MMFraudTicketsSummaryViewModel
    {
        public Int64 MatchId { get; set; }

        public bool IsLive { get; set; }

        public int Bettype { get; set; }

        public string Betteam { get; set; }

        public int? BetId { get; set; }

        public long Turnover { get; set; }

        public long BetCount { get; set; }
    }
}
