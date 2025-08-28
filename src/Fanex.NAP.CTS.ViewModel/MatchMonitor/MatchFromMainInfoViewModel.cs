namespace Fanex.NAP.CTS.ViewModel.MatchMonitor
{
    public class MatchFromMainInfoViewModel
    {
        public int MatchId { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime KickOffTime { get; set; }

        public int MatchSection { get; set; }

        public bool IsGetDataFromOrigin { get; set; }

        public short DataSource { get; set; }

        public string LeagueName { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }
    }
}
