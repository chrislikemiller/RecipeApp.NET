namespace RecipeApp.API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} started");
        var originalBodyStream = context.Response.Body;
        
        try
        {
            var startTime = DateTime.UtcNow;
            await _next(context);
            var elapsedMs = (DateTime.UtcNow - startTime).TotalMilliseconds;
            
            _logger.LogInformation(
                $"Request {context.Request.Method} {context.Request.Path} completed with status code {context.Response.StatusCode} in {elapsedMs}ms");
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}
