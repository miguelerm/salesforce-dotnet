using System.Net.Http;
using System.Text;

namespace CodeGardener.Salesforce.HttpContents
{
    sealed class QueryHttpContent : StringContent
    {
        public QueryHttpContent(string sessionId, string queryString) : base(GetQueryXmlContent(sessionId, queryString), Encoding.UTF8, "application/xml")
        {

        }

        private static string GetQueryXmlContent(string sessionId, string queryString)
        {
            return "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<s:Header>" +
                    "<h:SessionHeader xmlns:h=\"urn:enterprise.soap.sforce.com\" xmlns=\"urn:enterprise.soap.sforce.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                        "<sessionId>" + sessionId + "</sessionId>" +
                    "</h:SessionHeader>" +
                "</s:Header>" +
                "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<query xmlns=\"urn:enterprise.soap.sforce.com\">" +
                        "<queryString>" + queryString + "</queryString>" +
                    "</query>" +
                "</s:Body>" +
            "</s:Envelope>";
        }
    }
}
