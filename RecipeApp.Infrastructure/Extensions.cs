using System.Linq.Expressions;
using RecipeApp.Application.Models;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Infrastructure;

public static class Extensions
{
    public static IQueryable<T> WithFilters<T>(this IQueryable<T> query, RecipeQueryOptions queryOptions) where T : Recipe
    {
        return query
            .WhereIf(queryOptions.UserId != Guid.Empty,
                x => x.AuthorId == queryOptions.UserId)
            .WithBasicFilters(queryOptions);
    }

    public static IQueryable<T> WithBasicFilters<T>(this IQueryable<T> query, RecipeQueryOptions queryOptions) where T : Recipe
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(queryOptions.SearchTerm),
                x => x.Title.ToLower().Contains(queryOptions.SearchTerm!.ToLower()) ||
                     x.Description.ToLower().Contains(queryOptions.SearchTerm!.ToLower()) ||
                     x.RecipeIngredients.Any(i => i.Ingredient.Name.ToLower().Contains(queryOptions.SearchTerm!.ToLower())) ||
                     x.Tags.Any(t => t.Name.ToLower().Contains(queryOptions.SearchTerm!.ToLower())))
            .WhereIf(queryOptions.Difficulty.HasValue,
                x => x.Difficulty == queryOptions.Difficulty!.Value)
            .SortBy(queryOptions.SortType, queryOptions.SortDirection);
    }

    // Remove or keep this method for client-side evaluation if needed
    public static bool MatchesSearchTerm(this Recipe recipe, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return true;
        var lowerSearchTerm = searchTerm.ToLowerInvariant();
        return recipe.Title.ToLowerInvariant().Contains(lowerSearchTerm) ||
               recipe.Description.ToLowerInvariant().Contains(lowerSearchTerm) ||
               recipe.RecipeIngredients.Any(i => i.Ingredient.Name.ToLowerInvariant().Contains(lowerSearchTerm)) ||
               recipe.Tags.Any(t => t.Name.ToLowerInvariant().Contains(lowerSearchTerm));
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (condition)
            return source.Where(predicate);
        else
            return source;
    }

    public static IQueryable<TSource> SortBy<TSource>(
        this IQueryable<TSource> source,
        RecipeSortType sortType,
        SortDirection sortDirection) where TSource : Recipe
    {
        var isAscending = sortDirection == SortDirection.Ascending;
        return sortType switch
        {
            RecipeSortType.Title => isAscending
                ? source.OrderBy(r => r.Title)
                : source.OrderByDescending(r => r.Title),
            RecipeSortType.Difficulty => isAscending
                ? source.OrderBy(r => r.Difficulty)
                : source.OrderByDescending(r => r.Difficulty),
            RecipeSortType.Date => isAscending
                ? source.OrderBy(r => r.UpdatedAtUtc ?? r.CreatedAtUtc)
                : source.OrderByDescending(r => r.UpdatedAtUtc ?? r.CreatedAtUtc),
            _ => source
        };
    }
}

