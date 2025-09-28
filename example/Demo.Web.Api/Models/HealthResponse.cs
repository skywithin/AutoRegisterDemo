namespace Demo.Web.Api.Models;


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

