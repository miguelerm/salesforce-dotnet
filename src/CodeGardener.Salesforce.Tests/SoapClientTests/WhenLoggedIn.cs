using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using Xunit;
using System.IO;
using System.Net;
using System.Text;

namespace CodeGardener.Salesforce.Tests.SoapClientTests
{
    [Trait("SoapClient", "When Logged In")]
    public class WhenLoggedIn
    {
        private LoggedInTestHttpHandler http;
        private SoapClient client;

        public WhenLoggedIn()
        {
            http = new LoggedInTestHttpHandler();
            client = new SoapClient(http);
            client.LoginAsync(username: "user", password: "p@ss", token: "t0k3n").Wait();
        }

        [Fact(DisplayName = "Makes an HTTP POST that contains User, Password and Token")]
        public void MakesAnHttpPostThatContainsUserPasswordAnd()
        {
            Assert.Contains("<username>user</username>", http.LastPost);
            Assert.Contains("<password>p@sst0k3n</password>", http.LastPost);
        }

        [Fact(DisplayName = "Sets the ServerUrl and SessionId")]
        public void SetsTheServerUrlAndSessionId()
        {
            Assert.Equal("https://server-url", client.ServerUrl);
            Assert.Equal("ThisIsTheSessionId", client.SessionId);
        }

        [Fact(DisplayName = "HTTP POST is sent only once")]
        public void HttpPostIsSentOnlyOnce()
        {
            Assert.Equal(1, http.PostCount);
        }

        [Fact(DisplayName = "Fixed login URL is used")]
        public void FixedLoginUrlIsUsed()
        {
            Assert.Equal("https://login.salesforce.com/services/Soap/c/40.0", http.LastPortUrl);
        }

        private class LoggedInTestHttpHandler: IHttpHandler
        {
            public int PostCount { get; private set; } = 0;
            public string LastPortUrl { get; private set; }
            public string LastPost { get; private set; }

            public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
            {
                PostCount++;
                LastPortUrl = url;
                LastPost = await content.ReadAsStringAsync();
                return new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(GetTestResponseContent(), Encoding.UTF8, "application/xml")
                };
            }

            private static string GetTestResponseContent()
            {
                var assembly = typeof(LoggedInTestHttpHandler).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream("CodeGardener.Salesforce.Tests.SoapClientTests.WhenLoggedInTestResponse.xml"))
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
