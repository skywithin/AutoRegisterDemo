using Demo.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(IHealthCheckService healthCheck) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var result = healthCheck.GetBasicHealthReport();

        return Ok(result);
    }

    [HttpGet("detailed")]
    public IActionResult GetDetailed()
    {
        var result = healthCheck.GetDetailedHealthReport();

        return Ok(result);
    }
}
