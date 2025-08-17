using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Application.Services.Auth;
public interface IToken
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}

