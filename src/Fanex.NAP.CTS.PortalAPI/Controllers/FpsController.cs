namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Model.API.FPS.Account;
    using Fanex.NAP.CTS.Model.API.FPS.Device;
    using Fanex.NAP.CTS.Model.API.FPS.Search;
    using Fanex.NAP.CTS.Model.API.FPS.SearchAccount;
    using Fanex.NAP.CTS.Model.API.FPS.Subscriber;
    using Fanex.NAP.CTS.Model.API.FPS.Transaction;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.NAP.CTS.Service.Interface;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.CTS.Resource.Consts.Consts;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FpsController : ControllerBase
    {
        private readonly IFpsService _fpsService;
        private readonly ISubscriberService _subscriberService;

        public FpsController(IFpsService fpsService, ISubscriberService subscriberService)
        {
            _fpsService = fpsService;
            _subscriberService = subscriberService;
        }

        #region dcs center
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.SearchAccountDCSCenter)]
        public IActionResult SearchAccountDCSCenter(SearchAccountParamModel param)
        {
            try
            {
                var data = _fpsService.SearchAccount_DCSCenter(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Search Account info DCS Center exception: " + ex.Message);
                return Ok(new SearchAccountResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountInfoByCustSubscriberDCSCenter)]
        public IActionResult GetAssAccountInfoByCustSubscriberDCSCenter(GetAssAccountInfoByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccountInfoByCustSubscriber_DCSCenter(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Center exception: " + ex.Message);
                return Ok(new GetAssAccountInfoByCustSubscriberModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountsByCustSubscriberDCSCenter)]
        public IActionResult GetAssAccountsByCustSubscriberDCSCenter(GetAssAccountsByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccountsByCustSubscriber_DCSCenter(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Center exception: " + ex.Message);
                return Ok(new GetAssAccountsByCustSubscriberResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDevicesByCustSubscriberDCSCenter)]
        public IActionResult GetAssDevicesByCustSubscriberDCSCenter(GetAssDevicesByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDevicesByCustSubscriber_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Center exception: " + ex.Message);
                return Ok(new GetAssDevicesByCustSubscriberResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssMatrixAccountByCustSubscriberDCSCenter)]
        public IActionResult GetAssMatrixAccountByCustSubscriberDCSCenter(GetAssMatrixAccountByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssMatrixAccountByCustSubscriber_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Center exception: " + ex.Message);
                return Ok(new GetAssMatrixAccountByCustSubscriberResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetTransactionHistoryDCSCenter)]
        public IActionResult GetTransactionHistoryDCSCenter(TransactionParamModel param)
        {
            try
            {
                var data = _fpsService.GetTransactionHistory_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Transaction History DCS Center exception: " + ex.Message);

                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("timeout expired"))
                {
                    return Ok(new TransactionResultModel() { IsTimeout = true });
                }

                return Ok(new TransactionResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetLastestTransactionBySubscriberDCSCenter)]
        public IActionResult GetLastestTransactionBySubscriberDCSCenter(LatestTransactionParamModel param)
        {
            try
            {
                var data = _fpsService.GetLastestTransactionBySubscriber_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Lastest Transaction By Subscriber DCS Center exception: " + ex.Message);
                return Ok(new LatestTransactionResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetDevicesPerAccountDCSCenter)]
        public IActionResult GetDevicesPerAccountDCSCenter(GetDevicesPerAccountParamModel param)
        {
            try
            {
                var data = _fpsService.GetDevicesPerAccount_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Device By Account DCS Center exception: " + ex.Message);
                return Ok(new GetDevicesPerAccountResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAccountsPerDeviceDCSCenter)]
        public IActionResult GetAccountsPerDeviceDCSCenter(GetAccountPerDeviceParamModel param)
        {
            try
            {
                var data = _fpsService.GetAccountsPerDevice_DCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Device By Account DCS Center exception: " + ex.Message);
                return Ok(new GetAccountsPerDeviceResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.SearchDeviceDCSCenter)]
        public IActionResult SearchDeviceDCSCenter(SearchDeviceParamModel param)
        {
            try
            {
                var data = _fpsService.SearchDevice_DCSCenter(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Search Device DCS Center exception: " + ex.Message);
                return Ok(new List<SearchDeviceItemModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDeviceInfoByIdSubscriberDCSCenter)]
        public IActionResult GetAssDeviceInfoByIdSubscriberDCSCenter(GetAssDeviceInfoByIDSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDeviceInfoByIDSubscriber_DCSCenter(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by id subscriber DCS Center exception: " + ex.Message);
                return Ok(new GetAssDeviceInfoByIDSubscriberModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountsByDeviceSubscriberDCSCenter)]
        public IActionResult GetAssAccountsByDeviceSubscriberDCSCenter(GetAssAccByDevSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccByDevSubscriber_DCSCenter(param);

                return Ok(data.ToList());
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account by Device subscriber DCS Center exception: " + ex.Message);
                return Ok(new List<GetAssAccByDevSubscriberItemDCSCenterModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDevByDeviceSubscriberDCSCenter)]
        public IActionResult GetAssDevByDeviceSubscriberDCSCenter(GetAssDevByDevSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDevByDevSubscriber_DCSCenter(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Device by Device subscriber DCS Center exception: " + ex.Message);
                return Ok(new List<GetAssDevByDevSubscriberItemModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssMatrixByDeviceSubscriberDCSCenter)]
        public IActionResult GetAssMatrixByDeviceSubscriberDCSCenter(GetAssMatrixByDeviceSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssMatrixByDeviceSubscriber_DCSCenter(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Matrix by Device subscriber DCS Center exception: " + ex.Message);
                return Ok(new List<GetAssMatrixByDeviceSubscriberItemDataCenterModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetTransactionHistoryByDeviceDCSCenter)]
        public IActionResult GetTransactionHistoryByDeviceDCSCenter(TransactionHistoryByDeviceParamModel param)
        {
            try
            {
                var data = _fpsService.GetTransactionHistoryByDeviceDCSCenter(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Transaction History By Device DCS Center exception: " + ex.Message);

                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("timeout expired"))
                {
                    return Ok(new TransactionHistoryByDeviceDCSCenterResultModel() { IsTimeout = true });
                }

                return Ok(new TransactionHistoryByDeviceDCSCenterResultModel());
            }
        }

        #endregion dcs center

        #region dcs extra

        #region get sub
        [HttpGet]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetSubscriberInfoDCSExtra)]
        public IActionResult GetSubscriberInfoDCSExtra(string subscriberName)
        {
            try
            {
                var subscriber = _subscriberService.GetSubscriberBySubscriberName_DCSExtra(subscriberName);

                if (subscriber != null && subscriber.SubscriberStatus)
                {
                    return Ok(subscriber);
                }
                else
                {
                    LogExtension.LogError($"{subscriberName} is not integrated on DCS Extra.");
                }
            }
            catch (Exception ex)
            {
                LogExtension.LogError("Get Subscriber info DCS Extra exception: " + ex.Message);
            }

            return Ok(new SubscriberFpsInfo());
        }
        #endregion

        #region search
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.SearchAccountDCSExtra)]
        public IActionResult SearchAccountDCSExtra(SearchAccountParamModel param)
        {
            try
            {
                var data = _fpsService.SearchAccount_DCSExtra(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Search Account info DCS Extra exception: " + ex.Message);
                return Ok(new SearchAccountResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.SearchDeviceDCSExtra)]
        public IActionResult SearchDeviceDCSExtra(SearchDeviceParamModel param)
        {
            try
            {
                var data = _fpsService.SearchDevice_DCSExtra(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Search Device info DCS Extra exception: " + ex.Message);
                return Ok(new List<SearchDeviceItemDCSExtraModel>());
            }
        }
        #endregion

        #region account
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountInfoByCustSubscriberDCSExtra)]
        public IActionResult GetAssAccountInfoByCustSubscriberDCSExtra(GetAssAccountInfoByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccountInfoByCustSubscriber_DCSExtra(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Extra exception: " + ex.Message);
                return Ok(new GetAssAccountInfoByCustSubscriberModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountsByCustSubscriberDCSExtra)]
        public IActionResult GetAssAccountsByCustSubscriberDCSExtra(GetAssAccountsByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccountsByCustSubscriber_DCSExtra(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Extra exception: " + ex.Message);
                return Ok(new GetAssAccountsByCustSubscriberResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDevicesByCustSubscriberDCSExtra)]
        public IActionResult GetAssDevicesByCustSubscriberDCSExtra(GetAssDevicesByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDevicesByCustSubscriber_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Extra exception: " + ex.Message);
                return Ok(new GetAssDevicesByCustSubscriberResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssMatrixAccountByCustSubscriberDCSExtra)]
        public IActionResult GetAssMatrixAccountByCustSubscriberDCSExtra(GetAssMatrixAccountByCustSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssMatrixAccountByCustSubscriber_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by cust subscriber DCS Extra exception: " + ex.Message);
                return Ok(new GetAssMatrixAccountByCustSubscriberResultModel());
            }
        }
        #endregion

        #region device
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDeviceInfoByIdSubscriberDCSExtra)]
        public IActionResult GetAssDeviceInfoByIdSubscriberDCSExtra(GetAssDeviceInfoByIDSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDeviceInfoByIDSubscriber_DCSExtra(param);

                return Ok(data);

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account info by id subscriber DCS Extra exception: " + ex.Message);
                return Ok(new GetAssDeviceInfoByIDSubscriberModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssAccountsByDeviceSubscriberDCSExtra)]
        public IActionResult GetAssAccountsByDeviceSubscriberDCSExtra(GetAssAccByDevSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssAccByDevSubscriber_DCSExtra(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Account by Device subscriber DCS Extra exception: " + ex.Message);
                return Ok(new List<GetAssAccByDevSubscriberItemModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssDevByDeviceSubscriberDCSExtra)]
        public IActionResult GetAssDevByDeviceSubscriberDCSExtra(GetAssDevByDevSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssDevByDevSubscriber_DCSExtra(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Device by Device subscriber DCS Extra exception: " + ex.Message);
                return Ok(new List<GetAssDevByDevSubscriberItemModel>());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetAssMatrixByDeviceSubscriberDCSExtra)]
        public IActionResult GetAssMatrixByDeviceSubscriberDCSExtra(GetAssMatrixByDeviceSubscriberParamModel param)
        {
            try
            {
                var data = _fpsService.GetAssMatrixByDeviceSubscriber_DCSExtra(param);

                return Ok(data.ToList());

            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Ass Matrix by Device subscriber DCS Extra exception: " + ex.Message);
                return Ok(new List<GetAssMatrixByDeviceSubscriberItemModel>());
            }
        }
        #endregion

        #region report
        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetTransactionHistoryByDeviceDCSExtra)]
        public IActionResult GetTransactionHistoryByDeviceDCSExtra(TransactionHistoryByDeviceParamModel param)
        {
            try
            {
                var data = _fpsService.GetTransactionHistoryByDevice_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Transaction History By Device DCS Extra exception: " + ex.Message);

                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("timeout expired"))
                {
                    return Ok(new TransactionHistoryByDeviceDCSExtraResultModel() { IsTimeout = true });
                }

                return Ok(new TransactionHistoryByDeviceDCSExtraResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetTransactionHistoryByAccountDCSExtra)]
        public IActionResult GetTransactionHistoryByAccountDCSExtra(TransactionHistoryByAccountParamModel param)
        {
            try
            {
                var data = _fpsService.GetTransactionHistoryByAccount_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Transaction History By Account DCS Extra exception: " + ex.Message);

                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("timeout expired"))
                {
                    return Ok(new TransactionHistoryByAccountDCSExtraResultModel() { IsTimeout = true });
                }

                return Ok(new TransactionHistoryByAccountDCSExtraResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetTransactionHistoryDCSExtra)]
        public IActionResult GetTransactionHistoryDCSExtra(TransactionParamModel param)
        {
            try
            {
                var data = _fpsService.GetTransactionHistory_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Transaction History DCS Extra exception: " + ex.Message);

                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("timeout expired"))
                {
                    return Ok(new TransactionResultModel() { IsTimeout = true });
                }

                return Ok(new TransactionResultModel());
            }
        }

        [HttpPost]
        [TrackingFunctionUsageFilter(SystemId.CTSAPI, TrackingCtsFunctionId.GetLastestTransactionBySubscriberDCSExtra)]
        public IActionResult GetLastestTransactionBySubscriberDCSExtra(LatestTransactionParamModel param)
        {
            try
            {
                var data = _fpsService.GetLastestTransactionBySubscriber_DCSExtra(param);

                return Ok(data);
            }
            catch (Exception ex)
            {
                LogExtension.LogError("FPS Get Lastest Transaction By Subscriber DCS Extra exception: " + ex.Message);
                return Ok(new LatestTransactionResultModel());
            }
        }
        #endregion

        #endregion dcs extra
    }
}
