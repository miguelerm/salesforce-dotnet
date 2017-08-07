using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace CodeGardener.Salesforce.Tests
{
    static class TestTools
    {
        public static string GetResourceAsString(string resourceName)
        {
            var assembly = typeof(TestTools).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
