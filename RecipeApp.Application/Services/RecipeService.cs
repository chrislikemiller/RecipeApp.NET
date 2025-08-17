using Mapster;
using RecipeApp.API.Models;
using RecipeApp.Application.DTOs;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Models;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;

    public RecipeService(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeDTO?> GetRecipeByIdAsync(Guid id)
    {
        var recipe = await _recipeRepository.GetByIdAsync(id);
        return recipe?.Adapt<RecipeDTO>();
    }

    public async Task<PagedDataDTO<RecipeDTO>> GetRecipes(RecipeQueryOptions queryOptions)
    {
        var recipes = await _recipeRepository.GetPageAsync(queryOptions);
        int totalCount = await _recipeRepository.GetTotalCountAsync(queryOptions);
        var data = recipes.Adapt<IEnumerable<RecipeDTO>>();
        return new PagedDataDTO<RecipeDTO>
        {
            Data = data,
            TotalCount = totalCount
        };
    }

    public async Task<PagedDataDTO<RecipeDTO>> GetFavoriteRecipes(Guid userId, RecipeQueryOptions queryOptions)
    {
        var recipes = await _recipeRepository.GetFavoriteRecipesPageAsync(userId, queryOptions);
        int totalCount = await _recipeRepository.GetFavoriteRecipesTotalCountAsync(userId, queryOptions);
        var data = recipes.Adapt<IEnumerable<RecipeDTO>>();
        return new PagedDataDTO<RecipeDTO>
        {
            Data = data,
            TotalCount = totalCount
        };
    }

    public async Task<RecipeDTO> CreateRecipeAsync(CreateRecipeDTO createRecipeDTO, Guid authorId)
    {
        var recipe = createRecipeDTO.Adapt<Recipe>();
        recipe.Id = Guid.NewGuid();
        recipe.AuthorId = authorId;
        recipe.CreatedAtUtc = DateTime.UtcNow;

        await _recipeRepository.AddAsync(recipe);
        await _recipeRepository.SaveChangesAsync();

        return recipe.Adapt<RecipeDTO>();
    }

    public async Task<bool> UpdateRecipeAsync(Guid id, UpdateRecipeDTO updateRecipeDTO, Guid userId)
    {
        var recipe = await _recipeRepository.GetByIdAsync(id);
        if (recipe is null || recipe.AuthorId != userId)
        {
            return false;
        }

        updateRecipeDTO.Adapt(recipe);

        _recipeRepository.Update(recipe);
        await _recipeRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRecipeAsync(Guid id, Guid userId)
    {
        var recipe = await _recipeRepository.GetByIdAsync(id);
        if (recipe is null || recipe.AuthorId != userId)
        {
            return false;
        }

        _recipeRepository.Delete(recipe);
        await _recipeRepository.SaveChangesAsync();
        return true;
    }

    // todo: move this to UserService? compose repositories?
    public async Task FavoriteRecipe(Guid userId, Guid recipeId)
    {
        var recipe = await _recipeRepository.GetByIdAsync(recipeId);
        if (recipe is null)
        {
            throw new KeyNotFoundException($"Recipe with ID {recipeId} not found.");
        }
        var favRecipe = recipe.FavoritedByUsers.FirstOrDefault(u => u.UserId == userId);
        if (favRecipe == null)
        {
            recipe.FavoritedByUsers.Add(new FavoriteRecipe { UserId = userId, RecipeId = recipeId });
            _recipeRepository.Update(recipe);
            await _recipeRepository.SaveChangesAsync();
        }
        else
        {
            recipe.FavoritedByUsers.Remove(favRecipe);
            _recipeRepository.Update(recipe);
            await _recipeRepository.SaveChangesAsync();

        }
    }

}