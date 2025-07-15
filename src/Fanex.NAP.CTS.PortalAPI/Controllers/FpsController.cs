namespace Fanex.NAP.CTS.Portal.API.Controllers
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Model.API.FPS.Account;
    using Fanex.NAP.CTS.Model.API.FPS.Device;
    using Fanex.NAP.CTS.Model.API.FPS.Search;
    using Fanex.NAP.CTS.Model.API.FPS.SearchAccount;
    using Fanex.NAP.CTS.Model.API.FPS.Subscriber;
    using Fanex.NAP.CTS.Model.API.FPS.Transaction;
    using Fanex.NAP.CTS.Service.Interface;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
