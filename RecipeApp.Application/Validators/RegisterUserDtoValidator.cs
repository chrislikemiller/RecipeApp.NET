using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
            //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password);
    }
}
