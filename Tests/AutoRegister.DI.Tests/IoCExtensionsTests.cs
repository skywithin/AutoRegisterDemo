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
}
