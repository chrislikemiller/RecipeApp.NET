using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using RecipeApp.Api.Tests.Common;
using RecipeApp.Application.DTOs;
using RecipeApp.Domain.Entities;
using RecipeApp.Infrastructure.Persistence;
using RecipeApp.Infrastructure.Repositories;
namespace RecipeApp.API.Tests.Integration.Controller;

[TestFixture]
public class RecipesControllerIntegrationTests
{
    private TestWebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new TestWebApplicationFactory<Program>();
    }

    [SetUp]
    public void Setup()
    {
        _client = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }


    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _factory.Dispose();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task GetAllRecipes_ShouldReturnSomething_WhenRecipesAreAdded()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe
        {
            Id = recipeId,
            Title = "Test Recipe From DB",
            AuthorId = Guid.NewGuid()
        };

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Recipes.AddAsync(recipe);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/recipes/");

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<RecipeDTO>>();

        Assert.That(recipes, Is.Not.Null);
        Assert.That(recipes.Count(), Is.AtLeast(1));
        Assert.That(recipes.First().Id, Is.EqualTo(recipeId));
        Assert.That(recipes.First().Title, Is.EqualTo("Test Recipe From DB"));
    }

    [Test]
    public async Task GetRecipeById_ShouldReturnNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var unknownId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/recipes/{unknownId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetRecipeById_ShouldReturnRecipe_WhenRecipeExists()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe
        {
            Id = recipeId,
            Title = "Test Recipe From DB",
            AuthorId = Guid.NewGuid()
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Recipes.AddAsync(recipe);
            await context.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync($"/api/recipes/{recipeId}");

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.True);
        var recipeDTO = await response.Content.ReadFromJsonAsync<RecipeDTO>();

        Assert.That(recipeDTO, Is.Not.Null);
        Assert.That(recipeDTO.Id, Is.EqualTo(recipeId));
        Assert.That(recipeDTO.Title, Is.EqualTo("Test Recipe From DB"));
    }
}