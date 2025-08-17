using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Infrastructure.Persistence;

public class DummyDataDbContextInitializer
{
    private readonly ILogger<DummyDataDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DummyDataDbContextInitializer(
        ILogger<DummyDataDbContextInitializer> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (_userManager.Users.Any())
        {
            return;
        }

        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            _logger.LogInformation("Seeded default roles.");
        }

        var defaultUser = new User
        {
            Email = "testuser@example.com",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(defaultUser, "Password123!");
        await _userManager.AddToRoleAsync(defaultUser, "User");
        _logger.LogInformation("Seeded default user 'testuser@example.com'.");

        // 3. Seed recipes for the default user
        // This only runs if the user was just created.
        if (_context.Recipes.Any())
        {
            return; // Database has recipes, so we don't seed them.
        }

        var recipes = new List<Recipe>
            {
                new()
                {
                    Title = "Classic Spaghetti Carbonara",
                    Description = "1. Cook pasta. 2. Mix eggs and cheese. 3. Combine all.",
                    Difficulty = 3,
                    AuthorId = defaultUser.Id,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new()
                {
                    Title = "Simple Chicken Stir-Fry",
                    Description = "1. Chop veggies. 2. Cook chicken. 3. Stir-fry everything with soy sauce.",
                    Difficulty = 2,
                    AuthorId = defaultUser.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddHours(-1)
                },
                new()
                {
                    Title = "Avocado Toast",
                    Description = "1. Toast bread. 2. Mash avocado on top. 3. Season.",
                    Difficulty = 1,
                    AuthorId = defaultUser.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddHours(-2)
                },
                new()
                {
                    Title = "Gourmet Beef Wellington",
                    Description = "A very complicated recipe involving puff pastry, mushrooms, and beef tenderloin.",
                    Difficulty = 5,
                    AuthorId = defaultUser.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddHours(-3)
                },
                new()
                {
                    Title = "Quick Microwave Mug Cake",
                    Description = "1. Mix ingredients in a mug. 2. Microwave for 90 seconds.",
                    Difficulty = 1,
                    AuthorId = defaultUser.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddHours(-4)
                }
            };

        await _context.Recipes.AddRangeAsync(recipes);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded 5 recipes for the default user.");
    }
}