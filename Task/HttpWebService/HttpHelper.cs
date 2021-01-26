using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HttpWebService
{
    public class HttpHelper
    {

        /// <summary>
        /// 封装使用HttpClient调用WebService
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="content">参数</param>
        /// <returns></returns>
        private async Task<string> PostHelper(string url, HttpContent content)
        {
            var result = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    using (var response = await client.PostAsync("", content))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            result = await response.Content.ReadAsStringAsync();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(result);
                            result = doc.InnerText;
                        }
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
