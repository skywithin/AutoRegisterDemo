# AutoRegister.DI

A powerful dependency injection automation library for .NET that simplifies service registration using attributes. This library automatically discovers and registers services decorated with the `[AutoRegister]` attribute, supporting various lifetimes and registration strategies.

## Features

- **Automatic Service Discovery**: Scans assemblies for services decorated with `[AutoRegister]` attribute
- **Flexible Lifetimes**: Supports `Scoped`, `Transient`, and `Singleton` lifetimes
- **Multiple Registration Strategies**: Register as interface, self, or both
- **Assembly-Specific Scanning**: Target specific assemblies for better performance
- **Clean Architecture Support**: Perfect for layered applications with dependency hierarchies

## Quick Start

### 1. Decorate Your Services

Use the `[AutoRegister]` attribute to mark your services for automatic registration:

```csharp
using AutoRegister.DI;

// Register as interface only
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class UserService : IUserService
{
    // Implementation...
}

// Register as both interface and self
[AutoRegister(Lifetime.Singleton, RegisterAs.Interface | RegisterAs.Self)]
public class CacheService : ICacheService
{
    // Implementation...
}

// Register as self only
[AutoRegister(Lifetime.Transient, RegisterAs.Self)]
public class UtilityService
{
    // Implementation...
}
```

### 2. Configure in Your Application

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Register services from specific assemblies
        builder.Services.AddAutoRegisteredServicesFromAssembly(
            typeof(Program).Assembly,
            typeof(UserService).Assembly
        );
        
        var app = builder.Build();
        app.Run();
    }
}
```

## Advanced Usage

### Clean Architecture Example

The library excels in Clean Architecture scenarios where you have multiple layers with their own services:

```csharp
// SharedKernel Layer
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal sealed class ClockService : IClockService
{
    public DateTime UtcNow => DateTime.UtcNow;
}

// Application Layer
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal sealed class UserService : IUserService
{
    private readonly IUserDataProvider _dataProvider;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IUserDataProvider dataProvider, ILogger<UserService> logger)
    {
        _dataProvider = dataProvider;
        _logger = logger;
    }
    // Implementation...
}

// Infrastructure Layer
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal sealed class UserDataProvider : IUserDataProvider
{
    // Implementation...
}

// Web API Layer
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class HealthCheckService : IHealthCheckService
{
    // Implementation...
}
```

### Layer-by-Layer Registration

Configure each layer to register its own services:

```csharp
// Web API Bootstrap
public static IServiceCollection AddApiServices(this IServiceCollection services)
{
    services
        .AddControllers()
        .AddSwagger()
        .AddDemoInfrastructure()  // Registers all lower layers
        .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly); // Registers Web API layer services
    
    return services;
}

// Infrastructure Bootstrap
public static IServiceCollection AddDemoInfrastructure(this IServiceCollection services)
{
    services
        .AddDemoApplication()  // Registers Application layer
        .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);
    
    return services;
}

// Application Bootstrap
public static IServiceCollection AddDemoApplication(this IServiceCollection services)
{
    services
        .AddDemoDomain()  // Registers Domain layer
        .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);
    
    return services;
}

// Domain Bootstrap
public static IServiceCollection AddDemoDomain(this IServiceCollection services)
{
    services
        .AddDemoKernel()  // Registers SharedKernel layer
        .AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);
    
    return services;
}

// SharedKernel Bootstrap
public static IServiceCollection AddDemoKernel(this IServiceCollection services)
{
    services.AddAutoRegisteredServicesFromAssembly(AssemblyReference.Assembly);
    return services;
}
```

## API Reference

### AutoRegisterAttribute

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterAttribute : Attribute
{
    public AutoRegisterAttribute(Lifetime serviceLifetime, RegisterAs registerAs)
    {
        ServiceLifetime = serviceLifetime;
        RegisterAs = registerAs;
    }
    
    public Lifetime ServiceLifetime { get; }
    public RegisterAs RegisterAs { get; }
}
```

### Lifetime Enum

```csharp
public enum Lifetime
{
    Scoped,     // One instance per scope (e.g., per HTTP request)
    Transient,  // New instance every time
    Singleton   // Single instance for the application lifetime
}
```

### RegisterAs Enum

```csharp
[Flags]
public enum RegisterAs
{
    Interface = 1 << 0,  // Register as the first interface
    Self = 1 << 1        // Register as the concrete type
}
```

### Extension Methods

```csharp
// Register services from specific assemblies
public static IServiceCollection AddAutoRegisteredServicesFromAssembly(
    this IServiceCollection services, 
    params Assembly[] assemblies)
```

## Best Practices

### 1. Use Appropriate Lifetimes

- **Scoped**: For services that should be shared within a request scope (e.g., repositories, services)
- **Transient**: For stateless services that can be created frequently
- **Singleton**: For services that maintain state or are expensive to create

### 2. Interface Registration

Prefer registering as interfaces for better testability and loose coupling:

```csharp
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class UserService : IUserService
{
    // Implementation...
}
```

### 3. Assembly Organization

Organize your services by layer and register them appropriately:

```csharp
// Register each layer's services
services
    .AddSharedKernelServices()      // SharedKernel services
    .AddDomainServices()            // Domain services  
    .AddApplicationServices()        // Application services
    .AddInfrastructureServices()    // Infrastructure services
    .AddWebServices();             // Web API services
```

### 4. Performance Considerations

- Use assembly-specific registration for better performance
- Avoid scanning unnecessary assemblies
- Consider using `RegisterAs.Self` for internal services that don't need interface registration

## Console Output

The library provides helpful console output during service registration:

```
Scanning Demo.SharedKernel (1.0.0.0) assembly for auto-registrations...
Register [Demo.SharedKernel.Services.ClockService] as [Demo.SharedKernel.Services.IClockService] with [Scoped] lifetime.
Auto-registration completed. Registered 1 service(s)

Scanning Demo.Application (1.0.0.0) assembly for auto-registrations...
Register [Demo.Application.Services.UserService] as [Demo.Application.Services.IUserService] with [Scoped] lifetime.
Auto-registration completed. Registered 1 service(s)
```

## Error Handling

The library includes comprehensive error handling:

- Validates that at least one assembly is provided
- Warns when services have no interfaces but are registered as `RegisterAs.Interface`
- Provides clear error messages for configuration issues

## Example Project Structure

```
src/
├── AutoRegister.DI/           # The library itself
└── example/
    ├── Demo.SharedKernel/     # Shared domain concepts
    ├── Demo.Domain/           # Business logic and models
    ├── Demo.Application/      # Use cases and application services
    ├── Demo.Infrastructure/   # External concerns (data, APIs)
    └── Demo.Web.Api/          # Presentation layer
```

## How It Works

### 1. Service Discovery

The library scans specified assemblies for classes decorated with `[AutoRegister]` attribute:

```csharp
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class UserService : IUserService
{
    // This service will be automatically registered
}
```

### 2. Registration Process

For each discovered service, the library:

1. **Validates the service**: Ensures it's not abstract and not an interface
2. **Determines registration strategy**: Based on `RegisterAs` parameter
3. **Registers with appropriate lifetime**: Scoped, Transient, or Singleton
4. **Provides feedback**: Console output showing what was registered

### 3. Dependency Chain

In a Clean Architecture setup, dependencies flow from outer layers to inner layers:

```
Web API → Infrastructure → Application → Domain → SharedKernel
```

Each layer registers its own services and calls the next layer's registration method.

### 4. Service Resolution

When a service is requested, the DI container:

1. **Resolves dependencies**: Automatically injects required dependencies
2. **Maintains lifetime**: Creates new instances or reuses existing ones based on lifetime
3. **Handles disposal**: Properly disposes of services when scope ends

## Contributing

This library is designed to be simple, performant, and easy to use. Contributions are welcome!
