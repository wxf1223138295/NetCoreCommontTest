using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XiaoWeiTask
{
    public class HttpHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<string> HttpGet(string url, string data)
        {
            return await HttpRequest(url, data, "GET");
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, string data)
        {
            return  await HttpRequest(url, data, "POST");
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task<string> HttpRequest(string url, string data, string type)
        {
            HttpWebRequest httpWebRequest = null;
            StreamReader streamReader = null;
            WebResponse httpWebResponse = null;

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(data));
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                    httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                }
                httpWebRequest.ContentType = $"multipart/form-data;boundary=AaB03x";
                httpWebRequest.Accept = "text/plain";
                httpWebRequest.Method = type;
                httpWebRequest.Timeout = 5000000;
                AddRequestData(httpWebRequest, data);
                var temp =await httpWebRequest.GetResponseAsync();
                httpWebResponse = temp;
                streamReader = new StreamReader(temp.GetResponseStream(), Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (streamReader != null) streamReader.Close();
                if (httpWebResponse != null) httpWebResponse.Close();
                if (httpWebRequest != null) httpWebRequest.Abort();
            }
        }

        /// <summary>
        /// 增加请求参数
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="data"></param>
        private static void AddRequestData(HttpWebRequest httpWebRequest, string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            byte[] bs = Encoding.UTF8.GetBytes(data);
            httpWebRequest.ContentLength = bs.Length;
            httpWebRequest.GetRequestStream().Write(bs, 0, bs.Length);
        }
    }
}
