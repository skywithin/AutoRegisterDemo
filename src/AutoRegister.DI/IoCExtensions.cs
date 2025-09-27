using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoRegister.DI;

/// <summary>
/// Register services with [AutoRegister] attribute using reflection.
/// </summary>
public static class IoCExtensions
{
    /// <summary>
    /// Register services with [AutoRegister] attribute using reflection from specified assemblies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">The assemblies to scan for auto-registration attributes.</param>
    /// <returns>The service collection for chaining.</returns>
    /// <remarks>
    /// Should be called as latest as possible in the startup process.
    /// This method provides better performance by limiting the scope of assembly scanning.
    /// </remarks>
    public static IServiceCollection AddAutoRegisteredServicesFromAssembly(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            throw new ArgumentException("At least one assembly must be specified.", nameof(assemblies));
        }

        foreach (var assembly in assemblies)
        {
            PrintWarning($"Scanning {assembly.GetName().Name} ({assembly.GetName().Version}) assembly for auto-registrations...");
        }

        var typesToRegister = GetTypesToRegister(assemblies);

        foreach (var type in typesToRegister)
        {
            if (type.RegisterAs.HasFlag(RegisterAs.Self))
            {
                PrintSuccess($"Register [{type.Implementation.FullName}] as [self] with [{type.ServiceLifetime}] lifetime.");
                AddServiceBasedOnLifetime(services, type.Implementation, type.Implementation, type.ServiceLifetime);
            }

            if (type.RegisterAs.HasFlag(RegisterAs.Interface) )
            {
                if (type.ServiceInterface is null)
                {
                    PrintError($"WARNING! [{type.Implementation.FullName}] has no interface to register as. Skipping interface registration.");
                    continue;
                }

                PrintSuccess($"Register [{type.Implementation.FullName}] as [{type.ServiceInterface.FullName}] with [{type.ServiceLifetime}] lifetime.");
                AddServiceBasedOnLifetime(services, type.ServiceInterface, type.Implementation, type.ServiceLifetime);
            }
        }

        PrintInfo($"Auto-registration completed. Registered {typesToRegister.Count()} service(s)");
        Print("");

        return services;
    }

    private static IEnumerable<dynamic> GetTypesToRegister(Assembly[] assemblies)
    {
        return assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                type.IsDefined(typeof(AutoRegisterAttribute), inherit: false) &&
                !type.IsAbstract &&
                !type.IsInterface)
            .Select(type =>
            {
                var attr = type.GetCustomAttribute<AutoRegisterAttribute>();

                return new
                {
                    attr!.ServiceLifetime,
                    attr!.RegisterAs,
                    ServiceInterface = type.GetInterfaces().FirstOrDefault(), //[MV] Register as the first interface only. This can be improved
                    Implementation = type
                };
            });
    }

    private static void AddServiceBasedOnLifetime(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        Lifetime lifetime)
    {
        switch (lifetime)
        {
            case Lifetime.Scoped:
                services.AddScoped(serviceType, implementationType);
                break;
            case Lifetime.Transient:
                services.AddTransient(serviceType, implementationType);
                break;
            case Lifetime.Singleton:
                services.AddSingleton(serviceType, implementationType);
                break;
        }
    }

    private static void PrintError(string message) => Print(message, ConsoleColor.Red);

    private static void PrintSuccess(string message) => Print(message, ConsoleColor.Green);

    private static void PrintWarning(string message) => Print(message, ConsoleColor.Yellow);

    private static void PrintInfo(string message) => Print(message, ConsoleColor.Cyan);

    private static void Print(string message, ConsoleColor color = ConsoleColor.Gray)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
}
