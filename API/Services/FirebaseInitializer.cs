using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace API.Services;

public class FirebaseInitializer
{
    public void Initialize()
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("Services/test-deploy-d038d-firebase-adminsdk-ag34n-78b7bc5615.json")
        });
    }
}
