using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Services.Auth;
public class AuthResponse
{
    public static AuthResponseDTO RegisterFailed(string message = "User with this email already exists.")
    {
        return new RegisterAuthResponseDTO
        {
            Success = false,
            Error = $"Registration failed! {message}"
        };
    }

    public static AuthResponseDTO RegisterSucceeded()
    {
        return new RegisterAuthResponseDTO
        {
            Success = true,
        };
    }

    public static LoginAuthResponseDTO LoginSucceeded(Guid id, string name, IToken token)
    {
        return new LoginAuthResponseDTO
        {
            Success = true,
            Token = token,
            Id = id,
            Name = name,
        };
    }

    public static LoginAuthResponseDTO LoginFailed(string message = "Invalid email or password.")
    {
        return new LoginAuthResponseDTO
        {
            Success = false,
            Token = null,
            Error = $"Login failed. {message}"
        };
    }

    internal static AuthResponseDTO LogoutSucceeded()
    {
        return new LoginAuthResponseDTO
        {
            Success = true,
            Token = null,
        };
    }
}

