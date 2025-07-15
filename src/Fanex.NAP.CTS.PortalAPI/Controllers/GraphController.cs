namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.NAP.CTS.Model.DTO;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    /// <summary>
    /// Graph Controller - Using for CTS site
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GraphController : ControllerBase
    {
        #region API - Flagged Evidence Level

        /// <summary>
        /// API Get GetFlaggedEvidenceForLevel01 For Taiwan
        /// </summary>
        /// <param name="custIdList"></param>
        /// <returns></returns>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetFlaggedEvidenceForLevel01API)]
        public IActionResult GetFlaggedEvidenceForLevel01(string custIdList)
        {
            //var result = new List<CustomerEvidenceForTW>();

            //try
            //{
            //    if (!string.IsNullOrEmpty(custIdList))
            //    {
            //        result = associationService.GetFlaggedEvidenceByDirectAssForAPI(custIdList)?.ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogService.Error($"WEB API - GetFlaggedEvidenceForLevel01 - Data input: {custIdList} - failed: " + ex.Message + ex.StackTrace);
            //    throw;
            //}

            //return Json(result ?? new List<CustomerEvidenceForTW>());

            return Ok(new List<CustomerEvidenceForTW>());
        }

        /// <summary>
        /// API Get GetFlaggedEvidenceForLevel2 For Taiwan
        /// </summary>
        /// <param name="custIdList"></param>
        /// <returns></returns>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetFlaggedEvidenceForLevel2API)]
        public IActionResult GetFlaggedEvidenceForLevel2(string custIdList)
        {
            //var result = new List<CustomerEvidenceForTW>();

            //try
            //{
            //    if (!string.IsNullOrEmpty(custIdList))
            //    {
            //        result = associationService.GetFlaggedEvidenceByLevel2AssForAPI(custIdList)?.ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogService.Error($"WEB API - GetFlaggedEvidenceForLevel2 - Data input: {custIdList} - failed: " + ex.Message + ex.StackTrace);
            //    throw;
            //}

            //return Json(result ?? new List<CustomerEvidenceForTW>());

            return Ok(new List<CustomerEvidenceForTW>());
        }

        #endregion API - Flagged Evidence Level
    }
}