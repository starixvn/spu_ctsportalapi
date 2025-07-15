namespace Fanex.NAP.CTS.Service
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.CTS.Criteria.CustomerClassification;
    using Fanex.NAP.CTS.Model.CustomerClassification;
    using Fanex.NAP.CTS.Core.Extentions;

    public class DataLogTrackingService : IDataLogTrackingService
    {
        private readonly ICriteriaService _repository;

        public DataLogTrackingService(ICriteriaService repository)
        {
            _repository = repository;
        }

        public void LogErrorData(Exception exception, string serviceName)
        {
            try
            {
                var logInfo = new LogErrorDataModel
                {
                    Message =  exception.CreateLogMessage(serviceName),
                    FromFlow = serviceName,
                    SPName = DatabaseInfoGetters.GetDbExceptionInfo(exception)?.Where(c => c.Key == "SpName")?.FirstOrDefault().Value?.ToString(),
                    Parameters = DatabaseInfoGetters.GetDbExceptionInfo(exception)?.Where(c => c.Key == "Parameters")?.FirstOrDefault().Value?.ToString(),
                };

                LogErrorData(logInfo);
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }

        private void LogErrorData(LogErrorDataModel logInfo)
        {
            var criteria = new InputErrorLogInsertCriteria
            {
                LogInfo = JsonConvert.SerializeObject(logInfo)
            };

            _repository.Execute(criteria);
        }
    }
}