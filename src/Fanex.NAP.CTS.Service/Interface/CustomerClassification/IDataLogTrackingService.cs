namespace Fanex.NAP.CTS.Service
{
    using System;

    public interface IDataLogTrackingService
    {
        void LogErrorData(Exception exception, string serviceName);
    }
}
