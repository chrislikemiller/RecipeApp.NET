using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.Application.DTOs;

namespace RecipeApp.Application.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerDTO);
    Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginDTO);
    Task<AuthResponseDTO> LogoutAsync();
}

