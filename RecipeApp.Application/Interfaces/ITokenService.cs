using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.Application.Services.Auth;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Application.Interfaces;

public interface ITokenService
{
    IToken GenerateToken(User user);
    DateTime GetTokenExpiration();
}
