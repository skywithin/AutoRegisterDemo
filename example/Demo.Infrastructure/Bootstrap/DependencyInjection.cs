using AutoRegister.DI;
using Demo.Application.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Infrastructure.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddDemoInfrastructure(this IServiceCollection services)
    {
        services
            .AddDemoApplication()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}