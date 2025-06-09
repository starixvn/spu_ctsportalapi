using Fanex.NAP.CTS.Core.Utilities;
using Fanex.NAP.CTS.Portal.API.ActionFilter;
using Fanex.NAP.CTS.Service;
using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.SqlServer;
using Fanex.NAP.CTS.User.Helper;
using Fanex.RemotingRef;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.NAP.CTS.Core.Extentions;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var services = builder.Services;
    var configuration = builder.Configuration;

    services.AddHttpContextAccessor();
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fanex.NAP.CTS.PortalAPI", Version = "v1" });
    });

    ConfigSettings.Setup(configuration);
    Fanex.NAP.CTS.Core.Utilities.ConfigurationManager.Setup(configuration);
    services.AddFanexLogSentry(configuration);

    UMHelper.Setup(services);

    DbSettingProviderManager
      .StartNewSession()
      .WithSqlServer(configuration)
      .WithMySql(configuration)
      .UseDefaultDbSettingProvider(ConfigSettings.StoredProcedurePath)
      .Run();

    // SetupDI
    ServiceDIConfig.Setup(services);

    var app = builder.Build();

    // Setup ActionFilter
    TrackingFunctionUsageFilterAttribute.Setup(app.Services);
    UnderMaintainaceHappenFilterAttribute.Setup(app.Services);

    RefDataTypeContainer
        .ConnectTo(new Uri(Fanex.NAP.CTS.Core.Utilities.ConfigurationManager.AppSettings("Fanex.RemotingRef.Service.Url")))
        .FromApplication("CTS Portal API");

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    ex.LogError();
}
