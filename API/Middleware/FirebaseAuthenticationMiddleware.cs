using API.Services;

namespace API.Middleware;

public class FirebaseAuthenticationMiddleware(RequestDelegate next, FirebaseAuthenticationService firebaseAuthService)
{
    private readonly RequestDelegate _next = next;
    private readonly FirebaseAuthenticationService _firebaseAuthService = firebaseAuthService;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization header missing");
            return;
        }

        var token = authorizationHeader.ToString().Split(" ").Last();

        if (string.IsNullOrEmpty(token) || !await _firebaseAuthService.VerifyIdToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        await _next(context);
    }
}