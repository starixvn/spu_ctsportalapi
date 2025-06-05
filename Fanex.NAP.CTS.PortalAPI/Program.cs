using Fanex.Caching;
using Fanex.NAP.CTS.Core.Utilities;
using Fanex.NAP.CTS.Portal.API.ActionFilter;
using Fanex.NAP.CTS.Service;
using ONEbook.UM.Configurations;
using ONEbook.UM.Transport;
using ONEbook.UM;
using ONEbook.UM.NETFramework;
using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.SqlServer;
using System.Configuration;

//var umConfig = new UMClientConfiguration().Initialize();

//UMClientManager
//   .UseConfig(umConfig)
//   .UseCacheService(new UMCacheService(new CacheService()))
//   .UseRestClient(new UMRestClient(umConfig))
//   .UseHttpContext(new HttpContextHelper());

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

DbSettingProviderManager
  .StartNewSession()
  .WithSqlServer(configuration)
  .WithMySql(configuration)
  .UseDefaultDbSettingProvider("~/App_Data/SQL/SPNames.xml")
  .Run();

// SetupDI
ServiceDIConfig.Setup(services);

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Setup ActionFilter
ConfigSettings.Setup(app.Configuration);
Fanex.NAP.CTS.Core.Utilities.ConfigurationManager.Setup(app.Configuration);
TrackingFunctionUsageFilterAttribute.Setup(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
