namespace Fanex.NAP.CTS.Portal.API.ActionFilter
{
    using Fanex.NAP.CTS.Core.Utilities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Threading.Tasks;

    public class ApiKeyAuthFilterAttribute : ActionFilterAttribute
    {
        private const string APIKEYNAME = "X-API-KEY";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasKey = context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey);

            if (!hasKey || !ConfigSettings.ApiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}