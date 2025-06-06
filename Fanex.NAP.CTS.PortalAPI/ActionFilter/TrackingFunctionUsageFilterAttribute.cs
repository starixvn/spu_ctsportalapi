namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using Fanex.Logging;
    using Fanex.NAP.CTS.Service.Interface;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    /// <summary>
    /// ActionFilter for tracking CTSFunction Usage
    /// </summary>
    public class TrackingFunctionUsageFilterAttribute : ActionFilterAttribute
    {

        public TrackingCtsFunctionId FunctionId { get; set; }


        public SystemId SystemId { get; set; }

        private IMonitorCtsFunctionService _monitorService;
        private static IServiceProvider ServiceProvider;

        public static void Setup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public TrackingFunctionUsageFilterAttribute(SystemId systemId, TrackingCtsFunctionId functionId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                _monitorService = scope.ServiceProvider.GetRequiredService<IMonitorCtsFunctionService>();
            }

            this.FunctionId = functionId;
            this.SystemId = systemId;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                _monitorService.StatsAccessmentInsert(SystemId, FunctionId, 0);
            }
            catch (Exception ex)
            {
                LogService.Error($"Tracking APIs Usage - Insert Stats Accessment fail - {ex.Message}");
            }
        }
    }
}