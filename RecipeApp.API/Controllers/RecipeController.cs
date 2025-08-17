using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Models;
using RecipeApp.Application.DTOs;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Models;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly ILogger<RecipesController> _logger;

    public RecipesController(IRecipeService recipeService, ILogger<RecipesController> logger)
    {
        _recipeService = recipeService;
        _logger = logger;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<RecipeDTO>>> GetRecipes(
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = "",
        [FromQuery] int? difficulty = null,
        [FromQuery] RecipeSortType sortType = RecipeSortType.Date,
        [FromQuery] SortDirection sortDirection = SortDirection.Descending)
    {
        var queryOptions = new RecipeQueryOptions
        {
            UserId = Guid.Empty,
            CurrentPage = currentPage,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Difficulty = difficulty,
            SortType = sortType,
            SortDirection = sortDirection
        };
        return await QueryRecipes("recipes/all", queryOptions);
    }

    [HttpGet("my")]
    public async Task<ActionResult<PagedResult<RecipeDTO>>> GetUserRecipes(
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = "",
        [FromQuery] int? difficulty = null,
        [FromQuery] RecipeSortType sortType = RecipeSortType.Date,
        [FromQuery] SortDirection sortDirection = SortDirection.Descending)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var queryOptions = new RecipeQueryOptions
        {
            UserId = userId.Value,
            CurrentPage = currentPage,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Difficulty = difficulty,
            SortType = sortType,
            SortDirection = sortDirection
        };
        return await QueryRecipes("recipes/my", queryOptions);
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<PagedResult<RecipeDTO>>> GetFavoriteRecipes(
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = "",
        [FromQuery] int? difficulty = null,
        [FromQuery] RecipeSortType sortType = RecipeSortType.Date,
        [FromQuery] SortDirection sortDirection = SortDirection.Descending)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        var queryOptions = new RecipeQueryOptions
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Difficulty = difficulty,
            SortType = sortType,
            SortDirection = sortDirection
        };

        _logger.LogInformation($"Query options for favorites: {queryOptions}");

        var pagedData = await _recipeService.GetFavoriteRecipes(userId.Value, queryOptions);
        var pagedResult = new PagedResult<RecipeDTO>(
            "recipes/favorites",
            pagedData,
            queryOptions);

        _logger.LogInformation($"Retrieved page {queryOptions.CurrentPage} of favorite recipes out of {pagedResult.Metadata.TotalPages} (total recipes = {pagedData.TotalCount})");

        return Ok(pagedResult);
    }

    private async Task<ActionResult<PagedResult<RecipeDTO>>> QueryRecipes(string sourceEndpoint, RecipeQueryOptions queryOptions)
    {
        _logger.LogInformation($"Query options: {queryOptions}");

        var pagedData = await _recipeService.GetRecipes(queryOptions);
        var pagedResult = new PagedResult<RecipeDTO>(
            sourceEndpoint,
            pagedData,
            queryOptions);

        _logger.LogInformation($"Retrieved page {queryOptions.CurrentPage} of recipes out of {pagedResult.Metadata.TotalPages} (total recipes = {pagedData.TotalCount})");

        return Ok(pagedResult);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<RecipeDTO>> GetRecipeById(Guid id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);

        if (recipe is null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> FavoriteRecipe([FromBody] FavoriteRecipeRequest request)
    {
        var loggedInUserId = GetCurrentUserId();
        if (!loggedInUserId.HasValue || loggedInUserId != request.UserId)
        {
            return Unauthorized();
        }

        await _recipeService.FavoriteRecipe(request.UserId, request.RecipeId);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var authorId = GetCurrentUserId();
        if (authorId is null)
        {
            return Unauthorized();
        }

        var newRecipe = await _recipeService.CreateRecipeAsync(createRecipeDTO, authorId.Value);

        return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.Id }, newRecipe);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRecipe(Guid id, [FromBody] UpdateRecipeDTO updateRecipeDTO)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var success = await _recipeService.UpdateRecipeAsync(id, updateRecipeDTO, userId.Value);

        if (!success)
        {
            // This could be because the recipe doesn't exist OR the user doesn't own it.
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRecipe(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var success = await _recipeService.DeleteRecipeAsync(id, userId.Value);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }
}