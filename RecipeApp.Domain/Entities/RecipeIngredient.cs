using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApp.Domain.Entities;

public class RecipeIngredient
{
    [Required]
    public Guid RecipeId { get; set; }

    [ForeignKey(nameof(RecipeId))]
    public virtual Recipe Recipe { get; set; } = null!;

    [Required]
    public Guid IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Quantity { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Measurement { get; set; }
}