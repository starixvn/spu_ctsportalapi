namespace Fanex.NAP.CTS.User.Helper
{
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Microsoft.Extensions.Caching.Memory;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.DependencyInjection;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using System.ComponentModel;

    public static class UMHelper
    {
        public static void Setup(IServiceCollection services)
        {
            var umConfig = new UMClientConfiguration(GetValue<string>("ONEbook.UM:ServiceUri"), GetValue("ONEbook.UM:EnableCheckUM", defaultValue: true), GetValue("ONEbook.UM:SecretKey", "9yBNYA5Fnv2V58+pKLvYpFwV5BMkIh9NNTxPQk6Sbss="), GetValue("ONEbook.UM:EnableUnscheduledUM", defaultValue: false), GetValue<string>("ONEbook.UM:UMFrom"), GetValue<string>("ONEbook.UM:UMTo"), GetValue("ONEbook.UM:UMPageUrl", "~/UserLogin/Common/Offline"), GetValue("ONEbook.UM:SiteId", 4120), GetValue("ONEbook.UM:ResultCacheDuration", 60), GetValue("ONEbook.UM:UMLoggingCategory", "ONEbook-UM"), GetValue<string>("ONEbook.UM:ErrorUMPageUrl"));
            services.AddSingleton(umConfig);
            services.AddSingleton<IMemoryCache, MemoryCache>(sp =>
            {
                var options = new MemoryCacheOptions();
                return new MemoryCache(options);
            });

            services.AddScoped<IUMRestClient>(sp =>
            {
                var configuration = sp.GetRequiredService<UMClientConfiguration>();
                return new UMRestClient(configuration, null);
            });
            services.AddScoped<IUMCacheService, MemoryUmCacheService>(sp =>
            {
                var configuration = sp.GetRequiredService<IMemoryCache>();
                return new MemoryUmCacheService(configuration);
            });
            services.AddScoped<IHttpContextHelper, HttpContextHelper>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
        }

        public static bool IsUnderMaintainaceHappen(IMaintenanceService maintenanceService)
        {
            var maintenanceInfo = maintenanceService.GetUnderMaintenanceTime();
            //GMT+8 Taiwan
            var currentTime = DateTime.UtcNow.AddHours(8);
#pragma warning disable CS0168 // The variable 'maintainceFrom' is declared but never used
            DateTime maintainceFrom;
#pragma warning restore CS0168 // The variable 'maintainceFrom' is declared but never used
#pragma warning disable CS0168 // The variable 'maintainceTo' is declared but never used
            DateTime maintainceTo;
#pragma warning restore CS0168 // The variable 'maintainceTo' is declared but never used

            if (maintenanceInfo.IsUnderMaintenanceTime
                /*&& DateTime.TryParse(maintenanceInfo.From, out maintainceFrom)
                && DateTime.TryParse(maintenanceInfo.To, out maintainceTo)
                && currentTime >= maintainceFrom
                && currentTime <= maintainceTo*/)
            {
                return true;
            }

            return false;
        }

        private static T GetValue<T>(string key, T defaultValue)
        {
            string value = Core.Utilities.ConfigurationManager.AppSettings(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return GetValue<T>(key);
        }

        private static T GetValue<T>(string key)
        {
            string text = Core.Utilities.ConfigurationManager.AppSettings(key);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(text);
        }
    }
}
