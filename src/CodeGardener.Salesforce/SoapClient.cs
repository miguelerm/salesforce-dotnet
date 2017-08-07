using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGardener.Salesforce
{
    public class SoapClient
    {
        private const string LoginUrl = "https://login.salesforce.com/services/Soap/c/40.0";
        private readonly IHttpHandler http;

        public string ServerUrl { get; private set; }
        public string SessionId { get; private set; }

        public SoapClient(IHttpHandler http)
        {
            this.http = http;
        }

        public SoapClient(HttpClient client) : this(new DefaultHttpHandler(client))
        {

        }

        public async Task LoginAsync(string username, string password, string token)
        {
            var content = new LoginHttpContent(username, password, token);

            var response = await http.PostAsync(LoginUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var xml = await response.Content.ReadAsStringAsync();
                var result = DeserializeLoginResponse(xml);
                ServerUrl = result.ServerUrl;
                SessionId = result.SessionId;
            }
        }

        private static LoginResponse DeserializeLoginResponse(string xmlContent)
        {
            var regex = new Regex("<serverUrl>(?<serverUrl>.*)</serverUrl>\\s*<sessionId>(?<sessionId>.*)</sessionId>");
            var match = regex.Match(xmlContent);

            return new LoginResponse
            {
                ServerUrl = match.Groups["serverUrl"].Value,
                SessionId = match.Groups["sessionId"].Value
            };
        }

        
    }
}
