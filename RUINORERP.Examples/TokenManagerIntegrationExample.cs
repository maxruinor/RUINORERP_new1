using RUINORERP.PacketSpec.Tokens;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using System;

namespace RUINORERP.Examples
{
    /// <summary>
    /// TokenManager集成示例
    /// 展示如何在客户端应用程序中初始化和使用新的Token管理架构
    /// </summary>
    public class TokenManagerIntegrationExample
    {
        /// <summary>
        /// 在应用程序启动时初始化Token管理系统
        /// 现在只需要使用TokenManager单例即可
        /// </summary>
        public static void InitializeTokenSystem()
        {
            // TokenManager已经使用单例模式，无需额外初始化
            Console.WriteLine("Token管理系统初始化成功");
        }
        
        /// <summary>
        /// 示例：如何设置认证Token
        /// 通常在用户登录成功后调用
        /// </summary>
        /// <param name="accessToken">从服务器获取的访问Token</param>
        /// <param name="refreshToken">从服务器获取的刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        public static void SetAuthenticationTokens(string accessToken, string refreshToken, int expiresInSeconds)
        {
            // 通过TokenManager设置Token
            TokenManager.Instance.SetTokens(accessToken, refreshToken, expiresInSeconds);
            
            Console.WriteLine($"Token设置成功，过期时间: {DateTime.UtcNow.AddSeconds(expiresInSeconds).ToString("yyyy-MM-dd HH:mm:ss")} UTC");
        }
        
        /// <summary>
        /// 示例：如何使用TokenManager检查Token是否过期
        /// 可以在发起请求前调用此方法
        /// </summary>
        /// <returns>如果Token已过期或即将过期则返回true，否则返回false</returns>
        public static bool CheckIfTokenExpired()
        {
            bool isExpired = TokenManager.Instance.IsAccessTokenExpired();
            
            if (isExpired)
            {
                Console.WriteLine("Token已过期或即将过期，需要刷新");
            }
            else
            {
                Console.WriteLine("Token有效，可以继续使用");
            }
            
            return isExpired;
        }
        
        /// <summary>
        /// 示例：如何使用命令类（BaseCommand子类）
        /// AutoAttachToken方法会自动使用TokenManager中的Token
        /// </summary>
        public static void UseCommandWithToken()
        {
            // 创建一个命令实例（假设这是某个业务命令）
            var businessCommand = new SomeBusinessCommand();
            
            // 命令的AutoAttachToken方法会自动从TokenManager获取Token
            // 不需要为每个命令单独设置Token提供者
            businessCommand.AutoAttachToken();
            
            Console.WriteLine("命令已自动附加Token");
            Console.WriteLine($"Token类型: {businessCommand.TokenType}");
            Console.WriteLine($"Token长度: {businessCommand.AuthToken?.Length ?? 0} 字符");
        }
        
        /// <summary>
        /// 示例：如何在登出时清除Token
        /// </summary>
        public static void ClearAuthenticationTokens()
        {
            // 通过TokenManager清除Token
            TokenManager.Instance.ClearTokens();
            
            Console.WriteLine("Token已清除");
        }
        
        /// <summary>
        /// 示例：如何获取当前存储的Token信息
        /// </summary>
        public static void GetCurrentTokens()
        {
            // 从TokenManager获取Token
            var (success, accessToken, refreshToken) = TokenManager.Instance.GetTokens();
            
            if (success)
            {
                Console.WriteLine("成功获取Token信息");
                Console.WriteLine($"访问Token长度: {accessToken?.Length ?? 0} 字符");
                Console.WriteLine($"刷新Token长度: {refreshToken?.Length ?? 0} 字符");
            }
            else
            {
                Console.WriteLine("未找到有效Token信息");
            }
        }
    }
    
    /// <summary>
    /// 示例业务命令类
    /// 继承自BaseCommand，用于演示AutoAttachToken功能
    /// </summary>
    public class SomeBusinessCommand : BaseCommand
    {
        // 业务命令的具体实现
        // ...
    }
}