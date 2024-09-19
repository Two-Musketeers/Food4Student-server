using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class FirebaseAuthVerifyController(FirebaseAuthenticationService firebaseAuthService) : BaseApiController
{
    private readonly FirebaseAuthenticationService _firebaseAuthService = firebaseAuthService;

    [HttpPost("verify-token")]
    public async Task<IActionResult> VerifyToken([FromBody] IdTokenRequest request)
    {
        bool isValid = await _firebaseAuthService.VerifyIdToken(request.IdToken);
        if (isValid)
        {
            // Token is valid, proceed with your logic
            return Ok(new { message = "Token is valid" });
        }
        else
        {
            // Token is invalid
            return Unauthorized(new { message = "Invalid token" });
        }
    }
}

public class IdTokenRequest
{
    public required string IdToken { get; set; }
}