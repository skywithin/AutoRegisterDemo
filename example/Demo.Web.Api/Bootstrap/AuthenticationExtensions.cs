namespace Demo.Web.Api.Bootstrap;

internal static class AuthenticationExtensions
{
    /// <summary>
    /// Configures authentication and authorization services for the API
    /// </summary>
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services)
    {
        // Configure authentication services for the API
        //services
        //    .ConfigureOptions<AuthenticationOptionsSetup>() //Binds AuthenticationOptions using appsettings
        //    .ConfigureOptions<ApiAuthenticationSetup>()     //Configure JwtBearer
        //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer();

        return services;
    }

    /// <summary>
    /// Configures authorization policies for the API
    /// </summary>
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        //services
        //    .AddAuthorization(options =>
        //    {
        //
        //    });

        return services;
    }
}