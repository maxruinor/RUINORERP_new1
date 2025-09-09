using RUINORERP.SimpleHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS
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
            // 检查是否存在sessionId cookie
            if (request.Cookies["sessionId"] == null)
            {
                response.StatusCode = 401; // Unauthorized
                return false;
            }

            string sessionId = request.Cookies["sessionId"].Value;
            
            // 验证会话是否存在且未过期
            if (SessionStorage.Exists(sessionId))
            {
                // 可以在这里添加额外的验证逻辑，如IP地址检查等
                var userState = SessionStorage.Get(sessionId);
                // 可以将用户信息添加到请求上下文中供后续使用
                return true;
            }
            else
            {
                response.StatusCode = 401; // Unauthorized
                return false;
            }
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
                var userContext = new { Username = username, LoginTime = DateTime.UtcNow };
                SessionStorage.Add(sessionId, userContext, 30); // 会话超时设置为30分钟

                Cookie sessionCookie = new Cookie("sessionId", sessionId);
                
                // 设置Cookie属性增强安全性
                sessionCookie.HttpOnly = true; // 防止XSS攻击
                sessionCookie.Secure = false; // 如果使用HTTPS则设置为true
                sessionCookie.Expires = DateTime.Now.AddHours(24); // 设置Cookie过期时间为24小时
                
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
                frmMain.Instance.PrintInfoLog("有请求体");
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    var body = await reader.ReadToEndAsync();
                    var formData = ParseFormData(body);
                    
                    // 确保必要的字段存在
                    if (!formData.ContainsKey("username") || !formData.ContainsKey("password"))
                    {
                        rp.StatusCode = (int)HttpStatusCode.BadRequest;
                        rp.AsText("Username and password are required.");
                        return false;
                    }
                    
                    var username = formData["username"];
                    var password = formData["password"];
                    
                    // 验证用户名和密码
                    if (ValidateUser(username, password))
                    {
                        // 创建Session
                        string sessionId = Guid.NewGuid().ToString();
                        var userContext = new { Username = username, LoginTime = DateTime.UtcNow };
                        SessionStorage.Add(sessionId, userContext, 30); // 会话超时设置为30分钟
                        
                        Cookie sessionCookie = new Cookie("sessionId", sessionId);
                        
                        // 设置Cookie属性增强安全性
                        sessionCookie.HttpOnly = true; // 防止XSS攻击
                        sessionCookie.Secure = false; // 如果使用HTTPS则设置为true
                        sessionCookie.Expires = DateTime.Now.AddHours(24); // 设置Cookie过期时间为24小时
                        
                        rp.Cookies.Add(sessionCookie);

                        // 发送成功响应
                        rp.ContentType = "application/json";
                        var response = new { 
                            Status = "success", 
                            Message = "Login successful",
                            SessionId = sessionId
                        };
                        rp.AsText(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        frmMain.Instance.PrintInfoLog("Login successful");
                        return true;
                    }
                    else
                    {
                        // 发送失败响应
                        rp.StatusCode = (int)HttpStatusCode.Unauthorized;
                        var response = new { 
                            Status = "error", 
                            Message = "Invalid username or password"
                        };
                        rp.AsText(Newtonsoft.Json.JsonConvert.SerializeObject(response));
                        frmMain.Instance.PrintInfoLog("Invalid username or password");
                        return false;
                    }
                }
            }
            else
            {
                frmMain.Instance.PrintInfoLog("没有请求体");
                rp.StatusCode = (int)HttpStatusCode.BadRequest;
                rp.AsText("Request body is required.");
                return false;
            }
        }

        private Dictionary<string, string> ParseFormData(string formData)
        {
            var data = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(formData))
                return data;
                
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
            // 实际实现应该：
            // 1. 查询数据库中的用户信息
            // 2. 验证密码（应该使用哈希比较而不是明文比较）
            // 3. 检查账户状态（是否被锁定等）
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
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
            // 可以根据资源类型和用户角色进行细粒度控制
            
            // 从会话中获取用户信息
            if (request.Cookies["sessionId"] != null)
            {
                string sessionId = request.Cookies["sessionId"].Value;
                if (SessionStorage.Exists(sessionId))
                {
                    // 获取用户信息并检查权限
                    var userState = SessionStorage.Get(sessionId);
                    // 根据用户角色和资源类型判断是否有权限
                    // 这里只是一个示例，实际实现需要根据业务需求进行
                    return true;
                }
            }
            
            return false;
        }
    }
}