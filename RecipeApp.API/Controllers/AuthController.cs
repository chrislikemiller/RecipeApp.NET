using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Application.DTOs;
using RecipeApp.Application.Interfaces;
using RecipeApp.Application.Services.Auth;
using RecipeApp.Domain.Entities;

namespace RecipeApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger; // todo
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDTO)
    {
        var authResult = await _authService.RegisterAsync(registerDTO);
        if (!authResult.Success)
        {
            return BadRequest(authResult);
        }
        return Ok(authResult);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginUserDTO loginDTO)
    {
        var authResult = await _authService.LoginAsync(loginDTO);
        if (!authResult.Success)
        {
            return Unauthorized(authResult);
        }
        return Ok(authResult);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var authResult = await _authService.LogoutAsync();
        if (!authResult.Success)
        {
            return BadRequest(authResult);
        }
        return Ok(authResult);
    }
}