namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.Data;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Criteria.AssociationDetection;
    using Fanex.NAP.CTS.Criteria.Common;
    using Fanex.NAP.CTS.Model.AssociationDetection;

    public interface ICommonService
    {
        void CheckConnection();

        IEnumerable<ValidateUserNameModel> ValidateUsernames(string userNames);
    }

    public class CommonService : ICommonService
    {
        private readonly ICriteriaService _repository;

        public CommonService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public void CheckConnection()
        {
            using (var db = ObjectDbFactory.CreateInstance(new HealthCheckCriteria().GetSettingKey()))
            {
                var connection = db.CreateDbConnection();
                connection.Open();
                connection.Close();
            }
        }

        public IEnumerable<ValidateUserNameModel> ValidateUsernames(string userNames)
        {            
            var criteria = new ValidateUsernameCriteria
            {
                UserNames = userNames
            };

            return _repository.Fetch<ValidateUserNameModel>(criteria);
        }
    }
}