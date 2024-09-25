using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using NPOI.SS.Formula.Functions;
using NSoup.Helper;
using RUINORERP.Business;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.UI.SysConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 一个用于访问HttpWeb服务的类
    /// </summary>
    public class HttpWebService
    {


        public RUINORERP.Model.Context.ApplicationContext _appContext;
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;
        public ILogger<HttpWebService> _logger;
        public ConfigManager _configManager;

        public HttpWebService(ILogger<HttpWebService> logger, IUnitOfWorkManage unitOfWorkManage,
          ConfigManager configManager, RUINORERP.Model.Context.ApplicationContext appContext = null)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _appContext = appContext;
            _configManager = configManager;


        }

        /// <summary>
        /// 登陆webserver
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginUrl"></param>
        /// <returns></returns>
        public async Task Login(string username, string password, string loginUrl = "http://yourserver.com/login")
        {
            bool result = false;
            // 检查用户名和密码
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //return result;
            }

            // 发送登录请求到服务端
            var response = await SendLoginRequest(username, password, loginUrl);

            if (response.IsSuccessStatusCode)
            {
                // 获取服务端设置的Cookie
                // 尝试从响应头中获取 Cookie
                IEnumerable<string> cookies;
                if (response.Headers.TryGetValues("Set-Cookie", out cookies))
                {
                    var sessionCookie = cookies.FirstOrDefault(cookie => cookie.Contains("sessionId"));
                    if (!string.IsNullOrEmpty(sessionCookie))
                    {
                        var sessionId = sessionCookie.Split(';')[0].Split('=')[1];
                        // 存储 Cookie，可以选择存储在内存中，或者持久化到本地
                        StoreCookie(sessionId);
                        result = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Login failed. Please check your username and password.");
            }
            // return result;
        }

        private async Task<HttpResponseMessage> SendLoginRequest(string username, string password, string loginUrl = "http://yourserver.com/login")
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
        {
            { "username", username },
            { "password", password }
        };

                client.Timeout = TimeSpan.FromSeconds(5); // 设置超时时间，可根据需求调整

                var content = new FormUrlEncodedContent(values);

                try
                {
                    var response = await client.PostAsync(loginUrl, content);
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"请求登录时发生错误: {ex.Message}");
                    return null;
                }
            }
        }

        private void StoreCookie(string sessionId)
        {
            // 存储Cookie逻辑，可以存储在内存中，或者持久化到本地
            _appContext.SessionId = sessionId;
        }


        private async Task<HttpResponseMessage> SendRequestWithSessionId(string sessionId)
        {
            using (var client = new HttpClient())
            {
                // 添加SessionID到Cookie
                var cookie = new CookieContainer();
                cookie.Add(new Uri("http://yourserver.com"), new Cookie("sessionId", sessionId));


                // 发送请求
                var response = await client.GetAsync("http://yourserver.com/protected_resource");
                return response;
            }
        }


        public async Task<string> DeleteImageAsync(string imagePath, string token = null)
        {
            string serviceAddress = _configManager.GetValue("WebServerUrl");
            serviceAddress += "/deleteImages";
            try
            {
                // 构建请求
                var handler = new HttpClientHandler();
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 构建请求
                var request = new HttpRequestMessage(HttpMethod.Delete, serviceAddress);
                // 如果需要在请求中包含图片路径信息，可以通过请求头或请求内容添加
                request.Headers.TryAddWithoutValidation("ImagePath", imagePath);
                // 添加SessionID到Cookie
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri("http://yourserver.com"), new Cookie("sessionId", _appContext.SessionId));

                if (cookieContainer != null)
                {
                    handler.CookieContainer = cookieContainer;
                }
                // 发送 DELETE 请求
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static async Task<byte[]> DownloadImageAsync(string serviceAddress)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 发送 GET 请求
                var response = await httpClient.GetAsync(serviceAddress);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 应该要完善 添加验证
        /// </summary>
        /// <param name="serviceAddress"></param>
        /// <param name="imagePath"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static async Task<string> UploadImageAsync(string serviceAddress, string imagePath, string authToken)
        {

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                return "Error: Image file not found";
            }

            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 设置认证头，假设服务使用 bearer token
                if (!string.IsNullOrEmpty(authToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                }

                // 创建 MultipartFormDataContent 对象
                using var content = new MultipartFormDataContent();

                // 读取图片文件并添加到 MultipartFormDataContent
                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                using var fileStreamContent = new StreamContent(stream);
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileStreamContent, "file", Path.GetFileName(imagePath));

                // 发送 POST 请求
                var response = await httpClient.PostAsync(serviceAddress, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        /// <summary>
        /// 应该要完善 添加验证
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <param name="imgPath"></param>
        /// <param name="fileparameter"></param>
        /// <returns></returns>
        public static async Task<string> UploadImageAsyncOK(string uploadUrl, string imgPath, string fileparameter = "file")
        {
            if (string.IsNullOrEmpty(imgPath) || !File.Exists(imgPath))
            {
                return "Error: Image file not found";
            }
            string result = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(uploadUrl) as HttpWebRequest;
                request.AllowAutoRedirect = true;
                request.Method = "POST";

                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                int pos = imgPath.LastIndexOf("/");
                //string fileName = imgPath.Substring(pos + 1);
                string fileName = new System.IO.FileInfo(imgPath).Name;

                //请求头部信息
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"" + fileparameter + "\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();

                Stream postStream = await request.GetRequestStreamAsync();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UploadImageAsyncOK(string uploadUrl, string fileName, byte[] imgbytes, string fileparameter = "file")
        {
            if (imgbytes == null || imgbytes.Length == 0)
            {
                return "Error: Image file not found";
            }
            string result = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(uploadUrl) as HttpWebRequest;
                request.AllowAutoRedirect = true;
                request.Method = "POST";

                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                // 添加SessionID到Cookie
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri("http://yourserver.com"), new Cookie("sessionId", _appContext.SessionId));

                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }

                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");


                //请求头部信息
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"" + fileparameter + "\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                Stream postStream = await request.GetRequestStreamAsync();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(imgbytes, 0, imgbytes.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /*
        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="url">提交的地址</param>
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param>
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param>
        /// <param name="filepath">上传的文件路径  比如： c:/12.jpg </param>
        /// <param name="cookie">cookie数据</param>
        /// <param name="refre">头部的跳转地址</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string poststr, string fileformname, string filepath, string cookie, string refre)
        {

            // 这个可以是改变的，也可以是下面这个固定的字符串
            string boundary = "—————————6d930d1a850659";

            // 创建request对象
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.Headers.Add("Cookie: " + cookie);
            webrequest.Referer = refre;

            // 构造发送数据
            StringBuilder sb = new StringBuilder();

            // 文本域的数据，将user=eking&pass=123456  格式的文本域拆分 ，然后构造
            foreach (string c in poststr.Split('&'))
            {
                string[] item = c.Split('=');
                if (item.Length != 2)
                {
                    break;
                }
                string name = item[0];
                string value = item[1];
                sb.Append("–" + boundary);
                sb.Append("/r/n");
                sb.Append("Content-Disposition: form-data; name=/"" + name + @" / "");
                sb.Append("/r/n/r/n");
                sb.Append(value);
                sb.Append("/r/n");
            }

            // 文件域的数据
            sb.Append("–" + boundary);
            sb.Append("/r/n");
            sb.Append("Content-Disposition: form-data; name=/"icon / ";filename=/"" + filepath + " / "");
            sb.Append("/r/n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("/r/n/r/n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = System.Text.Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("/r/n–" + boundary + "–/r/n");

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码)
            return sr.ReadToEnd();
        } 
        */

        //下载图片文件
        public async Task<byte[]> DownloadImgFileAsync(string serviceAddress)
        {
            string imageUrl = "http://192.168.0.99:8080/ERPImages/" + serviceAddress + ".jpg";
            byte[] result = new byte[0];
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3); // 设置 3 秒超时
                    // 发送GET请求
                    HttpResponseMessage response = await client.GetAsync(imageUrl);
                    response.EnsureSuccessStatusCode();
                    // 读取响应内容为字节数组
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    result = new byte[imageBytes.Length];
                    result = imageBytes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }


        public static async Task<string> GetAsync(string serviceAddress)
        {
            try
            {
                string result = string.Empty;
                Uri getUrl = new Uri(serviceAddress);
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);
                result = await httpClient.GetAsync(serviceAddress).Result.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static async Task<string> PostAsync(string serviceAddress, string requestJson = null)
        {
            try
            {
                string result = string.Empty;
                Uri postUrl = new Uri(serviceAddress);

                using (HttpContent httpContent = new StringContent(requestJson))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using var httpClient = new HttpClient();
                    httpClient.Timeout = new TimeSpan(0, 0, 60);
                    result = await httpClient.PostAsync(serviceAddress, httpContent).Result.Content.ReadAsStringAsync();
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
