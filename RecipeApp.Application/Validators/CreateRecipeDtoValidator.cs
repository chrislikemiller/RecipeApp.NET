using FluentValidation;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Validators;

public class CreateRecipeDTOValidator : AbstractValidator<CreateRecipeDTO>
{
    public CreateRecipeDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Recipe title is required.")
            .MaximumLength(150)
            .WithMessage("Recipe titlemust not exceed 150 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.Difficulty)
            .InclusiveBetween(1, 5)
            .WithMessage("Difficulty must be between 1 and 5.");
    }
}