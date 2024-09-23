using FirebaseAdmin.Auth;

namespace API.Services;

public class FirebaseAuthenticationService
{
    public async Task<string?> VerifyIdToken(string idToken)
    {
        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;
        }
        catch (FirebaseAuthException)
        {
            // Token is invalid
            return null;
        }
    }
}