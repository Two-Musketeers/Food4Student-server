using FirebaseAdmin.Auth;

namespace API.Services;

public class FirebaseAuthenticationService
{
    public async Task<bool> VerifyIdToken(string idToken)
    {
        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            string uid = decodedToken.Uid;
            // Token is valid
            return true;
        }
        catch (FirebaseAuthException)
        {
            // Token is invalid
            return false;
        }
    }
}