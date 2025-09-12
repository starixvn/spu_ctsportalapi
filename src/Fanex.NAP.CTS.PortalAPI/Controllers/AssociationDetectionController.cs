namespace Fanex.NAP.CTS.PortalAPI.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Model.AssociationDetection;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.ViewModel.AssociationDetection;
    using Microsoft.AspNetCore.Mvc;

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
            var result = DetectAssociation(model);

            return Ok(result);
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
