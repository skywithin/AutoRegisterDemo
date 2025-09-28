using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Demo.Web.Api.Bootstrap;

internal static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger/OpenAPI for the Demo API
    /// </summary>
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services
           .AddSwaggerGen(options =>
           {
               options.AddSecurityDefinition(
                   name: "Bearer",
                   securityScheme:
                       new OpenApiSecurityScheme
                       {
                           Description = @"Please enter JWT with Bearer into field. Example: 'Bearer 12345abcdef'",
                           Name = "Authorization",
                           In = ParameterLocation.Header,
                           Type = SecuritySchemeType.ApiKey,
                           Scheme = "Bearer"
                       });

               options.AddSecurityRequirement(
                   new OpenApiSecurityRequirement
                   {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                            },
                            new List<string>()
                        }
                   });
           });

        return services;
    }
}