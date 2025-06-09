namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using ONEbook.UM.Services;
    using Fanex.NAP.CTS.User.Helper;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;

    public class UnderMaintainaceHappenFilterAttribute : ActionFilterAttribute
    {
        private static IServiceProvider ServiceProvider;
        private IMaintenanceService _maintenanceService;

        public static void Setup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public UnderMaintainaceHappenFilterAttribute()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                _maintenanceService = scope.ServiceProvider.GetRequiredService<IMaintenanceService>();
            }
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