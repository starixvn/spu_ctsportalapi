namespace Fanex.NAP.SPU.CTS.JobService
{
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.PortalAPI;

    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                (string.Format("{0} has been started at {1}", typeof(Program).FullName, DateTime.Now)).LogInfo();

                host.Run();
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5000");

                    webBuilder
                    .UseStartup<Startup>();
                });
        }

    }
}
