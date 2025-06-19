namespace Fanex.NAP.CTS.PortalAPI
{
    using Fanex.Data;
    using Fanex.Logging.Sentry.RavenClient;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;
    using Fanex.Data.SqlServer;
    using Fanex.NAP.CTS.Core.Utilities;
    using Fanex.NAP.CTS.Service;
    using Fanex.NAP.CTS.User.Helper;
    using Fanex.Data.MySql;
    using Fanex.NAP.CTS.Portal.API.ActionFilter;
    using Fanex.RemotingRef;
    using HealthChecks.UI.Client;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fanex.NAP.CTS.PortalAPI", Version = "v1" });
            });

            ConfigSettings.Setup(Configuration);
            ConfigurationManager.Setup(Configuration);
            services.AddFanexLogSentry(Configuration);
            UMHelper.Setup(services);

            RefDataTypeContainer
                .ConnectTo(new Uri(ConfigurationManager.AppSettings("Fanex.RemotingRef.Service.Url")))
                .FromApplication("CTS Portal API");

            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(Configuration)
              .WithMySql(Configuration)
              .UseDefaultDbSettingProvider(ConfigSettings.StoredProcedurePath)
              .Run();

            // SetupDI
            ServiceDIConfig.Setup(services);

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Setup ActionFilter
            TrackingFunctionUsageFilterAttribute.Setup(app.ApplicationServices);
            UnderMaintainaceHappenFilterAttribute.Setup(app.ApplicationServices);

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fanex.NAP.CTS.PortalAPI v1"));
            app.UseRouting();

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
