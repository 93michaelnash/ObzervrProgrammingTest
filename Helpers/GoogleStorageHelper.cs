using Google.Cloud.Storage.V1;
using System.IO;
using System.Reflection;

namespace ObzervrProgrammingTest.Helpers
{
    public static class GoogleStorageHelper
    {
        public static bool FileExists(string bucket, string fileName)
        {
            var client = StorageClient.Create();

            try
            {
                client.GetObject(bucket, fileName);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
