namespace Fanex.NAP.CTS.PortalAPI.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Criteria.AssociationDetection;
    using Fanex.NAP.CTS.Model.AssociationDetection;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.ViewModel.AssociationDetection;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilterAttribute))]
    public class AssociationDetectionController : ControllerBase
    {
        private readonly IAssociationDetectionService _associationDetectionService;

        public AssociationDetectionController(IAssociationDetectionService associationDetectionService)
        {
            _associationDetectionService = associationDetectionService;
        }

        [HttpPost]
        public IActionResult DetectAssociationByUserNames([FromBody] DetectAssociationByUserNamesViewModel model)
        {
            var result = new DetectAssociationViewModel();

            try
            {
                result = DetectAssociation(model);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 1;
                result.ErrorMessage = ex.Message;

                ex.LogError();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PrepareDataExportDetectAssociation([FromBody] DetectAssociationByUserNamesViewModel model)
        {
            var result = new DetectAssociationViewModel();

            try
            {
                if (!string.IsNullOrWhiteSpace(model.UserNames))
                {
                    var orgUserList = _associationDetectionService.ValidateUserName(model.UserNames);

                    var assMergeGroup = _associationDetectionService.DetectAssociation(model, orgUserList, true);

                    var assRearrangeGroup = RearrangeAssGroup(assMergeGroup);

                    var custIDs = string.Join(",", orgUserList.Select(x => x.CustID).Distinct());

                    var custStatisticsList = _associationDetectionService.GetCustStatisticsInfoRealtime(custIDs);

                    result.ExportItemList = GetExportItem(orgUserList, assRearrangeGroup, custStatisticsList);
                }
            }
            catch (Exception ex)
            {
                ex.LogError();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult DetectAssociationCustByUserRoot([FromBody] DetectAssociationByUserRootFilterModel assFilter)
        {            
            try
            {
                var assWithRootCust = _associationDetectionService.GetAssociationCustByUserRoot(assFilter);

                var result = JsonConvert.SerializeObject(assWithRootCust);

                return Ok(result);
            }
            catch (Exception ex)
            {               
                ex.LogError();

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Failed to detect association",
                    error = ex.Message
                });
            }            
        }

        private IEnumerable<ExportAssociationDetectModel> GetExportItem(IEnumerable<ValidateUserNameModel> orgUserList
            , IEnumerable<DetectAssociationMergeGroupModel> assMergeGroup
            , IEnumerable<GetCustStatisticsInfoRealtimeModel> custStatisticsList)
        {
            if (assMergeGroup.Any())
            {
                var assMergeGroupDictionary = assMergeGroup.Distinct(x => x.CustID, y => y);
                var custStatisticsDictionary = custStatisticsList.Distinct(x => x.CustID, y => y);

                return orgUserList.Select(x =>
                {
                    var cust1 = assMergeGroupDictionary.GetValueOrDefault(x.CustID);
                    var cust2 = custStatisticsDictionary.GetValueOrDefault(x.CustID);

                    var item = new ExportAssociationDetectModel { UserName = x.UserName };
                    if (cust1 != null)
                    {
                        item.CreatedDate = cust1.CreatedDate;
                        item.CustomerClass = cust1.CustomerClass;
                        item.CustomerCategory = cust1.CustomerCategory;
                        item.GroupID = cust1.GroupID;
                        item.Danger1 = cust1.Danger1;
                        item.Danger2 = cust1.Danger2;
                        item.Danger3 = cust1.Danger3;
                        item.Danger4 = cust1.Danger4;
                        item.Danger5 = cust1.Danger5;
                        item.BotTransSum = cust1.BotTransSum;
                        item.BotTransPercentage = cust1.BotTransPercentage;
                        item.InvalidBrowserInfoTransSum = cust1.InvalidBrowserInfoTransSum;
                        item.InvalidBrowserInfoTransPercentage = cust1.InvalidBrowserInfoTransPercentage;
                    }
                    if (cust2 != null)
                    {
                        item.TotalSettledTickets = cust2.TotalSettledTickets;
                        item.TotalWinloss = cust2.TotalWinlossRM;
                        item.WinlossLast3Days = cust2.WinlossRM_Last3Days;
                        item.WinlossLast7Days = cust2.WinlossRM_Last7Days;
                        item.TotalSabaTurnoverRM = cust2.TotalSabaTurnoverRM;
                        item.SabaTurnoverRMLast3Days = cust2.SabaTurnoverRM_Last3Days;
                        item.SabaTurnoverRMLast7Days = cust2.SabaTurnoverRM_Last7Days;
                        item.TotalTurnover = cust2.TotalTurnoverRM;
                        item.TurnoverLast3Days = cust2.TurnoverRM_Last3Days;
                        item.TurnoverLast7Days = cust2.TurnoverRM_Last7Days;
                    }

                    return item;
                });
            }

            return new List<ExportAssociationDetectModel>();
        }

        private DetectAssociationViewModel DetectAssociation(DetectAssociationByUserNamesViewModel model)
        {
            var result = new DetectAssociationViewModel();

            if (!string.IsNullOrWhiteSpace(model.UserNames))
            {
                var orgUserList = _associationDetectionService.ValidateUserName(model.UserNames);

                var assMergeGroup = _associationDetectionService.DetectAssociation(model, orgUserList, true);

                var assMergeGroupHasGroupID = assMergeGroup?.Where(x => x.GroupID.HasValue);

                var assRearrangeGroup = RearrangeAssGroup(assMergeGroupHasGroupID);

                result.ItemList = assRearrangeGroup.Select(x =>
                {
                    return new DetectAssociationItemViewModel(x);
                });
            }

            return result;
        }

        private IEnumerable<DetectAssociationMergeGroupModel> RearrangeAssGroup(IEnumerable<DetectAssociationMergeGroupModel> assGroupList)
        {
            var result = new List<DetectAssociationMergeGroupModel>();

            if (assGroupList.IsNotNullOrEmpty())
            {
                var idx = 0;
                var groupIdDict = new Dictionary<string, int>();

                foreach (var item in assGroupList)
                {
                    var key = item.GroupID?.ToString() ?? string.Empty;
                    if (item.GroupID.HasValue && !groupIdDict.ContainsKey(item.GroupID?.ToString() ?? string.Empty))
                    {
                        idx++;
                        groupIdDict[key] = idx;
                    }

                    if (item.GroupID.HasValue)
                    {
                        item.GroupID = groupIdDict[key];
                    }
                }

                result.AddRange(assGroupList.OrderBy(x => x.GroupID));
            }

            return result;
        }
    }
}
