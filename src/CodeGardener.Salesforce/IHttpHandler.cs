using System.Net.Http;
using System.Threading.Tasks;

namespace CodeGardener.Salesforce
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent message);
    }
}