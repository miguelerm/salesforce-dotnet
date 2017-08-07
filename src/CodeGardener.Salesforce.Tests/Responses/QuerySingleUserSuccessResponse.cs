using System.Net;
using System.Net.Http;
using System.Text;

namespace CodeGardener.Salesforce.Tests.Responses
{
    sealed class QuerySingleUserSuccessResponse : HttpResponseMessage
    {
        public QuerySingleUserSuccessResponse()
        {
            StatusCode = HttpStatusCode.OK;
            var responseContent = TestTools.GetResourceAsString("CodeGardener.Salesforce.Tests.Responses.QuerySingleUserSuccessResponse.xml");
            Content = new StringContent(responseContent, Encoding.UTF8, "application/xml");
        }
    }
}
