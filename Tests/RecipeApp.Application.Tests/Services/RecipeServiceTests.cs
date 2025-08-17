using NSubstitute;
using NUnit.Framework;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Services;
using RecipeApp.Domain.Entities;
using MapsterMapper;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Tests.Unit.Services;

[TestFixture]
public class RecipeServiceTests
{
    private IRecipeRepository _recipeRepository;
    private RecipeService _recipeService;

    [SetUp]
    public void Setup()
    {
        _recipeRepository = Substitute.For<IRecipeRepository>();
        _recipeService = new RecipeService(_recipeRepository);
    }

    [Test]
    public async Task GetRecipeByIdAsync_ShouldReturnNull_WhenRecipeDoesNotExist()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        _recipeRepository.GetByIdAsync(recipeId).Returns((Recipe?)null);

        // Act
        var result = await _recipeService.GetRecipeByIdAsync(recipeId);

        // Assert
        Assert.That(result, Is.Null);
        await _recipeRepository.Received(1).GetByIdAsync(recipeId);
    }

    [Test]
    public async Task GetRecipeByIdAsync_ShouldReturnObject_WhenRecipeExists()
    {
        // Arrange
        var instructions = "Mix ingredients and bake for 30 minutes.";
        var recipeId = Guid.NewGuid();
        _recipeRepository.GetByIdAsync(recipeId).Returns(
            new Recipe { Id = recipeId, Description = instructions });

        // Act
        var result = await _recipeService.GetRecipeByIdAsync(recipeId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(recipeId));
        Assert.That(result.Description, Is.EqualTo(instructions));
        await _recipeRepository.Received(1).GetByIdAsync(recipeId);
    }
}