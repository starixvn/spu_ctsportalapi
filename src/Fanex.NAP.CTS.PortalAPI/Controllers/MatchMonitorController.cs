namespace Fanex.NAP.CTS.PortalAPI.Controllers
{
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Interface;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilterAttribute))]
    public class MatchMonitorController : ControllerBase
    {
        private readonly IMatchMonitorService _matchMonitorService;

        public MatchMonitorController(IMatchMonitorService matchMonitorService)
        {
            _matchMonitorService = matchMonitorService;
        }

        [HttpPost]
        public IActionResult GetMatchFromMainInfo([FromBody] GetMatchFromMainInfoModel model)
        {
            var result = _matchMonitorService.GetMatchFromMainInfos(model);
            return Ok(result);
        }
    }
}
