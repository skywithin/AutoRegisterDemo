namespace Demo.Web.Api.Bootstrap;

internal static class HealthCheckExtensions
{
    /// <summary>
    /// Configures health checks for the API
    /// </summary>
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();

        return services;
    }
}