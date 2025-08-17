using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Infrastructure.Persistence;

namespace RecipeApp.Tests.Common;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Find the service descriptor that registers the DbContext.
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            // 2. If found, remove it.
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // 3. Add a new DbContext registration that uses an in-memory SQLite database.
            // A new database is created for each test.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("DataSource=file:inmem?mode=memory&cache=shared");
            });

            // 4. Ensure the database is created.
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        });
    }
}