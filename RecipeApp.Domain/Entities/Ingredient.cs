using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Domain.Entities;

public class Ingredient
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // navigation property
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}