using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestHttpPostWebService.Controllers;

namespace TestHttpPostWebService
{
    public class OperateSoap : IOperateSoap
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OperateSoap> _iLogger;

        public OperateSoap(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<OperateSoap> iLogger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _iLogger = iLogger;
        }
        public async Task<SynyiBizReturn<WSResponse>> CreateSoap11EnvelopeForSendTemplateMsg2Async(SendTemplateMsg2Input input)
        {

            XNamespace ns = "http://www.w3.org/2003/05/soap-envelope";
            XNamespace myns = "http://tempuri.org/";

            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

            XDocument soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(ns + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XAttribute(XNamespace.Xmlns + "soap", ns),
                    new XElement(ns + "Body",
                        new XElement(myns + "SendTemplateMsg2",
                            new XElement(myns + "_SNID", $"{input.Snid}"),
                            new XElement(myns + "_userName", $"{input.UserName}"),
                            new XElement(myns + "_pass", $"{input.Password}"),
                            new XElement(myns + "strTemplateNo", $"{input.StrTemplateNo}"),
                            new XElement(myns + "strXml", $"{input.StrXml}"),
                            new XElement(myns + "isSendWX", $"{input.IsSendWX}"),
                            new XElement(myns + "isSendAL", $"{input.IsSendAL}"),
                            new XElement(myns + "_digest", $"{input.Digest}")
                        )
                    )
                ));

            var response = await PostWebService(input.WebServiceUrl, soapRequest, input.ActionUrl);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {

                var bizreturn = new SynyiBizReturn<WSResponse>
                {
                    data = null,
                    message = result,
                    success = true
                };
                return bizreturn;
            }
            else
            {
                var bizreturn = new SynyiBizReturn<WSResponse>
                {
                    data = null,
                    exceptionMsg = result,
                    message = "error",
                    success = false
                };
                return bizreturn;
            }

        }

        private async Task<HttpResponseMessage> PostWebService(string url, XDocument xml, string actionurl)
        {
            var result = string.Empty;
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Post
                    };
                    request.Content = new StringContent(xml.ToString(), Encoding.UTF8, "text/xml");
                    request.Headers.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    request.Content.Headers.ContentType.CharSet = "UTF-8";
                    request.Headers.Add("SOAPAction", actionurl);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IOperateSoap
    {
        Task<SynyiBizReturn<WSResponse>> CreateSoap11EnvelopeForSendTemplateMsg2Async(SendTemplateMsg2Input input);

        Task<XDocument> CreateSoap12EnvelopeAsync()
        {
            throw new NoNullAllowedException();
        }
    }

    public class SynyiBizReturn<T>
    {
        public string message { get; set; }

        public T data { get; set; }
        public int resultCode { get; set; }
        public bool success { get; set; }
        public string exceptionMsg { get; set; }

    }
}
