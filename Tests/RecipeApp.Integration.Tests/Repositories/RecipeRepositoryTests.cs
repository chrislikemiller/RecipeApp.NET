using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipeApp.Domain.Entities;
using RecipeApp.Infrastructure.Persistence;
using RecipeApp.Infrastructure.Repositories;

namespace RecipeApp.Infrastructure.Tests.Unit.Repositories;

[TestFixture]
public class RecipeRepositoryTests
{
    private ApplicationDbContext _context;
    private RecipeRepository _recipeRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _recipeRepository = new RecipeRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnRecipe_WhenRecipeExists()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe { Id = recipeId, Title = "Test Recipe" };
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();

        // Act
        var result = await _recipeRepository.GetByIdAsync(recipeId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(recipe.Id));
        Assert.That(result.Title, Is.EqualTo(recipe.Title));
    }
}
