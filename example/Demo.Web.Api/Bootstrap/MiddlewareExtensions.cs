using Microsoft.OpenApi;

namespace Demo.Web.Api.Bootstrap;

internal static class MiddlewareExtensions
{
    /// <summary>
    /// Configures the middleware pipeline for the API
    /// </summary>
    public static WebApplication ConfigureApiMiddleware(this WebApplication app)
    {
        //TODO: Disable Swagger in deployed environments
        //if (app.Environment.IsDevelopment())
        app.UseSwagger(opt => opt.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0);
        app.UseSwaggerUI(opt => opt.EnableTryItOutByDefault());
        app.MapGet("/", () => "Demo API");

        // Add health check endpoints
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });
        app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = _ => false
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors(policyName: "TODO_MyOpenDoorPolicy");

        return app;
    }
}