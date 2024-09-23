using API.Services;

namespace API.Middleware;

public class FirebaseAuthenticationMiddleware(RequestDelegate next, FirebaseAuthenticationService firebaseAuthService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization header missing");
            return;
        }

        var token = authorizationHeader.ToString().Split(" ").Last();

        var userId = await firebaseAuthService.VerifyIdToken(token);

        if (string.IsNullOrEmpty(userId))

        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        await next(context);
    }
}