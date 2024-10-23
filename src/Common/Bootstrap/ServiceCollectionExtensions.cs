using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Bootstrap;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Automatically register services flagged with AutoRegisterAttribute
    /// </summary>
    public static IServiceCollection AddAutoWiredServices(this IServiceCollection services)
    {
        var autoRegistrations =
            AppDomain.CurrentDomain
                .GetAssemblies() // TODO: This is potentially problematic. In order to scan multiple assemblies, they (assemblies) must be referenced before AddAutoWiredServices method is called.
                .SelectMany(s => s.GetLoadableTypes())
                .Where(type =>
                    type.IsDefined(typeof(AutoWireAttribute), inherit: false) &&
                    !type.IsAbstract &&
                    !type.IsInterface)
                .Select(type =>
                {
                    var attr = type.GetCustomAttribute<AutoWireAttribute>();

                    return new
                    {
                        attr.Lifetime,
                        attr.RegisterAs,
                        ServiceInterface = type.GetInterfaces().FirstOrDefault(),
                        Implementation = type
                    };
                })
                .ToArray();

        foreach (var reg in autoRegistrations)
        {
            if (reg.RegisterAs.HasFlag(RegisterAs.Self))
            {
                switch (reg.Lifetime)
                {
                    case Lifetime.Scoped:
                        services.AddScoped(serviceType: reg.Implementation);
                        break;
                    case Lifetime.Transient:
                        services.AddTransient(serviceType: reg.Implementation);
                        break;
                    case Lifetime.Singleton:
                        services.AddSingleton(serviceType: reg.Implementation);
                        break;
                }
            }

            if (reg.RegisterAs.HasFlag(RegisterAs.Interface) && reg.ServiceInterface != null)
            {
                switch (reg.Lifetime)
                {
                    case Lifetime.Scoped:
                        services.AddScoped(serviceType: reg.ServiceInterface, implementationType: reg.Implementation);
                        break;
                    case Lifetime.Transient:
                        services.AddTransient(serviceType: reg.ServiceInterface, implementationType: reg.Implementation);
                        break;
                    case Lifetime.Singleton:
                        services.AddSingleton(serviceType: reg.ServiceInterface, implementationType: reg.Implementation);
                        break;
                }
            }
        }

        return services;
    }

    private static Type[] GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null).ToArray();
        }
    }
}
