using System.Net.Http;
using System.Text;

namespace CodeGardener.Salesforce.HttpContents
{
    sealed class LoginHttpContent : StringContent
    {
        public LoginHttpContent(string username, string password, string token) 
            : base(GetLoginXmlContent(username, password, token), Encoding.UTF8, "application/xml")
        {

        }

        private static string GetLoginXmlContent(string username, string password, string token)
        {
            return "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<login xmlns=\"urn:enterprise.soap.sforce.com\">" +
                        "<username>" + username + "</username>" +
                        "<password>" + password + token + "</password>" +
                    "</login>" +
                "</s:Body>" +
            "</s:Envelope>";
        }
    }
}
