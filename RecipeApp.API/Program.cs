using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RecipeApp.API.Middlewares;
using RecipeApp.Application;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Entities;
using RecipeApp.Infrastructure;
using RecipeApp.Infrastructure.Persistence;
using RecipeApp.Infrastructure.Repositories;

namespace RecipeApp.API;

// next steps:
// add logger everywhere
// separate models between layers so DB layer can be independent
// add routing
// middleware (task 4)
// create a middleware that only runs on one endpoint, but not the other
// generate swagger documentation, add examples with attributes?
// - look into operation processors

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        InitializeServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowAngularDev");
            app.MapOpenApi();

            using var scope = app.Services.CreateScope();
            var dummyDataInitializer = scope
                .ServiceProvider
                .GetService<DummyDataDbContextInitializer>();
            if (dummyDataInitializer != null)
            {
                await dummyDataInitializer.InitializeAsync();
                await dummyDataInitializer.SeedAsync();
            }
        }

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<JwtDebuggingMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void InitializeServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();

        services.AddControllers();
        services.AddOpenApi();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();


        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });


        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularDev", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

    }
}
