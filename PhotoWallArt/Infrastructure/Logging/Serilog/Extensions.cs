using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System.Configuration;
using System.Reflection;

namespace Infrastructure.Logging.Serilog;
public static class Extensions
{
    public static void RegisterSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddOptions<LoggerSettings>().BindConfiguration(nameof(LoggerSettings));
        builder.Services.AddOptions<SinkOptionsSection>().BindConfiguration(nameof(SinkOptionsSection));

        _ = builder.Host.UseSerilog((_, sp, serilogConfig) =>
        {
            var loggerSettings = sp.GetRequiredService<IOptions<LoggerSettings>>().Value;
            var sinkOptions = sp.GetRequiredService<IOptions<SinkOptionsSection>>().Value;
            var connectionString = loggerSettings.WriteTo.ConnectionString;

            string test = loggerSettings.WriteTo.SinkOptionsSection.TableName;
            string appName = loggerSettings.AppName;
            string elasticSearchUrl = loggerSettings.ElasticSearchUrl;
            bool writeToFile = loggerSettings.WriteToFile;
            bool structuredConsoleLogging = loggerSettings.StructuredConsoleLogging;
            string minLogLevel = loggerSettings.WriteTo.SinkOptionsSection.RestrictedToMinimumLevel;
            ConfigureEnrichers(serilogConfig, appName);
            ConfigureConsoleLogging(serilogConfig, structuredConsoleLogging);
            ConfigureWriteTo(serilogConfig, loggerSettings,connectionString);
            SetMinimumLogLevel(serilogConfig, minLogLevel);
            OverideMinimumLogLevel(serilogConfig);
            Console.WriteLine(FiggleFonts.Standard.Render(loggerSettings.AppName));
        });
    }

    private static void ConfigureEnrichers(LoggerConfiguration serilogConfig, string appName)
    {
        serilogConfig
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", appName)
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithMachineName()
                        .Enrich.WithProcessId()
                        .Enrich.WithThreadId()
                        .Enrich.FromLogContext();
    }

    private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool structuredConsoleLogging)
    {
        if (structuredConsoleLogging)
        {
            serilogConfig.WriteTo.Async(wt => wt.Console(new CompactJsonFormatter()));
        }
        else
        {
            serilogConfig.WriteTo.Async(wt => wt.Console());
        }
    }

    private static void ConfigureWriteTo(LoggerConfiguration serilogConfig, LoggerSettings Options, string connectionString)
    {
        //serilogConfig.WriteTo.MSSqlServer(
        //    connectionString, Options.WriteTo.SinkOptionsSection.TableName,null,
        //    LogEventLevel.Error, 50, null, null,
        //    Options.WriteTo.SinkOptionsSection.AutoCreateSqlTable, null, null, Options.WriteTo.SinkOptionsSection.SchemaName, null);

        serilogConfig.WriteTo.MSSqlServer(
         connectionString:
                 connectionString,
             restrictedToMinimumLevel: LogEventLevel.Error,
             sinkOptions: new MSSqlServerSinkOptions
             {
                 TableName = "LogEvents",
                 AutoCreateSqlTable = true
             });
    }

    private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
    {
        serilogConfig
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                     .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
    }

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        switch (minLogLevel.ToLower())
        {
            case "debug":
                serilogConfig.MinimumLevel.Debug();
                break;
            case "information":
                serilogConfig.MinimumLevel.Information();
                break;
            case "warning":
                serilogConfig.MinimumLevel.Warning();
                break;
            default:
                serilogConfig.MinimumLevel.Information();
                break;
        }
    }
}
