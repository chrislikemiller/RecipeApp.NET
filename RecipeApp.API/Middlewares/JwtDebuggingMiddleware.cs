using Microsoft.Extensions.Primitives;

namespace RecipeApp.API.Middlewares;

public class JwtDebuggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtDebuggingMiddleware> _logger;

    public JwtDebuggingMiddleware(RequestDelegate next, ILogger<JwtDebuggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogAuthorizationDetails(context);

        await _next(context);
    }

    private void LogAuthorizationDetails(HttpContext context)
    {
        var hasAuthHeader = context.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValues);

        if (!hasAuthHeader)
        {
            _logger.LogInformation("No Authorization header found for path: {Path}", context.Request.Path);
            return;
        }

        var authHeader = authHeaderValues.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
        {
            _logger.LogInformation("Auth Header missing");
            return;
        }

        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Authorization header does not start with 'Bearer ': '{Header}'", authHeader);
            return;
        }
        var token = authHeader["Bearer ".Length..].Trim();
        _logger.LogInformation("Bearer Token Extracted - Length: {Length}", token.Length);

        if (!string.IsNullOrEmpty(token))
        {
            var dotCount = token.Count(c => c == '.');
            _logger.LogInformation("Token format: dots={DotCount}, valid?={IsValid}",
                dotCount, dotCount == 2);

            if (token.Length > 0)
            {
                _logger.LogInformation("Token sample: '{TokenSample}'",
                    token.Length > 100 ? token[..100] + "..." : token);
            }
        }
        else
        {
            _logger.LogWarning("Bearer token is empty");
        }
    }
}