using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Reflection;

namespace ObzervrProgrammingTest.Helpers
{
    public static class GoogleCredentialHelper
    {
        public static GoogleCredential GetGoogleCredentials()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"google-cloud-sa.json");
            return GoogleCredential.FromFile(path);
        }
    }
}
