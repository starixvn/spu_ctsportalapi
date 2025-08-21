namespace Fanex.NAP.CTS.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using Fanex.NAP.CTS.Core.Service;
    using Fanex.NAP.CTS.Service.Implement.Api;
    using Fanex.NAP.CTS.Service.Interface.Api;
    using Fanex.NAP.CTS.Service.Interface;
    using Fanex.NAP.CTS.Service.Implement;
    using Fanex.NAP.CTS.Service.Implement.CustomerClassification;
    using Fanex.NAP.CTS.Service.Interface.CustomerClassification;

    public static class ServiceDIConfig
    {
        public static void Setup(this IServiceCollection services)
        {
            services.RegisterServiceCore();

            services.RegisterServiceApi();

            services.RegisterServiceCustomerClassification();

            services.RegisterServiceOther();

            services.RegisterServiceMatchMonitor();
        }

        public static void RegisterServiceCore(this IServiceCollection services)
        {
            services.AddScoped<ICriteriaService, CriteriaService>();
        }

        public static void RegisterServiceApi(this IServiceCollection services)
        {
            services.AddScoped<ICtsAssociationApiService, CtsAssociationApiService>();
            services.AddScoped<IFpsService, FpsService>();
            services.AddScoped<ILicenseeVIPBAService, LicenseeVIPBAService>();
            services.AddScoped<IProblemCaseMonitorService, ProblemCaseMonitorService>();
            services.AddScoped<ITWInitialSmartService, TWInitialSmartService>();
            services.AddScoped<ITWSpecialLicSubService, TWSpecialLicSubService>();
        }

        public static void RegisterServiceCustomerClassification(this IServiceCollection services)
        {
            services.AddScoped<ICustomerClassificationService, CustomerClassificationService>();
            services.AddScoped<IDataLogTrackingService, DataLogTrackingService>();
            services.AddScoped<IProblemAccountClassificationService, ProblemAccountClassificationService>();
            services.AddScoped<IRescanCustomerClassificationService, RescanCustomerClassificationService>();
        }

        public static void RegisterServiceOther(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAntiFraudCenterService, AntiFraudCenterService>();
            services.AddScoped<IAssociatedAccountService, AssociatedAccountService>();
            services.AddScoped<IAssociatedAccountService, AssociatedAccountService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICustomerCategoryService, CustomerCategoryService>();
            services.AddScoped<ILogInputDataService, LogInputDataService>();
            services.AddScoped<IMonitorCtsFunctionService, MonitorCtsFunctionService>();
            services.AddScoped<ISubscriberService, SubscriberService>();
        }

        public static void RegisterServiceMatchMonitor(this IServiceCollection services)
        {
            services.AddScoped<IMatchMonitorService, MatchMonitorService>();
        }
    }
}
