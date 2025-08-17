using RecipeApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeApp.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
    public DbSet<CommentHeart> CommentHearts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Composite key for RecipeIngredient
        builder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        // Composite key for Rating
        builder.Entity<Rating>()
            .HasKey(r => new { r.UserId, r.RecipeId });

        // Composite key for FavoriteRecipe
        builder.Entity<FavoriteRecipe>()
            .HasKey(fr => new { fr.UserId, fr.RecipeId });

        // Composite key for CommentHeart
        builder.Entity<CommentHeart>()
            .HasKey(ch => new { ch.UserId, ch.CommentId });

        // Composite key and relationships for UserFollow
        builder.Entity<UserFollow>()
            .HasKey(uf => new { uf.FollowerId, uf.FollowingId });

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserFollow>()
            .HasOne(uf => uf.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index for Tag names
        builder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // Unique index for Ingredient names
        builder.Entity<Ingredient>()
            .HasIndex(i => i.Name)
            .IsUnique();
    }
}