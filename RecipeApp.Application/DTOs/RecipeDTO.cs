namespace RecipeApp.Application.DTOs;

public class RecipeDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid AuthorId { get; set; }
    public int FavoritesCount { get; set; }
}