namespace Fanex.NAP.CTS.Service.Implement
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Helpers;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Core.Utilities;
    using Fanex.NAP.CTS.Criteria.AssociationDetection;
    using Fanex.NAP.CTS.Model;
    using Fanex.NAP.CTS.Model.AssociationDetection;
    using Fanex.NAP.CTS.ViewModel.AssociationDetection;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAssociationDetectionService
    {
        IEnumerable<ValidateUserNameModel> ValidateUserName(string userNameString);

        IEnumerable<DetectAssociationMergeGroupModel> DetectAssociation(DetectAssociationByUserNamesViewModel model, IEnumerable<ValidateUserNameModel> orgUserList, bool IsGetLogTransInfo);

        IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter);

        IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByAgent(string ctsCustIDs);

        IEnumerable<DetectAssociationMergeGroupModel> DetectGroupDetectAssociation(MergeGroupInfoModel model);

        IEnumerable<GetCustStatisticsInfoRealtimeModel> GetCustStatisticsInfoRealtime(string custIDList);

    }

    public class AssociationDetectionService : IAssociationDetectionService
    {
        private readonly ICriteriaService _repository;
        private readonly ICommonService _commonService;

        public AssociationDetectionService(ICriteriaService repository, ICommonService commonService)
        {
            _repository = repository;
            _commonService = commonService;
        }

        public IEnumerable<ValidateUserNameModel> ValidateUserName(string userNameString)
        {
            var userNames = userNameString.Split(',');

            var userNameList = string.Join(",", userNames.Select(x => x.ToUpper(CultureInfo.InvariantCulture)).Distinct());

            var orgUserList = userNames.Select(x => new ValidateUserNameModel { UserName = x });

            var validatedList = _commonService.ValidateUsernames(userNameList);

            orgUserList = orgUserList.Join(validatedList, c1 => c1.UserName.ToLower(CultureInfo.InvariantCulture), c2 => c2.UserName.ToLower(CultureInfo.InvariantCulture),
                (c1, c2) => new ValidateUserNameModel
                {
                    UserName = c1.UserName,
                    CustID = c2.CustID,
                    CTSCustID = c2.CTSCustID,
                    IsInternal = c2.IsInternal
                });

            return orgUserList;
        }

        public IEnumerable<DetectAssociationMergeGroupModel> DetectAssociation(DetectAssociationByUserNamesViewModel model, IEnumerable<ValidateUserNameModel> orgUserList, bool IsGetLogTransInfo)
        {
            var validateUserNames = model.IsIPLast3Days || model.Is3MatchesLast7Days ?
                orgUserList.Where(x => !x.IsInternal).Distinct(x => x.CustID, y => y) : null;

            var custIDs = string.Join(",", orgUserList.Select(x => x.CustID).Distinct());

            var ctsCustIDs = string.Join(",", orgUserList.Select(x => x.CTSCustID).Distinct());

            var mergeGroupInfo = new MergeGroupInfoModel { CTSCustIDs = ctsCustIDs, IsGetLogTransInfo = IsGetLogTransInfo };

            Parallel.Invoke(() =>
            {
                var assBySharedIP = model.IsIPLast3Days ? DetectAssociationByIP(validateUserNames, custIDs) : Enumerable.Empty<SharedIPInfoModel>();
                mergeGroupInfo.AssociationBySharedIPJson = GetAssociationCustByIP(assBySharedIP);
            }, () =>
            {
                var assBySharedMatches = model.Is3MatchesLast7Days ? DetectAssociationBySharedMatches(validateUserNames, custIDs) : Enumerable.Empty<SharedIPMatchesInfoModel>();
                mergeGroupInfo.AssociationBySharedMatchesJson = GetAssociationCustByMatch(assBySharedMatches);
            }, () =>
            {

                mergeGroupInfo.AssociationByDeviceJson = model.IsDevice
                ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.Device))
                : "[]";
            }, () =>
            {

                mergeGroupInfo.AssociationByAIJson = model.IsBP
                ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.AI))
                : "[]";
            }, () =>
            {

                mergeGroupInfo.AssociationByIPJson = model.IsIP
                ? GetAssociationCustByPrimary(new DetectAssociationByPrimaryFilterModel(ctsCustIDs, AssociationDetectionType.IP))
                : "[]";
            }, () =>
            {

                mergeGroupInfo.AssociationByAgentJson = model.IsAgent ? GetAssociationCustByAgent(ctsCustIDs) : "[]";
            });
            
            return DetectGroupDetectAssociation(mergeGroupInfo);
        }

        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByPrimary(DetectAssociationByPrimaryFilterModel filter)
        {
            var criteria = new DetectAssociationByPrimaryCriteria
            {
                CTSCustIDs = filter.CTSCustIDs,
                IsAI = filter.IsAI,
                IsDevice = filter.IsDevice,
                IsIP = filter.IsIP
            };

            return _repository.Fetch<DetectAssociationByPrimaryModel>(criteria);
        }

        public IEnumerable<DetectAssociationByPrimaryModel> DetectAssociationByAgent(string ctsCustIDs)
        {
            var criteria = new DetectAssociationByAgentCriteria
            {
                CTSCustIDs = ctsCustIDs
            };

            return _repository.Fetch<DetectAssociationByPrimaryModel>(criteria);
        }

        public IEnumerable<DetectAssociationMergeGroupModel> DetectGroupDetectAssociation(MergeGroupInfoModel model)
        {
            try
            {
                var criteria = new DetectAssociationMergeGroupCriteria
                {
                    CTSCustIDs = model.CTSCustIDs,
                    DeviceJson = model.AssociationByDeviceJson,
                    AIJson = model.AssociationByAIJson,
                    IPJson = model.AssociationByIPJson,
                    AgentJson = model.AssociationByAgentJson,
                    SharedIPJson = model.AssociationBySharedIPJson,
                    SharedMatchesJson = model.AssociationBySharedMatchesJson,
                    IsGetLogTransInfo = model.IsGetLogTransInfo
                };
               
                return _repository.Fetch<DetectAssociationMergeGroupModel>(criteria);
            }
            catch (Exception ex)
            {               
                var logMessage = new LogMessage(ConfigSettings.EnvironmentName + " " + ex.Message)                                
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                .AddMessage($"{DateTimeOffset.Now}").ToString();

                LogService.Error(logMessage);

                throw;
            }           
        }

        public IEnumerable<DetectAssociationBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList)
        {
            var criteria = new DetectAssociationBySharedMatchesCriteria
            {
                CustIDList = custIDList,
                QueryType = (int)DetectAssociation_QueryType.DetectAss
            };

            return _repository.Fetch<DetectAssociationBySharedIPMatchesModel>(criteria);
        }

        public IEnumerable<SharedIPInfoModel> DetectAssociationByIP(Dictionary<int, ValidateUserNameModel> validateUsernames, string custIds)
        {            
            var detectAssByIP = DetectAssociationByIP(custIds);
            
            var mergeIP_DetectAssByIP = detectAssByIP.GroupBy(x => new { x.CustID1, x.CustID2 })
                .Select(g => new DetectAssociationBySharedIPModel
                {
                    CustID1 = g.Key.CustID1,
                    CustID2 = g.Key.CustID2,
                    IP = g.Select(x => x.IP.Trim()).Join()
                });
            
            return DetectAssByIPMappingToSharedIPInfo(mergeIP_DetectAssByIP, validateUsernames);
        }

        public IEnumerable<GetCustStatisticsInfoRealtimeModel> GetCustStatisticsInfoRealtime(string custIDList)
        {
            var criteria = new GetMainCustStatisticsInfoCriteria
            {
                CustIDList = custIDList,
                IsResetBalance = true
            };

            return _repository.Fetch<GetCustStatisticsInfoRealtimeModel>(criteria);
        }


        #region Private Methods
        private IEnumerable<DetectAssociationBySharedIPModel> DetectAssociationByIP(string custIDList)
        {
            var criteria = new DetectAssociationByIPCriteria
            {
                ListCustId = custIDList
            };

            return _repository.Fetch<DetectAssociationBySharedIPModel>(criteria);
        }

        private IEnumerable<SharedIPInfoModel> DetectAssByIPMappingToSharedIPInfo(IEnumerable<DetectAssociationBySharedIPModel> detectAssByIP, Dictionary<int, ValidateUserNameModel> validateUsernames)
        {
            return detectAssByIP.Select(x =>
            {
                var fromCTSCust = validateUsernames.GetValueOrDefault(x.CustID1) ?? new ValidateUserNameModel();
                var toCTSCust = validateUsernames.GetValueOrDefault(x.CustID2) ?? new ValidateUserNameModel();

                return new SharedIPInfoModel
                {
                    FromCustID = x.CustID1,
                    FromCTSCustID = fromCTSCust.CTSCustID,
                    FromUserName = fromCTSCust.UserName,
                    ToCustID = x.CustID2,
                    ToCTSCustID = toCTSCust.CTSCustID,
                    ToUserName = toCTSCust.UserName,
                    IP = x.IP
                };
            }).Where(x => x.FromCTSCustID != 0 && x.ToCTSCustID != 0);
        }

        private string GetAssociationCustByIP(IEnumerable<SharedIPInfoModel> ass)
        {
            var assCustBySharedIP = ass.Select(x => new AssociationCustByIPViewModel
            {               
                O = x.FromCTSCustID,               
                D = x.ToCTSCustID              
            });

            return ObjectHelper.SerializeObj(assCustBySharedIP);
        }

        private IEnumerable<SharedIPMatchesInfoModel> DetectAssociationBySharedMatches(Dictionary<int, ValidateUserNameModel> validateUserNames, string custIds)
        {
            var detectAssByAssMatches = DetectAssociationBySharedMatches(custIds);

            return detectAssByAssMatches.Select(x =>
            {
                var fromCTSCust = validateUserNames.GetValueOrDefault(x.CustID1) ?? new ValidateUserNameModel();
                var toCTSCust = validateUserNames.GetValueOrDefault(x.CustID2) ?? new ValidateUserNameModel();

                return new SharedIPMatchesInfoModel
                {
                    FromCustID = x.CustID1,
                    FromCTSCustID = fromCTSCust.CTSCustID,
                    FromUserName = fromCTSCust.UserName,
                    ToCustID = x.CustID2,
                    ToCTSCustID = toCTSCust.CTSCustID,
                    ToUserName = toCTSCust.UserName
                };
            }).Where(x => x.FromCTSCustID != 0 && x.ToCTSCustID != 0);
        }

        private string GetAssociationCustByMatch(IEnumerable<SharedIPMatchesInfoModel> ass)
        {
            var assCustBySharedIP = ass.Select(x => new AssociationCustModel
            {              
                FromCTSCustID = x.FromCTSCustID,            
                ToCTSCustID = x.ToCTSCustID             
            });

            return JsonConvert.SerializeObject(assCustBySharedIP);
        }

        private string GetAssociationCustByPrimary(DetectAssociationByPrimaryFilterModel custAssType)
        {
            var assByPrimary = DetectAssociationByPrimary(custAssType);

            var input = assByPrimary.Select(c => new
            {
                O = c.OrigID,
                D = c.DestID,
                A = c.AssociationType
            });

            return ObjectHelper.SerializeObj(input);
        }

        private string GetAssociationCustByAgent(string ctsCustIDs)
        {
            var assByAgent = DetectAssociationByAgent(ctsCustIDs);

            var input = assByAgent.Select(c => new
            {
                O = c.OrigID,
                D = c.DestID,
                A = c.AssociationType
            });

            return ObjectHelper.SerializeObj(input);
        }
        #endregion
    }
}
