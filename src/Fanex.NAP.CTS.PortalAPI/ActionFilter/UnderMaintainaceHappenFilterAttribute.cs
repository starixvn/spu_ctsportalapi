namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using ONEbook.UM.Services;
    using Fanex.NAP.CTS.User.Helper;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    public class UnderMaintainaceHappenFilterAttribute : ActionFilterAttribute
    {
        private IMaintenanceService _maintenanceService;

        public UnderMaintainaceHappenFilterAttribute(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // TODO comment for check API in UM
            //if (UMHelper.IsUnderMaintainaceHappen(_maintenanceService))
            //{
            //    filterContext.Result = new ContentResult()
            //    {
            //        Content = JsonConvert.SerializeObject("System is under maintenance, please try again later."),
            //        StatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
            //        ContentType = "application/json"
            //    };

            //    return;
            //}
            // TODO comment for check API in UM

            base.OnActionExecuting(filterContext);
        }
    }
}