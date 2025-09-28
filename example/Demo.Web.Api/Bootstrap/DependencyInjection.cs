using AutoRegister.DI;
using Demo.Infrastructure.Bootstrap;

namespace Demo.Web.Api.Bootstrap;

internal static class DependencyInjection
{
    /// <summary>
    /// Adds all API services to the service collection
    /// </summary>
    public static IServiceCollection AddDemoApiServices(this IServiceCollection services)
    {
        services
            .AddHttpClient()
            .AddEndpointsApiExplorer()
            .AddMemoryCache()
            .AddApiHealthChecks();
            //.AddDbContext<MyDbContext>();

        // Add controllers with exception filter
        services
            .AddControllers(options =>
            {
                //options.Filters.Add<ApiExceptionFilter>();
            });

        // Add CORS
        services.AddApiCors();

        // Add Swagger
        services.AddApiSwagger();

        // Add Demo application services
        services.AddDemoInfrastructure();

        // Add local services
        services.AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}