using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.Application.Services.Auth;

namespace RecipeApp.Application.DTOs;

public abstract class AuthResponseDTO
{
    public bool Success { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string? Error { get; set; }

}
public class RegisterAuthResponseDTO : AuthResponseDTO
{
}

public class LoginAuthResponseDTO : AuthResponseDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public IToken? Token { get; set; }
}

