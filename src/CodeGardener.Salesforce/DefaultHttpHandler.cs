using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGardener.Salesforce
{
    public class DefaultHttpHandler : IHttpHandler
    {
        private readonly HttpClient client;

        public DefaultHttpHandler(HttpClient client)
        {
            this.client = client;
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return client.PostAsync(requestUri, content);
        }
    }
}
