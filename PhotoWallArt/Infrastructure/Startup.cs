using Infrastructure.BackgroundJobs;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddApiVersioning()
                //.AddAuth(config)
                .AddBackgroundJobs(config)
                // .AddCaching(config)
                //.AddCorsPolicy(config)
                //.AddExceptionMiddleware()
                //.AddBehaviours(applicationAssembly)
                //.AddHealthCheck()
                //.AddPOLocalization(config)
                //.AddMailing(config)
                .AddMediatR(Assembly.GetExecutingAssembly());
                //.AddNotifications(config)
                //.AddOpenApiDocumentation(config)
                //.AddRequestLogging(config)
                //.AddRouting(options => options.LowercaseUrls = true)
                //.AddServices();
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder
                .UseRequestLocalization()
                .UseStaticFiles()
                .UseHangfireDashboard(config);

        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapControllers().RequireAuthorization();
            builder.MapHealthCheck();
            return builder;
        }

        private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
            endpoints.MapHealthChecks("/api/health");
    }
}
