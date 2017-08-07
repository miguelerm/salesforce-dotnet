using CodeGardener.Salesforce.Models;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using System.Net.Http;
using CodeGardener.Salesforce.Tests.Responses;

namespace CodeGardener.Salesforce.Tests.SoapClientTests
{
    [Trait("SoapClient", "When querying an user by Id")]
    public class WhenQueryingAnUserById
    {
        private readonly Mock<IHttpHandler> http;
        private readonly SoapClient client;
        private readonly IEnumerable<User> result;

        public WhenQueryingAnUserById()
        {
            http = CreateHttpMock();
            client = new SoapClient(http.Object);
            client.LoginAsync(username: "user", password: "p@ss", token: "t0k3n").Wait();
            result = client.QueryAsync<User>("SELECT Id, FirstName, LastName, Email FROM User where Id = 'user-id'").Result;
        }

        [Fact(DisplayName = "Deserialize results into an IEnumerable of Users")]
        public void DesearilizeResultsIntoAnIEnumerableOfUsers()
        {
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }

        [Fact(DisplayName = "Deserialize user XML elements into user properties")]
        public void DesearilizeUserXmlElementsIntoUserProperties()
        {
            var user = result.Single();
            Assert.Equal("user-id", user.Id);
            Assert.Equal("user-first-name", user.FirstName);
            Assert.Equal("user-last-name", user.LastName);
            Assert.Equal("user-email@email.com", user.Email);
        }

        [Fact(DisplayName = "Adds the Query to the SOAP Body")]
        public void AddsTheQueryToTheSoapBody()
        {
            http.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<HttpContent>(c => HasQueryInTheBody(c))), Times.Once());
        }

        [Fact(DisplayName = "Adds the Session Id to the SOAP Header")]
        public void AddsTheSessionIdToTheSoapHeader()
        {
            http.Verify(x => x.PostAsync(It.IsAny<string>(), It.Is<HttpContent>(c => HasSessionIdInHeader(c))), Times.Once());
        }

        [Fact(DisplayName = "Uses the server url to make the request")]
        public void UsesTheServerUrlToMakeTheRequest()
        {
            http.Verify(x => x.PostAsync("https://server-url", It.IsAny<HttpContent>()), Times.Once());
        }

        private static bool HasQueryInTheBody(HttpContent content)
        {
            var stringContent = content.ReadAsStringAsync().Result;
            return stringContent.Contains("<queryString>SELECT Id, FirstName, LastName, Email FROM User where Id = 'user-id'</queryString>");
        }

        private static bool HasSessionIdInHeader(HttpContent content)
        {
            var stringContent = content.ReadAsStringAsync().Result;
            return stringContent.Contains("<sessionId>ThisIsTheSessionId</sessionId>");
        }

        private static Mock<IHttpHandler> CreateHttpMock()
        {
            var mock = new Mock<IHttpHandler>();

            SetupFakeLogin(mock);
            SetupFakeQuery(mock);

            return mock;
        }

        private static void SetupFakeLogin(Mock<IHttpHandler> mock)
        {
            var loginUrl = "https://login.salesforce.com/services/Soap/c/40.0";
            var loginResponse = Task.FromResult<HttpResponseMessage>(new LoginSuccessResponse());
            mock.Setup(x => x.PostAsync(loginUrl, It.IsAny<HttpContent>())).Returns(() => loginResponse);
        }

        private static void SetupFakeQuery(Mock<IHttpHandler> mock)
        {
            var queryUrl = "https://server-url";
            var queryResponse = Task.FromResult<HttpResponseMessage>(new QuerySingleUserSuccessResponse());
            mock.Setup(x => x.PostAsync(queryUrl, It.IsAny<HttpContent>())).Returns(() => queryResponse);
        }
    }
}
