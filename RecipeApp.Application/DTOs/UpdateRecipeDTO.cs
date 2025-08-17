namespace RecipeApp.Application.DTOs;

public class UpdateRecipeDTO
{
    public string Name { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int Difficulty { get; set; }
}