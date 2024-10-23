using Common.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLogic.Bootstrap;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationLogic(this IServiceCollection services)
    {
        // Add custom registrations 

        return services;
    }
}
