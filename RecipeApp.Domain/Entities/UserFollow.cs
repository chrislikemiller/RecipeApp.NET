using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApp.Domain.Entities;

public class UserFollow
{
    [Required]
    public Guid FollowerId { get; set; }

    [ForeignKey(nameof(FollowerId))]
    public virtual User Follower { get; set; } = null!;

    [Required]
    public Guid FollowingId { get; set; }

    [ForeignKey(nameof(FollowingId))]
    public virtual User Following { get; set; } = null!;
}