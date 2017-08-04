using System.Net.Http;
using System.Text;
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

        public async Task LoginAsync(string username, string password, string token)
        {
            var content = GetLoginXmlContent(username, password + token);

            var response = await http.PostAsync(LoginUrl, new StringContent(content, Encoding.UTF8, "application/xml"));

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

        private static string GetLoginXmlContent(string username, string password)
        {
            return "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<login xmlns=\"urn:enterprise.soap.sforce.com\">" +
                        "<username>" + username + "</username>" +
                        "<password>" + password + "</password>" +
                    "</login>" +
                "</s:Body>" +
            "</s:Envelope>";
        }
    }
}
