namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using ONEbook.UM;
    using ONEbook.UM.Services;
    using Fanex.NAP.CTS.User.Helper;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;

    public class UnderMaintainaceHappenFilterAttribute : ActionFilterAttribute
    {
        private IMaintenanceService _maintenanceService;

        public UnderMaintainaceHappenFilterAttribute()
        {
            _maintenanceService = new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UMHelper.IsUnderMaintainaceHappen(_maintenanceService))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "System is under maintenance, please try again later.",
                    StatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
                    ContentType = "application/json"
                };

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}