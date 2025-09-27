using AutoRegister.DI;
using Demo.Application.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Infrastructure.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddApplication()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}