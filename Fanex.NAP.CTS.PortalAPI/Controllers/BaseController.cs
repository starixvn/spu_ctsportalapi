namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.ViewModel.API.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [UnderMaintainaceHappenFilter]
    public abstract class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        [NonAction]
        protected IActionResult Result<T>(T detail, string message = "")
        {
            var result = new HttpResponseViewModel<T>
            {
                statusCode = message.IsNullOrEmpty() ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                errorMessage = message,
                detail = detail
            };

            return Ok(result);
        }

        [NonAction]
        protected IActionResult Json<T>(T data)
        {
            return Ok(data);
        }
    }
}
