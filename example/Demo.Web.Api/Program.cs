using Demo.Web.Api.Bootstrap;
using Serilog;

namespace Demo.Web.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host
            .UseSerilog((context, provider, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();
            });

        builder.Services.AddDemoApiServices();

        var app = builder.Build();

        app.ConfigureApiMiddleware();

        app.Run();
    }
}
