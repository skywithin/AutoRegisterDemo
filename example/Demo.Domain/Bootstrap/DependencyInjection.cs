using AutoRegister.DI;
using Demo.SharedKernel.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddKernel()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}