using SimpleHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WebServerConsole
{
    //身份验证服务
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(HttpListenerRequest request, HttpListenerResponse response);
    }

    /// <summary>
    /// 验证服务
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<bool> Authenticate(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Cookies["sessionId"] == null)
            {
                return false;
            }

            string sessionId = request.Cookies["sessionId"].Value;
            if (SessionStorage.Exists(sessionId))
            {
                var userState = SessionStorage.Get(sessionId);
                // 处理请求
            }
            else
            {
                response.StatusCode = 401; // Unauthorized
                //response.Close(); // 关闭连接后后面无法访问
                return false;
            }

            // 实现身份验证逻辑，例如检查请求头中的令牌

            return true; // 假设用户已通过身份验证
        }

        // 伪代码示例
        public void LoginByQueryString(HttpListenerRequest request, HttpListenerResponse response)
        {

            string username = request.QueryString["username"];
            string password = request.QueryString["password"];

            // 验证用户名和密码
            if (ValidateUser(username, password))
            {
                // 创建Session
                string sessionId = Guid.NewGuid().ToString();
                Cookie sessionCookie = new Cookie("sessionId", sessionId);

                // 设置Cookie过期时间为24小时
                sessionCookie.Expires = DateTime.Now.AddHours(24);
                response.Cookies.Add(sessionCookie);

                // 发送成功响应
                response.ContentType = "text/plain";
                response.AsText("Login successful.");
            }
            else
            {
                // 发送失败响应
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.AsText("Invalid username or password.");
            }
        }
        public async Task<bool> LoginAsync(HttpListenerRequest request, HttpListenerResponse rp)
        {
            if (request.HasEntityBody)
            {
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    var body = await reader.ReadToEndAsync();
                    var formData = ParseFormData(body);
                    var username = formData["username"];
                    var password = formData["password"];
                    // 验证用户名和密码
                    if (ValidateUser(username, password))
                    {
                        // 创建Session
                        string sessionId = Guid.NewGuid().ToString();
                        SessionStorage.Add(sessionId, new { Username = username }, 30); //会话超时设置为30分钟
                        Cookie sessionCookie = new Cookie("sessionId", sessionId);

                        // 设置Cookie过期时间为24小时
                        sessionCookie.Expires = DateTime.Now.AddHours(24);
                        rp.Cookies.Add(sessionCookie);

                        // 发送成功响应
                        rp.ContentType = "text/plain";
                        rp.AsText("Login successful.");
                    }
                    else
                    {
                        // 发送失败响应
                        rp.StatusCode = (int)HttpStatusCode.Unauthorized;
                        rp.AsText("Invalid username or password.");
                    }
                }
            }
            return false;
        }

        private Dictionary<string, string> ParseFormData(string formData)
        {
            var data = new Dictionary<string, string>();
            var parts = formData.Split('&');
            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                if (keyValue.Length == 2)
                {
                    data[Uri.UnescapeDataString(keyValue[0])] = Uri.UnescapeDataString(keyValue[1]);
                }
            }
            return data;
        }

        private bool ValidateUser(string username, string password)
        {
            // 这里应该查询数据库验证用户
            // 仅为示例，这里直接返回true
            return true;
        }
    }

    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthorizationService
    {
        bool Authorize(HttpListenerRequest request, string resource);
    }

    /// <summary>
    /// 授权服务
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        public bool Authorize(HttpListenerRequest request, string resource)
        {
            // 实现授权逻辑，例如检查用户的角色和权限
            return true; // 假设用户有权限访问资源
        }
    }


}
