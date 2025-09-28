namespace Demo.Web.Api.Bootstrap;

internal static class DatabaseExtensions
{
    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public static WebApplication SeedDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            //var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            //SeedDatabase.Run(dbContext);
        }

        return app;
    }
}