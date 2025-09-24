namespace Fanex.NAP.CTS.PortalAPI.Controllers
{
    using Fanex.NAP.CTS.Model.MatchMonitor;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.Service.Interface;
    using Fanex.NAP.CTS.ViewModel.AssociationDetection;
    using Fanex.NAP.CTS.ViewModel.MatchMonitor;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilterAttribute))]
    public class MatchMonitorController : ControllerBase
    {
        private readonly IMatchMonitorService _matchMonitorService;
        private readonly IAssociationDetectionService _associationDetectionService;

        public MatchMonitorController(IMatchMonitorService matchMonitorService, IAssociationDetectionService associationDetectionService)
        {
            _matchMonitorService = matchMonitorService;
            _associationDetectionService = associationDetectionService;
        }

        [HttpPost]
        public IActionResult GetMatchFromMainInfo([FromBody] GetMatchFromMainInfoModel model)
        {
            var result = _matchMonitorService.GetMatchFromMainInfos(model);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetMatchMonitor([FromBody] MatchMonitorGetModel model)
        {
            var result = _matchMonitorService.GetMatchMonitor(model);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetMMFraudTicketsSummary([FromBody] MMFraudTicketsSummaryModel model)
        {
            var result = _matchMonitorService.GetMMFraudTicketsSummary(model);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetMMParlayFraudTicketsSummary([FromBody] MMParlayFraudTicketsSummaryModel model)
        {
            var result = _matchMonitorService.GetMMParlayFraudTicketsSummary(model);
            return Ok(result);
        }

        #region Association Detection
        [HttpPost]      
        public ActionResult DetectGroupAssWithOptions(DetectGroupAssFilterViewModel model)
        {           
            var orgUserList = _associationDetectionService.ValidateUserName(model.RawUserNames);

            var data = _associationDetectionService.DetectAssociation(new DetectAssociationByUserNamesViewModel
            {
                UserNames = model.RawUserNames,
                IsBP = model.IsOptBP,
                IsDevice = model.IsOptDevice,
                IsIP = model.IsOptIP,
                IsAgent = model.IsOptAgent,
                IsIPLast3Days = model.IsOptIPLast3Days,
                Is3MatchesLast7Days = model.IsOpt3MatchesLast7Days
            }, orgUserList, false).Select(x => new DetectGroupAssociationItemViewModel(x));

            var result = new DetectGroupAssociationListViewModel()
            {
                ItemList = RearrangeAssGroup(data, true)
            };
           
            return Ok(result);
        }

        private IEnumerable<DetectGroupAssociationItemViewModel> RearrangeAssGroup(IEnumerable<DetectGroupAssociationItemViewModel> assGroupList, bool isDisableNone)
        {
            var result = new List<DetectGroupAssociationItemViewModel>();

            if (isDisableNone)
                assGroupList = assGroupList.Where(x => x.GroupID != 0);

            if (assGroupList.Any())
            {
                var grouping = assGroupList
                    .GroupBy(x => x.GroupID, (key, g) => new { GroupID = key, Items = g.ToList() })
                    .OrderBy(x => x.GroupID);

                var idx = 1;

                foreach (var group in grouping)
                {
                    var groupID = group.GroupID;
                    var groupName = group.GroupID.ToString();

                    if (groupID.HasValue)
                    {
                        if (groupID == 0)
                        {
                            groupID = -1;
                            groupName = "None";
                        }
                        else
                        {
                            groupID = idx;
                            groupName = idx.ToString();
                            idx += 1;
                        }
                    }

                    result.AddRange(group.Items.Select(x =>
                    {
                        x.GroupID = groupID;
                        x.GroupName = groupName;

                        return x;
                    }));
                }
            }

            return result;
        }
        #endregion

    }
}
