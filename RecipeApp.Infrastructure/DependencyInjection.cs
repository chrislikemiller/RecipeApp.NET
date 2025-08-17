using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Application.Interfaces;
using RecipeApp.Infrastructure.Persistence;
using RecipeApp.Infrastructure.Repositories;

namespace RecipeApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (!Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Testing") == true)
        {
            services.AddScoped<DummyDataDbContextInitializer>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
        }

        services.AddScoped<IRecipeRepository, RecipeRepository>();

        return services;
    }
}
