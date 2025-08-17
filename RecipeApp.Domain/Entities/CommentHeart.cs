using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApp.Domain.Entities;

public class CommentHeart
{
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [Required]
    public Guid CommentId { get; set; }

    [ForeignKey(nameof(CommentId))]
    public virtual Comment Comment { get; set; } = null!;
}