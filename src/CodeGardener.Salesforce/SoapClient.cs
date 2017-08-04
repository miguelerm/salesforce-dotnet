using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodeGardener.Salesforce
{
    public class SoapClient
    {
        private readonly IHttpHandler http;

        public SoapClient(IHttpHandler http)
        {
            this.http = http;
        }

        public Task LoginAsync(string username, string password, string token)
        {
            var content = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                              "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                  "<login xmlns=\"urn:enterprise.soap.sforce.com\">" +
                                      "<username>" + username + "</username>" +
                                      "<password>" + password + token + "</password>" +
                                  "</login>" +
                              "</s:Body>" +
                          "</s:Envelope>";

            return http.PostAsync("", new StringContent(content, Encoding.UTF8, "application/xml"));
        }
    }
}
