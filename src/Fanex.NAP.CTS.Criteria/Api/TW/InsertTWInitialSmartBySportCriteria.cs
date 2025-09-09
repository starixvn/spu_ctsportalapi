namespace Fanex.NAP.CTS.Criteria.Api.TW
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class InsertTWInitialSmartBySportCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustInfo")]
        public string CustInfoJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_InitialSmart_BySport_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
