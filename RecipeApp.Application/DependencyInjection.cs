using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Application.Common.Mapping;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Services;
using RecipeApp.Application.Services.Auth;

namespace RecipeApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Mappers
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly()); 
            services.AddSingleton(typeAdapterConfig);

            // Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Auth
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            // Services
            services.AddScoped<IRecipeService, RecipeService>();

            return services;
        }
    }
}