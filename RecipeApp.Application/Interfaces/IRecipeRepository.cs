using RecipeApp.Application.Models;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Application.Interfaces;

public interface IRecipeRepository
{
    Task<Recipe?> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetPageAsync(RecipeQueryOptions queryOptions);
    Task<int> GetTotalCountAsync(RecipeQueryOptions queryOptions);
    Task<IEnumerable<Recipe>> GetFavoriteRecipesPageAsync(Guid userId, RecipeQueryOptions queryOptions);
    Task<int> GetFavoriteRecipesTotalCountAsync(Guid userId, RecipeQueryOptions queryOptions);
    Task AddAsync(Recipe recipe);
    void Update(Recipe recipe);
    void Delete(Recipe recipe);
    Task<int> SaveChangesAsync();
}