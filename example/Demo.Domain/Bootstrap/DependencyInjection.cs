using AutoRegister.DI;
using Demo.SharedKernel.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddDemoDomain(this IServiceCollection services)
    {
        services
            .AddDemoKernel()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}