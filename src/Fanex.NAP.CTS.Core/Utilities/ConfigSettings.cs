namespace Fanex.NAP.CTS.Core.Utilities
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigSettings
    {
        private static IConfiguration Configuration;

        public static void Setup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string GetConfigStringByName(string name)
        {
            return Configuration?[name] ?? string.Empty;
        }

        public static string StoredProcedurePath => GetConfigStringByName("StoredProcedurePath");

        public static string EnvironmentName
        {
            get
            {
                var environment = GetConfigStringByName("Environment");

                switch (environment)
                {
                    case "1":
                        return "LOCAL";
                    case "2":
                        return "UAT";
                    case "3":
                        return "PRO";
                    case "4":
                        return "SIT";
                    case "5":
                        return "AWS";
                    case "6":
                        return "KL Simple";
                    case "7":
                        return "KL Prod";
                    default:
                        return "LOCAL";
                }
            }
        }

        public static int PushCCToExternalChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("PushCCToExternal-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static int InternalChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("Internal-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static bool IsSyncAFCData
        {
            get
            {
                var rs = Configuration["IsSyncAntiFraudCenterData"];

                return rs == "1";
            }
        }

        public static string ApiKey
        {
            get
            {
                var apiKey = Configuration["ApiKey"] ?? string.Empty;
                return apiKey;
            }
        }
    }
}