using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using RecipeApp.Application.DTOs;
using RecipeApp.Application.Interfaces;
using RecipeApp.Domain.Entities;

namespace RecipeApp.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user is null)
        {
            return AuthResponse.LoginFailed();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
        if (!result.Succeeded)
        {
            return AuthResponse.LoginFailed();
        }
        var token = _tokenService.GenerateToken(user);
        return AuthResponse.LoginSucceeded(user.Id, user.UserName!, token);
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerDTO)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (existingUser != null)
        {
            return AuthResponse.RegisterFailed();
        }

        var user = registerDTO.Adapt<User>();
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
        {
            return AuthResponse.RegisterFailed(string.Join(',',
                    result.Errors.Select(x => $"[{x.Code}] {x.Description}")));
        }

        await _userManager.AddToRoleAsync(user, "User");
        return AuthResponse.RegisterSucceeded();
    }

    public async Task<AuthResponseDTO> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return AuthResponse.LogoutSucceeded();
    }
}
