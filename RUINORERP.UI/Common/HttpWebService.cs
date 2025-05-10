using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using Netron.GraphLib;
using NSoup.Helper;
using Org.BouncyCastle.Asn1.Ocsp;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.UI.SysConfig;
using SqlSugar.SplitTableExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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


        #region 测试DefaultAddElseUpdate

        public async Task<int> DefaultAddElseUpdateTest()
        {
            tb_Unit unit=new tb_Unit();
            unit.UnitName = "测试GH";

            //下面的写法可以做到批量的  插入更新。雪花ID
            var x = _unitOfWorkManage.GetDbClient().Storageable(unit).ToStorage();
            x.AsInsertable.ExecuteReturnSnowflakeIdList();//不存在插入
            return await x.AsUpdateable.ExecuteCommandAsync();//存在更新

        
            List<tb_Unit> units = new List<tb_Unit>();
            units.Add(new tb_Unit() { Notes = "0001LL", UnitName = "a" });
            units.Add(new tb_Unit() { Notes = "0002AA", UnitName = "b" });
            units.Add(new tb_Unit() { Unit_ID = 1920666804015992832, UnitName = "8aa" });
            units.Add(new tb_Unit() { Unit_ID = 1921125849642438656, UnitName = "9bb" });

            ////下面的写法可以做到批量的  插入更新。雪花ID
            //var x = _unitOfWorkManage.GetDbClient().Storageable(units).ToStorage();
            //x.AsInsertable.ExecuteReturnSnowflakeIdList();//不存在插入
            //return await x.AsUpdateable.ExecuteCommandAsync();//存在更新

        }

        #endregion


        #region 登陆
        /// <summary>
        /// 登陆webserver
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginUrl"></param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password, string loginUrl = "http://yourserver.com/login")
        {
            bool result = false;
            // 检查用户名和密码
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //return result;
            }

            // 发送登录请求到服务端
            var response = await SendLoginRequest(username, password, loginUrl);
            if (response == null)
            {
                MessageBox.Show("web服务器连接失败，请检查网络连接，或联系管理员确认服务器是否正常运行。");
                return false;
            }
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
                        result = StoreCookie(sessionId);
                    }
                }
            }
            else
            {
                MessageBox.Show("Login failed. Please check your username and password.");
            }
            return result;
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

        private bool StoreCookie(string sessionId)
        {
            // 存储Cookie逻辑，可以存储在内存中，或者持久化到本地
            _appContext.SessionId = sessionId;
            return true;
        }


        #endregion


        public async Task<string> DeleteImageAsync(string imagefileName, string token = null)
        {
            string serviceAddress = _configManager.GetValue("WebServerUrl");
            serviceAddress += "/deleteImages";//与服务器对应 写死的只处理删除图片
            try
            {
                // 构建请求
                var handler = new HttpClientHandler();
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 构建请求
                var request = new HttpRequestMessage(HttpMethod.Delete, serviceAddress);
                // 如果需要在请求中包含图片路径信息，可以通过请求头或请求内容添加
                request.Headers.TryAddWithoutValidation("fileName", imagefileName);//后缀.jpg在服务器添加了。
                handler.CookieContainer = GetCookieContainer();
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


        /// <summary>
        ///   添加SessionID到Cookie
        /// </summary>
        private CookieContainer GetCookieContainer()
        {
            // 添加SessionID到Cookie
            var cookieContainer = new CookieContainer();
            string webServerUrl = _configManager.GetValue("WebServerUrl");
            cookieContainer.Add(new Uri(webServerUrl), new Cookie("sessionId", _appContext.SessionId));

            return cookieContainer;
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

        public async Task<string> UploadImageAsync(string fileName, byte[] imgbytes, string fileparameter = "file")
        {
            string uploadUrl = _configManager.GetValue("WebServerUrl");
            uploadUrl += "/upload/";
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
                request.CookieContainer = GetCookieContainer();

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
                request.CookieContainer = GetCookieContainer();

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
                return "上传成功" + sr.ReadToEnd();
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
        public async Task<byte[]> DownloadImgFileAsync(string fileNameNoExtName)
        {
            var WebServerUrl = _configManager.GetValue("WebServerUrl");
            var ServerImageDirectory = _configManager.GetValue("ServerImageDirectory");
            string imageUrl = WebServerUrl + "/" + ServerImageDirectory + "/" + fileNameNoExtName + ".jpg";
            byte[] result = new byte[0];
            try
            {
                // 构建请求
                var handler = new HttpClientHandler();

                using (HttpClient client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(5); // 设置 5 秒超时
                    handler.CookieContainer = GetCookieContainer();
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
                //MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
