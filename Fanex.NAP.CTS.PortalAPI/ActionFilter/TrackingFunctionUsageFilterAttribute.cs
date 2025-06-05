namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using Fanex.Logging;
    using Fanex.NAP.CTS.Service.Interface;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    /// <summary>
    /// ActionFilter for tracking CTSFunction Usage
    /// </summary>
    public class TrackingFunctionUsageFilterAttribute : ActionFilterAttribute
    {

        public TrackingCtsFunctionId FunctionId { get; set; }


        public SystemId SystemId { get; set; }

        private static IMonitorCtsFunctionService MonitorService;

        public static void Setup(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                MonitorService = scope.ServiceProvider.GetRequiredService<IMonitorCtsFunctionService>();
            }
        }

        public TrackingFunctionUsageFilterAttribute(SystemId systemId, TrackingCtsFunctionId functionId)
        {
            this.FunctionId = functionId;
            this.SystemId = systemId;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                MonitorService.StatsAccessmentInsert(SystemId, FunctionId, 0);
            }
            catch (Exception ex)
            {
                LogService.Error($"Tracking APIs Usage - Insert Stats Accessment fail - {ex.Message}");
            }
        }
    }
}