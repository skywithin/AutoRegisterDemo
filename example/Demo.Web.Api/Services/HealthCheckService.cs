using AutoRegister.DI;
using Demo.Web.Api.Models;

namespace Demo.Web.Api.Services;

public interface IHealthCheckService
{
    HealthResponse GetBasicHealthReport();
    HealthResponse GetDetailedHealthReport();
}

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class HealthCheckService : IHealthCheckService
{
    // For demonstration, we return static data.
    public HealthResponse GetBasicHealthReport()
    {
        return new HealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        };
    }

    // For demonstration, we return static data.
    public HealthResponse GetDetailedHealthReport()
    {
        return new DetailedHealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            MachineName = Environment.MachineName,
            Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
            Services = new Dictionary<string, string>
            {
                { "Database", "Connected" },
                { "Cache", "Available" },
                { "External API", "Reachable" }
            }
        };
    }
}
