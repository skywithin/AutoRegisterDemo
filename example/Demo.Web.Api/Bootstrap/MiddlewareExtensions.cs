using Microsoft.OpenApi;

namespace Demo.Web.Api.Bootstrap;

internal static class MiddlewareExtensions
{
    /// <summary>
    /// Configures the middleware pipeline for the API
    /// </summary>
    public static WebApplication ConfigureApiMiddleware(this WebApplication app)
    {
        app.UseSwagger(opt => opt.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0);
        app.UseSwaggerUI(opt => opt.EnableTryItOutByDefault());
        app.MapGet("/", () => "Demo API");

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}