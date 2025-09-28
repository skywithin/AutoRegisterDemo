using AutoRegister.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.SharedKernel.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddDemoKernel(this IServiceCollection services)
    {
        services.AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}