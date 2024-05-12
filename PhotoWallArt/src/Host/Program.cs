using Hangfire;
using PhotoWallArt.Application;
using PhotoWallArt.Application.Common.Interfaces;
using PhotoWallArt.Host.BackGroundServices;
using PhotoWallArt.Host.Configurations;
using PhotoWallArt.Host.Controllers;
using PhotoWallArt.Infrastructure;
using PhotoWallArt.Infrastructure.Common;
using PhotoWallArt.Infrastructure.Common.Services;
using PhotoWallArt.Infrastructure.Logging.Serilog;
using Serilog;
using Serilog.Formatting.Compact;

[assembly: ApiConventionType(typeof(FSHApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddHttpContextAccessor();

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();
    app.UseInfrastructure(builder.Configuration);

    app.MapEndpoints();
    app.Run();
    RecurringJob.AddOrUpdate<LogCleanupService>("DeleteOldLogs", service => service.DeleteOldLogs(), Cron.Daily);
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}