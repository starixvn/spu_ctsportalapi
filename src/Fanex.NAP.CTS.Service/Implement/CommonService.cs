namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.Data;
    using Fanex.NAP.CTS.Criteria.Common;
    using Fanex.NAP.CTS.Service.Interface;

    public class CommonService : ICommonService
    {
        public void CheckConnection()
        {
            using (var db = ObjectDbFactory.CreateInstance(new HealthCheckCriteria().GetSettingKey()))
            {
                var connection = db.CreateDbConnection();
                connection.Open();
                connection.Close();
            }
        }
    }
}