using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace RUINORERP.WebServerConsole.Comm
{
    public class HttpHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="obj">请求参数：dic or model></param>
        /// <param name="headers">自定义请求头</param>
        /// <returns>PacketResult</returns>
        public static PacketResult Post(string url, object obj, Dictionary<string, string> headers)
        {
            var result = new PacketResult();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestVersion = HttpVersion.Version11;

                if (headers != null && headers.Count > 0)
                {
                    foreach (var dr in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(dr.Key, dr.Value);
                    }
                }

                StringContent content = new(JsonHelper.ToJson(obj), Encoding.UTF8, "application/json");
                using var response = httpClient.PostAsync(url, content).Result;

                result.Flag = true;
                result.Data = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex) { result.Msg = "请求异常，" + ex.Message; }
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">请求参数</param>
        /// <returns>PacketResult</returns>
        public static PacketResult Get(string url, Dictionary<string, string> dic)
        {
            var result = new PacketResult();
            try
            {
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Accept.Clear();

                var postUrl = url + "?" + string.Join("&", dic.Select(d => string.Format("{0}={1}", d.Key, d.Value)));

                using HttpResponseMessage response = httpClient.GetAsync(postUrl).Result;
                result.Flag = true;
                result.Data = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                result.Msg = "请求异常，" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static PacketResult PostForm(string url, Dictionary<string, string> dic)
        {
            var result = new PacketResult();
            try
            {
                var postData = "";
                if (dic != null && dic.Count > 0)
                {
                    postData = string.Join("&", dic.Keys.Select(key => $"{key}={HttpUtility.UrlEncode(dic[key], Encoding.GetEncoding("utf-8"))}"));
                }

                using HttpClient client = new();
                using HttpContent httpContent = new StringContent(postData, Encoding.UTF8);
                httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                var response = client.PostAsync(url, httpContent).Result;
                result.Flag = true;
                result.Data = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                result.Msg = "请求异常 ，" + e.Message;
            }
            return result;
        }

    }
}