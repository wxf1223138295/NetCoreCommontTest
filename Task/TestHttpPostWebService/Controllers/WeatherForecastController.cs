using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TestHttpPostWebService.Controllers
{
    public class CardInfo
    {
        /// <summary>
        /// 
        /// </summary>
  
        public int PatientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CardNo { get; set; }

        public DateTime ReportTime { get; set; }

        public decimal NumValue { get; set; }
        public string UnitName { get; set; }

        public string ItemName { get; set; }
    }
    public class SendTemplateMsg2Input
    {
        public string Snid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string StrTemplateNo { get; set; }
        public string StrXml { get; set; }
        public string IsSendWX { get; set; }
        public string IsSendAL { get; set; }
        public string Digest { get; set; }
        public string WebServiceUrl { get; set; }
        public string ActionUrl { get; set; }
    }
    [XmlRoot("WSRsp")]
    public class WSResponse
    {
        [XmlElement("SNID")]
        public string Snid { get; set; }
        [XmlElement("resInfo")]
        public string ResInfo { get; set; }
        [XmlElement("extendInfo")]

        public string ExtendInfo { get; set; }
        [XmlElement("digest")]

        public string Digest { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOperateSoap _operateSoap;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IOperateSoap operateSoap)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _operateSoap = operateSoap;
        }

        private string Md5(string result)
        {
            using (var md5 = MD5.Create())
            {
                var streamstr = result;
                var inputBytes = Encoding.Default.GetBytes(streamstr);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private static string ReadContext(string file)
        {
            byte[] array = Encoding.Default.GetBytes(file);
            MemoryStream ms = new MemoryStream(array);
            StreamReader sr = new StreamReader(ms, System.Text.Encoding.UTF8);

            string context = sr.ReadToEnd();
            ms.Close();
            sr.Close();
            sr.Dispose();
            ms.Dispose();

            return context;
        }
        [HttpGet("Get3")]
        public async Task<string> Get3()
        {
            try
            {


                //调用微信接口发送消息

                string userName = "tnbapp";
                string userpass = "U0MkY3IiB0aXR";
                string webserviceurl = "http://194.1.10.43:8045/Common/WebServiceForMessage.asmx";
                string strSalt = userpass;
                string _SNID = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                //string _templateNo = this.txtTemplateNo.Text.Trim();//测试数据为"YYTZ"(请向开发人员索取)
                //string _strXML = this.txtXML.Text.Trim();//测试数据为"<xml><USERID>{CardInfo}</USERID><URL></URL><FIRST>这是一则普通通知</FIRST><TZSJ>2018-7-3 18:42</TZSJ><TZNR>多喝水</TZNR><REMARK>祝您身体健康！</REMARK></xml>"(不要有回车)(请向开发人员索取)
                string _templateNo = "YYTZ";
                string _strXML = "<xml><USERID>{CardInfo}</USERID><URL></URL><FIRST></FIRST><TZSJ>{CurrentTime}</TZSJ><TZNR>尊敬的用户{PatientName}，监测到您的血糖异常（检查时间：{CheckTime}，{ItemName}值{NumValue}{UnitName}），推荐您打开下方工具栏“患者通道”，使用“糖管佳”小程序，参与糖尿病防诊治项目，连接医生,掌控健康</TZNR><REMARK></REMARK></xml>";


                CardInfo card=new CardInfo();
                card.CardNo = "0008775227";
                card.ItemName = "血糖";
                card.NumValue = 5.43M;
                card.PatientId = 414523;
                card.PatientName = "李涛";
                card.UnitName = "mmol/L";
                card.ReportTime = Convert.ToDateTime("2021-01-01 19:07:00");
                //替换时间
                var timestr = string.Empty;
                if (card?.ReportTime != null)
                {
                    var reporttime = Convert.ToDateTime(card.ReportTime);

                    var yearstr = reporttime.Year;
                    var monthstr = reporttime.Month;
                    var daystr = reporttime.Day;
                    var sb = new StringBuilder();
                    sb.Append(yearstr.ToString());
                    sb.Append("年");
                    sb.Append(monthstr.ToString());
                    sb.Append("月");
                    sb.Append(daystr.ToString());
                    sb.Append("日");

                    timestr = sb.ToString();
                }

                _strXML = _strXML.Replace("{CardInfo}", card?.CardNo.ToString())
                    .Replace("{CheckTime}", timestr)
                    .Replace("{NumValue}", card.NumValue.ToString())
                    .Replace("{UnitName}", card?.UnitName)
                    .Replace("{PatientName}", card?.PatientName)
                    .Replace("{CurrentTime}", DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm"))
                    .Replace("{ItemName}", card?.ItemName);

                Console.WriteLine($"xml:{_strXML}");

                string _isSendWX = "1";
                string _isSendAL = "0";

                //string _digest = Md5Encrypt.CreateMd5Code(_SNID + userName + userpass + _templateNo + _strXML + _isSendWX + _isSendAL, strSalt);
                var tt=Md5(_SNID + userName + userpass + _templateNo + _strXML + _isSendWX + _isSendAL);
                string digest = Md5(tt + strSalt);


                //WindowsApplication1.WSMSG.WebServiceForMessage ws = new WindowsApplication1.WSMSG.WebServiceForMessage();
                //string res = ws.SendTemplateMsg2(_SNID, userName, userpass, _templateNo, _strXML, _isSendWX, _isSendAL, digest);
                //context.LogInformation($"微信推送结果为"+ res);

                //*******************************************************************************************************************************
                //2020 12 25 
                //添加httpclient调用soap
                //*******************************************************************************************************************************
                SendTemplateMsg2Input input = new SendTemplateMsg2Input
                {
                    Snid = _SNID,
                    ActionUrl = "http://tempuri.org/SendTemplateMsg2",
                    Digest = digest,
                    IsSendAL = "0",
                    IsSendWX = "1",
                    Password = userpass,
                    StrTemplateNo = _templateNo,
                    StrXml = _strXML,
                    UserName = userName,
                    WebServiceUrl = webserviceurl
                };
                var result = _operateSoap.CreateSoap11EnvelopeForSendTemplateMsg2Async(input).ConfigureAwait(false)
                    .GetAwaiter().GetResult();

                if (result.success)
                {
                    Console.WriteLine($"微信推送结果为" + result?.message);
                }
                else
                {
                    Console.WriteLine($"微信推送失败" + result?.exceptionMsg);
                }



                // 更新这一批的最大报告时间


                return JsonConvert.SerializeObject(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"发生异常：{e.Message} 调用堆栈：{e.StackTrace}");
            }

            return "";
        }

        [HttpGet("Get2")]
        public async Task<string> Get2()
        {
            string strResult = "";
            try
            {

                var weburl = $"{_configuration["url"]}";
                string snid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                var templateNo = "YYTZ";
                // var strxml = $"{_configuration["testxml"]}";



                //var strxml = new XElement("xml",
                //    new XElement("USERID", "0008775227"),
                //    new XElement("URL", ""),
                //    new XElement("FIRST", "这是一则普通通知"),
                //    new XElement("TZSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                //    new XElement("TZNR", "多喝水"),
                //    new XElement("REMARK", "祝您身体健康")
                //);

                var strxml =
                    "<xml><USERID>{CardInfo}</USERID><URL></URL><FIRST></FIRST><TZSJ>{CurrentTime}</TZSJ><TZNR>尊敬的用户{PatientName}，监测到您的血糖异常（检查时间：324324324，血糖值12334），推荐您打开下方工具栏“患者通道”，使用“糖管佳”小程序，参与糖尿病防诊治项目，连接医生,掌控健康</TZNR><REMARK></REMARK></xml>";

                strxml = strxml.Replace("{CardInfo}", "0008775227")
                    .Replace("{PatientName}", "李涛")
                    .Replace("{CurrentTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                //var sdsd=new XDocument(new XDeclaration("1.0", "UTF-8", "no"),
                //    new XElement("xml",
                //        new XElement("USERID", "0008775227"),
                //        new XElement("URL", ""),
                //        new XElement("FIRST", "这是一则普通通知"),
                //        new XElement("TZSJ", DateTime.Now.ToString("yyyy-MM-DD HH:mm")),
                //        new XElement("TZNR", "多喝水"),
                //        new XElement("REMARK", "祝您身体健康")
                //    )
                //    );


                //string l_strResult = strxml.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                string l_strResult = strxml;

                //var xmdlsk = $"<xml><USERID>0008775227</USERID><URL></URL><FIRST>534234热热饭5345</FIRST><TZSJ>{DateTime.Now.ToString("yyyy-MM-DD HH:mm")}</TZSJ><TZNR>435为543</TZNR><REMARK>53453为5345</REMARK></xml>";

                //byte[] array = Encoding.Default.GetBytes(xmdlsk);

                //UTF8Encoding utf8 = new UTF8Encoding();
                //tesr = utf8.GetString(array);

                //strxml = strxml.Replace("{CardInfo}", "0008775227")
                //     .Replace("{PatientName}", "李涛")
                //     .Replace("{CurrentTime}", DateTime.Now.ToString("yyyy-MM-DD HH:mm"));



                var para1 = snid + "tnbapp" + "U0MkY3IiB0aXR" + templateNo + l_strResult + "1" + "0";
                Console.WriteLine(para1);
                var in1 = Md5(para1);

                var _digest = Md5(in1 + "U0MkY3IiB0aXR");

                var actionrul = "http://tempuri.org/SendTemplateMsg2";

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
                                new XElement(myns + "_SNID", snid),
                                new XElement(myns + "_userName", "tnbapp"),
                                new XElement(myns + "_pass", "U0MkY3IiB0aXR"),
                                new XElement(myns + "strTemplateNo", templateNo),
                                new XElement(myns + "strXml", l_strResult),
                                new XElement(myns + "isSendWX", "1"),
                                new XElement(myns + "isSendAL", "0"),
                                new XElement(myns + "_digest", _digest)
                            )
                        )
                    ));

                strResult = await PostHelper(weburl, soapRequest.ToString(), actionrul);

                Console.WriteLine($"%%%%%%%%%%%%%%%%%%%%%……………………………………………………：{strResult}");
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return strResult;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            try
            {



                var weburl = $"{_configuration["url"]}";
                string strResult = "";
                // 参数

                var snid = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                var username = _configuration["testuser"];
                var password = _configuration["password"];
                var key = _configuration["key"];
                var patientno = _configuration["patientno"];


                var para1 = snid + username + password + patientno;
                Console.WriteLine($"para1:{para1}");
                var in1 = Md5(snid + username + password + patientno);


                string _digest = Md5(in1 + key);

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
                            new XElement(myns + "GetUserBindInfo2",
                                               new XElement(myns + "_SNID", $"{snid}"),
                                               new XElement(myns + "_userName", $"{username}"),
                                               new XElement(myns + "_pass", $"{password}"),
                                               new XElement(myns + "strPatiNo", $"{patientno}"),
                                               new XElement(myns + "_digest", $"{_digest}")
                            )
                        )
                    ));

                strResult = await PostHelper(weburl, soapRequest.ToString(), "http://tempuri.org/GetUserBindInfo2");
                return strResult;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        /// <summary>
        /// 封装使用HttpClient调用WebService
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="content">参数</param>
        /// <returns></returns>
        private async Task<string> PostHelper(string url, string xml, string actionurl)
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


                    request.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
                    request.Headers.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    request.Content.Headers.ContentType.CharSet = "UTF-8";
                    request.Headers.Add("SOAPAction", actionurl);
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    result = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return result;

                    }
                    else
                    {
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
