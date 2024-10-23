# Auto-Wired Services Extension

This extension method simplifies service registration in an ASP.NET Core application by automatically registering services that are flagged with the `AutoWireAttribute`. It supports various lifetimes (`Scoped`, `Transient`, and `Singleton`) and allows for registration as either the implementation type (self) or as an interface.

## Features

- Automatically registers services flagged with the `AutoWireAttribute`.
- Supports three lifetimes: `Scoped`, `Transient`, and `Singleton`.
- Allows registration of services either as the implementation type (self) or as an interface.
- Scans all loaded assemblies in the current AppDomain for eligible services.

## Installation

To use this feature, add the extension method in your ASP.NET Core project. Ensure that your services are flagged with the `AutoWireAttribute`.

## Usage

1. **Define your services with the `AutoWireAttribute`:**

   Use the `AutoWireAttribute` to decorate your service classes. You can specify the lifetime and how the service should be registered (as `Self`, `Interface`, or both).

   ```
   [AutoWire(Lifetime.Singleton, RegisterAs.Interface | RegisterAs.Self)]
   public class MyService : IMyService
   {
       // Implementation...
   }
   ```

1. Add the Auto-Wired Services in `Program` or `Startup`:

   ```
   public class Program
   {
      public static void Main(string[] args)
      {
          var builder = WebApplication.CreateBuilder(args);
          builder.Services.AddAutoWiredServices();

          // Other config...
          
          app.Run();
      }
   }
   ```
