using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace API.Middleware;

public class FirebaseAuthenticationMiddleware(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)

{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return AuthenticateResult.Fail("Authorization header missing");
        }

        var token = authorizationHeader.ToString().Split(" ").Last();

        if (string.IsNullOrEmpty(token)) return AuthenticateResult.Fail("There's no token");

        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

        if (decodedToken == null) return AuthenticateResult.Fail("Invalid token");

        if (string.IsNullOrEmpty(decodedToken.Uid)) return AuthenticateResult.Fail("Invalid token");

        var role = decodedToken.Claims.ContainsKey("role") ? decodedToken.Claims["role"].ToString() : "NotRegistered";

        // Add the userId to the HttpContext.User claims
        var claims = new[] { 
            new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid), 
            new Claim(ClaimTypes.Role, role!)
        };
        var identity = new ClaimsIdentity(claims, "Firebase");
        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
        
}