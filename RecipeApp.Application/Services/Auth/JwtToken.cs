using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Application.Services.Auth;

public class JwtToken : IToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } 
}

