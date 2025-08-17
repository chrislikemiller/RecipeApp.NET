namespace RecipeApp.Application.DTOs;

public class CreateRecipeDTO
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public string[] Ingredients { get; set; } = Array.Empty<string>();
    public string[] Instructions { get; set; } = Array.Empty<string>();
}