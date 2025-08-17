using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Infrastructure.Persistence;

namespace RecipeApp.Api.Tests.Common;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private DbConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureTestDatabase(services);

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        });
    }

    private static void ConfigureTestDatabase(IServiceCollection services)
    {
        // Note: no need to remove existing DbContext
        // Adding one depends on not being in testing environment
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase("TestDb");
        });

        var dbInitializer = services
            .SingleOrDefault(d => d.ServiceType == typeof(DummyDataDbContextInitializer));
        if (dbInitializer != null)
        {
            services.Remove(dbInitializer);
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}