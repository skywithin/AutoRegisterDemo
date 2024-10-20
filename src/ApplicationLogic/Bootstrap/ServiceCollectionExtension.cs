using Common.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLogic.Bootstrap;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationLogic(this IServiceCollection services)
    {
        services.AddAutoRegisteredServices(); // Automatically register all services flagged with AutoRegister attribute

        return services;
    }
}
