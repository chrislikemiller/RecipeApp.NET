using FluentValidation;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Validators;

public class UpdateRecipeDTOValidator : AbstractValidator<UpdateRecipeDTO>
{
    public UpdateRecipeDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Recipe name is required.")
            .MaximumLength(150)
            .WithMessage("Recipe name must not exceed 150 characters.");

        RuleFor(x => x.Instructions)
            .NotEmpty()
            .WithMessage("Instructions are required.");

        RuleFor(x => x.Difficulty)
            .InclusiveBetween(1, 5)
            .WithMessage("Difficulty must be between 1 and 5.");
    }
}