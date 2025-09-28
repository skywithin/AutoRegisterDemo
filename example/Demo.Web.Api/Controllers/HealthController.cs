using Microsoft.AspNetCore.Mvc;

namespace Demo.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var response = new HealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        };

        return Ok(response);
    }

    [HttpGet("detailed")]
    public IActionResult GetDetailed()
    {
        var response = new DetailedHealthResponse
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

        return Ok(response);
    }
}

/// <summary>
/// Basic health response model
/// </summary>
public class HealthResponse
{
    /// <summary>
    /// Current health status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the health check
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// API version
    /// </summary>
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Detailed health response model
/// </summary>
public class DetailedHealthResponse : HealthResponse
{
    /// <summary>
    /// Current environment
    /// </summary>
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// Machine name where the API is running
    /// </summary>
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// System uptime
    /// </summary>
    public TimeSpan Uptime { get; set; }

    /// <summary>
    /// Status of various services
    /// </summary>
    public Dictionary<string, string> Services { get; set; } = new();
}
