namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.Logging.Extensions;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Model.API.TWInitialSmart;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TWController : ControllerBase
    {
        private readonly ITWInitialSmartService _tWInitialSmartService;

        public TWController(ITWInitialSmartService tWInitialSmartService)
        {
            _tWInitialSmartService = tWInitialSmartService;
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.AddCustomerInitialSmarts)]
        public IActionResult AddCustomerEarlyBlock([FromBody] List<InitialSmartCustInfoModel> custInfos)
        {
            try
            {
                var dbExistedCusts = _tWInitialSmartService.AddCustTWInitialSmarts(custInfos);

                if (dbExistedCusts != null && dbExistedCusts.Any())
                {
                    var logMessage = new LogMessage($"CTS API - TW AddCustomerInitialSmarts cust have existed")
                                      .AddMessage($"Custs existed in DB: {JsonConvert.SerializeObject(dbExistedCusts)}")
                                      .AddMessage($"Data {JsonConvert.SerializeObject(custInfos)} - DateTime {DateTime.UtcNow} UTC").ToString();

                    LogExtension.LogError(logMessage);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage($"CTS API - TW AddCustomerInitialSmarts exception")
                                       .AddMessage(ex.Message)
                                       .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                                       .AddMessage($"Data {JsonConvert.SerializeObject(custInfos)} - DateTime {DateTime.UtcNow} UTC").ToString();

                LogExtension.LogError(logMessage);

                return BadRequest($"Add Customers Initial Smart failed.");
            }

            return Ok("Success");
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.AddCustomerInitialSmartsBySport)]
        public IActionResult AddCustomerEarlyBlockBySport([FromBody] List<InitialSmartCustInfoBySportModel> custInfos)
        {
            try
            {
                var dbExistedCusts = _tWInitialSmartService.AddCustTWInitialSmartsBySport(custInfos);

                if (dbExistedCusts != null && dbExistedCusts.Any())
                {
                    var logMessage = new LogMessage($"CTS API - TW AddCustomerInitialSmartsBySport cust have existed")
                                      .AddMessage($"Custs existed in DB: {JsonConvert.SerializeObject(dbExistedCusts)}")
                                      .AddMessage($"Data {JsonConvert.SerializeObject(custInfos)} - DateTime {DateTime.UtcNow} UTC").ToString();

                    LogExtension.LogError(logMessage);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage($"CTS API - TW AddCustomerInitialSmartsBySport exception")
                                       .AddMessage(ex.Message)
                                       .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                                       .AddMessage($"Data {JsonConvert.SerializeObject(custInfos)} - DateTime {DateTime.UtcNow} UTC").ToString();

                LogExtension.LogError(logMessage);

                return BadRequest($"Add Customers Initial Smart By Sport failed.");
            }

            return Ok("Success");
        }
    }
}