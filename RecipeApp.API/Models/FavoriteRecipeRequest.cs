namespace RecipeApp.API.Models;

public class FavoriteRecipeRequest
{
    public Guid UserId { get; set; }
    public Guid RecipeId { get; set; }
}
