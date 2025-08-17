using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Domain.Entities;

public class Tag
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}