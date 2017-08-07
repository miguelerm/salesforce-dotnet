using CodeGardener.Salesforce.HttpContents;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

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
            else
            {
                throw new Exception($"HTTP Error: {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string queryString) where T : class, new()
        {
            var content = new QueryHttpContent(SessionId, queryString);

            var response = await http.PostAsync(ServerUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var xml = await response.Content.ReadAsStringAsync();
                return DeserializeQueryResponse<T>(xml);
            }
            else
            {
                throw new Exception($"HTTP Error: {response.StatusCode}");
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

        private IEnumerable<T> DeserializeQueryResponse<T>(string xml) where T : class, new()
        {
            return new T[] { };
        }
    }
}
