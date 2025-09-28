using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AutoRegister.DI.Tests;

public interface IServiceA { }
public interface IServiceB { }
public interface IServiceC { }
public interface IServiceD { }
public interface IServiceE { }
public interface IServiceF { }
public interface IServiceG { }

/// <summary>
/// Represents a service that is not auto-registered.
/// </summary>
public class ServiceA : IServiceA { }

/// <summary>
/// Represents a service that is registered with a Transient lifetime and is available as <see cref="IServiceB"/>
/// </summary>
[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceB : IServiceB { }

/// <summary>
/// Represents a service that is registered with a Singleton lifetime and is available as <see cref="IServiceC"/>
/// </summary>
[AutoRegister(Lifetime.Singleton, RegisterAs.Interface)]
public class ServiceC : IServiceC { }

/// <summary>
/// Represents a service that is registered with a Singleton lifetime and is available as <see cref="IServiceD"/>
/// </summary>
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class ServiceD : IServiceD { }

/// <summary>
/// Represents a service that is registered as its self type.
/// </summary>
[AutoRegister(Lifetime.Transient, RegisterAs.Self)]
public class ServiceE : IServiceE { }

/// <summary>
/// Represents a service that is registered as interface.
/// </summary>
[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceF : IServiceF { }

/// <summary>
/// Represents a service that is registered as interface.
/// </summary>
[AutoRegister(Lifetime.Transient, RegisterAs.Self | RegisterAs.Interface)]
public class ServiceG : IServiceG { }

// Additional test services for comprehensive testing

/// <summary>
/// Represents a service with no interface that should be skipped when registered as Interface
/// </summary>
[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceWithoutInterface { }

/// <summary>
/// Represents a service with multiple interfaces
/// </summary>
public interface IMultipleInterfaceService1 { }
public interface IMultipleInterfaceService2 { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceWithMultipleInterfaces : IMultipleInterfaceService1, IMultipleInterfaceService2 { }

/// <summary>
/// Represents a service with constructor dependencies
/// </summary>
public interface IServiceWithDependencies { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceWithDependencies : IServiceWithDependencies
{
    public ServiceWithDependencies(IServiceB serviceB, IServiceC serviceC)
    {
        // Constructor with dependencies
    }
}

/// <summary>
/// Represents a service with complex dependency chain
/// </summary>
public interface IServiceWithComplexDependencies { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceWithComplexDependencies : IServiceWithComplexDependencies
{
    public ServiceWithComplexDependencies(IServiceWithDependencies serviceWithDependencies)
    {
        // Complex dependency chain
    }
}

/// <summary>
/// Represents a simple service without generics
/// </summary>
public interface ISimpleService { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class SimpleService : ISimpleService { }

/// <summary>
/// Represents a nested service
/// </summary>
public interface INestedService { }

public class OuterClass
{
    [AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
    public class NestedService : INestedService { }
}

/// <summary>
/// Represents services with different access modifiers
/// </summary>
public interface IPublicService { }
public interface IInternalService { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class PublicService : IPublicService { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
internal class InternalService : IInternalService { }

/// <summary>
/// Represents a disposable service
/// </summary>
public interface IDisposableService : IDisposable { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class DisposableService : IDisposableService
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

/// <summary>
/// Represents a service with optional dependencies
/// </summary>
public interface IServiceWithOptionalDependencies { }

[AutoRegister(Lifetime.Transient, RegisterAs.Interface)]
public class ServiceWithOptionalDependencies : IServiceWithOptionalDependencies
{
    public ServiceWithOptionalDependencies(IServiceB? optionalService = null)
    {
        // Constructor with optional dependency
    }
}

[Category("Unit")] 
internal class IoCExtensionsTests
{
    private IServiceCollection _services = null!;
    private ServiceProvider _serviceProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _services = new ServiceCollection();
        _services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        _serviceProvider = _services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider?.Dispose();
    }

    /// <summary>
    /// Verifies that services without the [AutoRegister] attribute are not registered in the DI container
    /// and cannot be resolved, ensuring only explicitly marked services are auto-registered.
    /// </summary>
    [Test]
    public void Non_Registered_Service_Should_Not_Be_Resolvable()
    {
        // Act
        var serviceA1 = _serviceProvider.GetService<ServiceA>();
        var serviceA2 = _serviceProvider.GetService<IServiceA>();

        // Assert
        serviceA1.Should().BeNull();
        serviceA2.Should().BeNull();
    }

    /// <summary>
    /// Verifies that services registered with Transient lifetime create a new instance each time
    /// they are resolved, ensuring proper transient behavior for stateless services.
    /// </summary>
    [Test]
    public void Transient_Lifetime_Service_Should_Be_Created_Each_Time_When_Resolved()
    {
        // Act
        var serviceB1 = _serviceProvider.GetService<IServiceB>();
        var serviceB2 = _serviceProvider.GetService<IServiceB>();

        // Assert: Transient lifetime services are created each time they are requested
        serviceB1.Should().NotBeNull().And.BeAssignableTo<ServiceB>();
        serviceB2.Should().NotBeNull().And.BeAssignableTo<ServiceB>();
        serviceB1.Should().NotBeSameAs(serviceB2); // Different instances due to Transient lifetime
    }

    /// <summary>
    /// Verifies that services registered with Singleton lifetime create only one instance
    /// throughout the application lifetime, ensuring proper singleton behavior for shared state.
    /// </summary>
    [Test]
    public void Singleton_Lifetime_Service_Should_Be_Created_Only_Once()
    {
        // Act
        var serviceC1 = _serviceProvider.GetService<IServiceC>();
        var serviceC2 = _serviceProvider.GetService<IServiceC>();

        // Assert: Singleton lifetime services once created will remain
        serviceC1.Should().NotBeNull().And.BeAssignableTo<ServiceC>();
        serviceC2.Should().NotBeNull().And.BeAssignableTo<ServiceC>();
        serviceC1.Should().BeSameAs(serviceC2); // Same instance due to Singleton lifetime
    }

    /// <summary>
    /// Verifies that services registered with Scoped lifetime create one instance per scope
    /// but different instances across different scopes, ensuring proper scoped behavior for request-scoped services.
    /// </summary>
    [Test]
    public void Scoped_Lifetime_Services_Should_Be_Created_Once_Per_Request()
    {
        IServiceD? serviceD1 = null;
        IServiceD? serviceD2 = null;
        
        using (var scope = _serviceProvider.CreateScope())
        {
            serviceD1 = scope.ServiceProvider.GetService<IServiceD>();
            serviceD2 = scope.ServiceProvider.GetService<IServiceD>();

            // Assert
            serviceD1.Should().NotBeNull().And.BeAssignableTo<ServiceD>();
            serviceD2.Should().NotBeNull().And.BeAssignableTo<ServiceD>();
            serviceD1.Should().BeSameAs(serviceD2); // Same instance in the same scope
        }

        IServiceD? serviceD3 = null;

        using (var scope = _serviceProvider.CreateScope())
        {
            serviceD3 = scope.ServiceProvider.GetService<IServiceD>();

            // Assert
            serviceD1.Should().NotBeNull().And.BeAssignableTo<ServiceD>();
            serviceD3.Should().NotBeNull().And.BeAssignableTo<ServiceD>();
            serviceD3.Should().NotBeSameAs(serviceD1); // Different instance in different scopes
        }
    }

    /// <summary>
    /// Verifies that services registered with RegisterAs.Self can only be resolved as their concrete type
    /// and not as their interface, ensuring proper self-registration behavior.
    /// </summary>
    [Test]
    public void Service_Registered_As_Self_Should_Be_Resolvable_As_Itself_And_Not_As_Interface()
    {
        // Act
        var serviceEAsSelf = _serviceProvider.GetService<ServiceE>();
        var serviceEAsInterface = _serviceProvider.GetService<IServiceE>();

        // Assert
        serviceEAsSelf.Should().NotBeNull().And.BeAssignableTo<ServiceE>();
        serviceEAsInterface.Should().BeNull(); // Not resolvable as the interface
    }

    /// <summary>
    /// Verifies that services registered with RegisterAs.Interface can only be resolved as their interface
    /// and not as their concrete type, ensuring proper interface-registration behavior.
    /// </summary>
    [Test]
    public void Service_Registered_As_Interface_Should_Be_Resolvable_As_Interface_And_Not_As_Self()
    {
        // Act
        var serviceFAsInterface = _serviceProvider.GetService<IServiceF>();
        var serviceFAsSelf = _serviceProvider.GetService<ServiceF>();

        // Assert
        serviceFAsInterface.Should().NotBeNull().And.BeAssignableTo<ServiceF>();
        serviceFAsSelf.Should().BeNull(); // Not resolvable as self
    }

    /// <summary>
    /// Verifies that services registered with both RegisterAs.Self and RegisterAs.Interface
    /// can be resolved as both their concrete type and interface, ensuring flexible registration behavior.
    /// </summary>
    [Test]
    public void Service_Registered_Both_As_Self_And_Interface_Should_Be_Resolvable_As_Self_And_Interface()
    {
        // Act
        var serviceGAsInterface = _serviceProvider.GetService<IServiceG>();
        var serviceGAsSelf = _serviceProvider.GetService<ServiceG>();

        // Assert
        serviceGAsInterface.Should().NotBeNull().And.BeAssignableTo<ServiceG>();
        serviceGAsSelf.Should().NotBeNull().And.BeAssignableTo<ServiceG>();
    }

    /// <summary>
    /// Verifies that the AddAutoRegisteredServicesFromAssembly method only registers services
    /// from the specified assemblies, ensuring proper assembly-specific registration behavior.
    /// </summary>
    [Test]
    public void AddAutoRegisteredServices_WithSpecificAssemblies_ShouldOnlyRegisterFromThoseAssemblies()
    {
        // Arrange
        var services = new ServiceCollection();
        var currentAssembly = Assembly.GetExecutingAssembly();

        // Act
        services.AddAutoRegisteredServicesFromAssembly(currentAssembly);
        var serviceProvider = services.BuildServiceProvider();

        // Assert - Should register services from this test assembly
        var serviceB = serviceProvider.GetService<IServiceB>();
        serviceB.Should().NotBeNull().And.BeAssignableTo<ServiceB>();
    }

    /// <summary>
    /// Verifies that the AddAutoRegisteredServicesFromAssembly method throws an ArgumentException
    /// when called with an empty array of assemblies, ensuring proper validation of input parameters.
    /// </summary>
    [Test]
    public void AddAutoRegisteredServices_WithEmptyAssemblies_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var act = () => services.AddAutoRegisteredServicesFromAssembly(new Assembly[0]);
        act.Should().Throw<ArgumentException>()
            .WithMessage("At least one assembly must be specified.*")
            .And.ParamName.Should().Be("assemblies");
    }

    /// <summary>
    /// Verifies that the AddAutoRegisteredServicesFromAssembly method throws an ArgumentException
    /// when called with null assemblies, ensuring proper validation of input parameters.
    /// </summary>
    [Test]
    public void AddAutoRegisteredServices_WithNullAssemblies_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var act = () => services.AddAutoRegisteredServicesFromAssembly(null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("At least one assembly must be specified.*")
            .And.ParamName.Should().Be("assemblies");
    }

    /// <summary>
    /// Verifies that services without interfaces are skipped when registered as Interface,
    /// ensuring proper handling of services that cannot be registered as interfaces.
    /// </summary>
    [Test]
    public void Service_WithNoInterface_RegisteredAsInterface_ShouldBeSkipped()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<ServiceWithoutInterface>();

        // Assert
        service.Should().BeNull(); // Should not be registered because it has no interface
    }

    /// <summary>
    /// Verifies that services implementing multiple interfaces are registered only as the first interface,
    /// ensuring proper handling of services with multiple interface implementations.
    /// </summary>
    [Test]
    public void Service_WithMultipleInterfaces_ShouldRegisterAsFirstInterface()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var serviceAsFirstInterface = serviceProvider.GetService<IMultipleInterfaceService1>();
        var serviceAsSecondInterface = serviceProvider.GetService<IMultipleInterfaceService2>();

        // Assert
        serviceAsFirstInterface.Should().NotBeNull().And.BeAssignableTo<ServiceWithMultipleInterfaces>();
        serviceAsSecondInterface.Should().BeNull(); // Only first interface should be registered
    }

    /// <summary>
    /// Verifies that services with constructor dependencies can be resolved correctly,
    /// ensuring proper dependency injection for services with required dependencies.
    /// </summary>
    [Test]
    public void Service_WithConstructorDependencies_ShouldResolveDependencies()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IServiceWithDependencies>();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<ServiceWithDependencies>();
        service.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that services with complex dependency chains can be resolved correctly,
    /// ensuring proper dependency injection for services with nested dependencies.
    /// </summary>
    [Test]
    public void Service_WithComplexDependencyChain_ShouldResolveAllDependencies()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IServiceWithComplexDependencies>();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<ServiceWithComplexDependencies>();
    }

    /// <summary>
    /// Verifies that simple services can be registered and resolved correctly,
    /// ensuring proper handling of basic service registration without generics.
    /// </summary>
    [Test]
    public void Service_WithSimpleType_ShouldBeRegisteredCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<ISimpleService>();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<SimpleService>();
    }

    /// <summary>
    /// Verifies that nested services can be registered and resolved correctly,
    /// ensuring proper handling of nested class types in service registration.
    /// </summary>
    [Test]
    public void Service_WithNestedType_ShouldBeRegisteredCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<INestedService>();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<OuterClass.NestedService>();
    }

    /// <summary>
    /// Verifies that services with different access modifiers can be registered and resolved correctly,
    /// ensuring proper handling of public and internal services in service registration.
    /// </summary>
    [Test]
    public void Service_WithDifferentAccessModifiers_ShouldBeRegisteredCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var publicService = serviceProvider.GetService<IPublicService>();
        var internalService = serviceProvider.GetService<IInternalService>();

        // Assert
        publicService.Should().NotBeNull().And.BeAssignableTo<PublicService>();
        internalService.Should().NotBeNull().And.BeAssignableTo<InternalService>();
    }

    /// <summary>
    /// Verifies that disposable services are properly disposed when the service provider is disposed,
    /// ensuring proper cleanup of resources for services implementing IDisposable.
    /// </summary>
    [Test]
    public void Service_WithIDisposable_ShouldBeDisposedCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IDisposableService>();
        serviceProvider.Dispose();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<DisposableService>();
        ((DisposableService)service).IsDisposed.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that services with optional dependencies can be resolved correctly,
    /// ensuring proper handling of services with nullable or optional constructor parameters.
    /// </summary>
    [Test]
    public void Service_WithOptionalDependencies_ShouldResolveCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAutoRegisteredServicesFromAssembly(Assembly.GetExecutingAssembly());
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<IServiceWithOptionalDependencies>();

        // Assert
        service.Should().NotBeNull().And.BeAssignableTo<ServiceWithOptionalDependencies>();
    }
}
