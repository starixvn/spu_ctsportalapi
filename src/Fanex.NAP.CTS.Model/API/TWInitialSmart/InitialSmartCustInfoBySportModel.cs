namespace Fanex.NAP.CTS.Model.API.TWInitialSmart
{
    public class InitialSmartCustInfoBySportModel
    {
        public long CustID { get; set; }

        public int SportType { get; set; }

        public decimal Probability { get; set; }

        public DateTime PredictTime { get; set; }
    }
}
