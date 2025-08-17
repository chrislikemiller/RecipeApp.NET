using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace RecipeApp.Domain.Entities;
public class User : IdentityUser<Guid>
{
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();
    public virtual ICollection<CommentHeart> CommentHearts { get; set; } = new List<CommentHeart>();

    public virtual ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public virtual ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
}