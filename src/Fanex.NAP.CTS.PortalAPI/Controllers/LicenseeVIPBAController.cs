namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Model.API;
    using Fanex.NAP.CTS.Model.LogInputData;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    [ApiController]
    [Route("api/cts/[action]")]
    public class LicenseeVIPBAController : ControllerBase
    {
        private readonly ILicenseeVIPBAService licenseeLAPCustomerClassificationService;
        private readonly ILogInputDataService logInputDataService;

        public LicenseeVIPBAController(ILicenseeVIPBAService licenseeLAPCustomerClassificationService, ILogInputDataService logInputDataService)
        {
            this.licenseeLAPCustomerClassificationService = licenseeLAPCustomerClassificationService;
            this.logInputDataService = logInputDataService;
        }



        /// <summary>
        /// Add Licencee VIP
        /// </summary>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.AddLicVIP_API)]
        public IActionResult AddLicVIP([FromBody] LicenseeVIPBAInputModel parameters)
        {
            try
            {
                logInputDataService.LogInputData(new LogInputViewModel { LicenseeVIPBAModel = new LicenseeVIPBALogDataModel { Data = parameters, FromSystem = parameters.IsFromExternal ? FromSystem.External : FromSystem.Internal } }, FunctionId.AddLicVIP_API);

                if (!licenseeLAPCustomerClassificationService.CheckValid(parameters))
                {
                    LogExtension.LogError($"CTS API - AddLicVIP - Invalid input parameter from LAP: {JsonConvert.SerializeObject(parameters)} - DateTime {DateTime.UtcNow} UTC");

                    return BadRequest($"Invalid input parameter. Data: {JsonConvert.SerializeObject(new { parameters.CustIDs, parameters.CreatedBy })}");
                }

                licenseeLAPCustomerClassificationService.InsertLicVIP(parameters);
            }
            catch (Exception ex)
            {
                ex.LogError($"CTS API - AddLicVIP");

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Remove Licencee VIP
        /// </summary>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.RemoveLicVIP_API)]
        public IActionResult RemoveLicVIP([FromBody] LicenseeVIPBAInputModel parameters)
        {
            try
            {
                logInputDataService.LogInputData(new LogInputViewModel { LicenseeVIPBAModel = new LicenseeVIPBALogDataModel { Data = parameters, FromSystem = FromSystem.External } }, FunctionId.RemoveLicVIP_API);

                if (!licenseeLAPCustomerClassificationService.CheckValid(parameters))
                {
                    LogExtension.LogError($"CTS API - RemoveLicVIP - Invalid input parameter from LAP: {JsonConvert.SerializeObject(parameters)} - DateTime {DateTime.UtcNow} UTC");

                    return BadRequest($"Invalid input parameter. Data: {JsonConvert.SerializeObject(new { parameters.CustIDs, parameters.CreatedBy })}");
                }

                licenseeLAPCustomerClassificationService.RemoveLicVIP(parameters);
            }
            catch (Exception ex)
            {
                ex.LogError($"CTS API - RemoveLicVIP");

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Add Licencee BA
        /// </summary>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.AddLicVIP_API)]
        public IActionResult AddLicBA([FromBody] LicenseeVIPBAInputModel parameters)
        {
            try
            {
                logInputDataService.LogInputData(new LogInputViewModel { LicenseeVIPBAModel = new LicenseeVIPBALogDataModel { Data = parameters, FromSystem = parameters.IsFromExternal ? FromSystem.External : FromSystem.Internal } }, FunctionId.AddLicBA_API);

                if (!licenseeLAPCustomerClassificationService.CheckValid(parameters))
                {
                    LogExtension.LogError($"CTS API - AddLicBA - Invalid input parameter from LAP: {JsonConvert.SerializeObject(parameters)} - DateTime {DateTime.UtcNow} UTC");

                    return BadRequest($"Invalid input parameter. Data: {JsonConvert.SerializeObject(new { parameters.CustIDs, parameters.CreatedBy })}");
                }

                licenseeLAPCustomerClassificationService.InsertLicBA(parameters);
            }
            catch (Exception ex)
            {
                ex.LogError($"CTS API - AddLicBA");

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Remove Licencee BA
        /// </summary>
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.RemoveLicBAP_API)]
        public IActionResult RemoveLicBA([FromBody] LicenseeVIPBAInputModel parameters)
        {
            try
            {
                logInputDataService.LogInputData(new LogInputViewModel { LicenseeVIPBAModel = new LicenseeVIPBALogDataModel { Data = parameters, FromSystem = FromSystem.External } }, FunctionId.RemoveLicBA_API);

                if (!licenseeLAPCustomerClassificationService.CheckValid(parameters))
                {
                    LogExtension.LogError($"CTS API - RemoveLicBA - Invalid input parameter from LAP: {JsonConvert.SerializeObject(parameters)} - DateTime {DateTime.UtcNow} UTC");

                    return BadRequest($"Invalid input parameter. Data: {JsonConvert.SerializeObject(new { parameters.CustIDs, parameters.CreatedBy })}");
                }

                licenseeLAPCustomerClassificationService.RemoveLicBA(parameters);
            }
            catch (Exception ex)
            {
                ex.LogError($"CTS API - RemoveLicBA");

                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

            return Ok();
        }

    }
}