using AutoRegister.DI;
using Demo.Infrastructure.Bootstrap;

namespace Demo.Web.Api.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddInfrastructure()
            .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}