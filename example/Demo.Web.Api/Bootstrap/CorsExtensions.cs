namespace Demo.Web.Api.Bootstrap;

internal static class CorsExtensions
{
    /// <summary>
    /// Configures CORS for the API
    /// </summary>
    public static IServiceCollection AddApiCors(this IServiceCollection services)
    {
        services
            .AddCors(opt =>
                opt.AddPolicy(
                    name: "TODO_MyOpenDoorPolicy",
                    builder =>
                    {
                        builder
                            // TODO: Allow everything for now. Needs to be properly implemented.
                            //.WithOrigins("http://foo.com", "http://bar.com")
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }));

        return services;
    }
}