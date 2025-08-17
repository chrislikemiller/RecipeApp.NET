using Mapster;
using RecipeApp.Application.DTOs;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Application.Common.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Auth
        config.NewConfig<RegisterUserDTO, User>()
            .Map(dest => dest.UserName, src => src.Name)
            .Map(dest => dest.Email, src => src.Email);

        // Recipes
        config.NewConfig<Recipe, RecipeDTO>()
            .Map(dest => dest.FavoritesCount, src => src.FavoritedByUsers.Count);
        config.NewConfig<UpdateRecipeDTO, Recipe>()
            .Map(dest => dest.UpdatedAtUtc, src => DateTime.UtcNow);
    }
}