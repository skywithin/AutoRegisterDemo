using AutoRegister.DI;
using Demo.Domain.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Application.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddDomain()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}