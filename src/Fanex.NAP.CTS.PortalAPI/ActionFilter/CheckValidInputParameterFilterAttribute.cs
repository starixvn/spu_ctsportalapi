namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging;
    using Newtonsoft.Json;
    using System.Linq;
    using Fanex.NAP.CTS.Core.Extentions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    public class CheckValidInputParameterFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var invalidModelState = filterContext.ModelState.Values
                    .Where(x => x.Errors.Count > 0)?.SelectMany(x => x.Errors?.Select(y => y.Exception.Message));

                var invalidJson = invalidModelState.IsNotNullOrEmpty() ? JsonConvert.SerializeObject(invalidModelState) : string.Empty;

                var logMessage = new LogMessage("Invalid Input Parameter")
                    .AddMessage(invalidJson)
                    .ToString();

                LogService.Error(logMessage);

                filterContext.Result = new ContentResult
                {
                    Content = invalidJson,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json"
                };

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}