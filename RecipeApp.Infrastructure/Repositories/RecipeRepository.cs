using Microsoft.EntityFrameworkCore;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Models;
using RecipeApp.Domain.Entities;
using RecipeApp.Infrastructure.Persistence;

namespace RecipeApp.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _context;

    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        return await _context.Recipes
            .Include(r => r.FavoritedByUsers)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<int> GetAllRecipesTotalCountAsync()
    {
        return _context.Recipes.AsNoTracking().CountAsync();
    }

    public Task<int> GetTotalCountAsync(RecipeQueryOptions queryOptions)
    {
        return RecipesAsFilteredQueryable(queryOptions).CountAsync();
    }

    public async Task<IEnumerable<Recipe>> GetPageAsync(RecipeQueryOptions queryOptions)
    {
        return await RecipesAsFilteredQueryable(queryOptions)
            .Include(x => x.Author)
            .Include(x => x.Ratings)
            .Include(x => x.Tags)
            .Include(x => x.FavoritedByUsers)
            .Skip((queryOptions.CurrentPage - 1) * queryOptions.PageSize)
            .Take(queryOptions.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recipe>> GetFavoriteRecipesPageAsync(Guid userId, RecipeQueryOptions queryOptions)
    {
        return await FavoriteRecipesAsFilteredQueryable(userId, queryOptions)
            .Include(x => x.Author)
            .Include(x => x.Ratings)
            .Include(x => x.Tags)
            .Include(x => x.FavoritedByUsers)
            .Skip((queryOptions.CurrentPage - 1) * queryOptions.PageSize)
            .Take(queryOptions.PageSize)
            .ToListAsync();
    }

    public Task<int> GetFavoriteRecipesTotalCountAsync(Guid userId, RecipeQueryOptions queryOptions)
    {
        return FavoriteRecipesAsFilteredQueryable(userId, queryOptions).CountAsync();
    }

    private IQueryable<Recipe> RecipesAsFilteredQueryable(RecipeQueryOptions queryOptions)
    {
        return _context.Recipes
            .AsNoTracking()
            .AsQueryable()
            .WithFilters(queryOptions);
    }

    private IQueryable<Recipe> FavoriteRecipesAsFilteredQueryable(Guid userId, RecipeQueryOptions queryOptions)
    {
        return _context.Recipes
            .AsNoTracking()
            .Where(x => x
                .FavoritedByUsers
                .Any(y => y.UserId == userId))
            .WithBasicFilters(queryOptions);
    }

    public async Task AddAsync(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }

    public void Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
    }

    public void Delete(Recipe recipe)
    {
        _context.Recipes.Remove(recipe);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}