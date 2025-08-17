using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace RecipeApp.Domain.Entities;

public class Recipe
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Range(1, 5)]
    public int Difficulty { get; set; }
    [Required]
    public Guid AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public virtual User Author { get; set; } = null!;
    public string? ThumbnailImagePath { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<FavoriteRecipe> FavoritedByUsers { get; set; } = new List<FavoriteRecipe>();
}