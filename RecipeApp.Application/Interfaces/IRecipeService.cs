using RecipeApp.API.Models;
using RecipeApp.Application.DTOs;
using RecipeApp.Application.Models;

namespace RecipeApp.Application.Interfaces;

public interface IRecipeService
{
    Task<RecipeDTO?> GetRecipeByIdAsync(Guid id);
    Task<PagedDataDTO<RecipeDTO>> GetRecipes(RecipeQueryOptions queryOptions);
    Task<PagedDataDTO<RecipeDTO>> GetFavoriteRecipes(Guid userId, RecipeQueryOptions queryOptions);
    Task<RecipeDTO> CreateRecipeAsync(CreateRecipeDTO createRecipeDTO, Guid authorId);
    Task<bool> UpdateRecipeAsync(Guid id, UpdateRecipeDTO updateRecipeDTO, Guid userId);
    Task<bool> DeleteRecipeAsync(Guid id, Guid userId);
    Task FavoriteRecipe(Guid userId, Guid recipeId);
}