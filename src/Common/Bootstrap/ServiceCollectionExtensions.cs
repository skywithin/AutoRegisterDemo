﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Bootstrap;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Automatically register services flagged with AutoRegisterAttribute
    /// </summary>
    public static IServiceCollection AddAutoRegisteredServices(this IServiceCollection services)
    {
        var autoRegistrations =
            AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetLoadableTypes())
                .Where(type =>
                    type.IsDefined(typeof(AutoRegisterAttribute), inherit: false) &&
                    !type.IsAbstract &&
                    !type.IsInterface)
                .Select(type =>
                {
                    var attr = type.GetCustomAttribute<AutoRegisterAttribute>();

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
