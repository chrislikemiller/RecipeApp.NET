using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RecipeApp.API.Extensions;

public static class ModelStateExtensions
{
    public static Dictionary<string, string[]> GetModelError(this ModelStateDictionary modelState)
    {
        return modelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(x => x.Key, e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
