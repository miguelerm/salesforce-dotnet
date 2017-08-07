using System.Net;
using System.Net.Http;
using System.Text;

namespace CodeGardener.Salesforce.Tests.Responses
{
    sealed class LoginSuccessResponse : HttpResponseMessage
    {
        public LoginSuccessResponse()
        {
            StatusCode = HttpStatusCode.OK;
            var responseContent = TestTools.GetResourceAsString("CodeGardener.Salesforce.Tests.Responses.LoginSuccessResponse.xml");
            Content = new StringContent(responseContent, Encoding.UTF8, "application/xml");
        }
    }
}
