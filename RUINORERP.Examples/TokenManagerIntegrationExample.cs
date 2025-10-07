using RUINORERP.PacketSpec.Tokens;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Examples
{
    /// <summary>
    /// TokenManager集成示例
    /// 展示如何在客户端应用程序中初始化和使用新的Token管理架构
    /// 推荐使用依赖注入方式获取TokenManager实例
    /// </summary>
    public class TokenManagerIntegrationExample
    {
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 构造函数 - 通过依赖注入获取TokenManager实例
        /// </summary>
        /// <param name="tokenManager">TokenManager实例</param>
        public TokenManagerIntegrationExample(TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        /// <summary>
        /// 在应用程序启动时初始化Token管理系统
        /// 推荐使用依赖注入方式获取TokenManager实例
        /// </summary>
        public static void InitializeTokenSystem()
        {
            // TokenManager推荐使用依赖注入，无需手动初始化
            Console.WriteLine("Token管理系统初始化成功 - 推荐使用依赖注入");
        }
        
        /// <summary>
        /// 示例：如何设置认证Token
        /// 通常在用户登录成功后调用
        /// </summary>
        /// <param name="accessToken">从服务器获取的访问Token</param>
        /// <param name="refreshToken">从服务器获取的刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        public async Task SetAuthenticationTokens(string accessToken, string refreshToken, int expiresInSeconds)
        {
            // 通过TokenManager设置Token - 使用新的Token体系
            var tokenInfo = new TokenInfo 
            { 
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresInSeconds
            };
            
            await _tokenManager.TokenStorage.SetTokenAsync(tokenInfo);
            
            Console.WriteLine($"Token设置成功，过期时间: {DateTime.UtcNow.AddSeconds(expiresInSeconds).ToString("yyyy-MM-dd HH:mm:ss")} UTC");
        }
        
        /// <summary>
        /// 示例：如何使用TokenManager检查Token是否过期
        /// 推荐使用TokenManager.ValidateStoredTokenAsync()进行统一验证
        /// </summary>
        /// <returns>如果Token已过期或无效则返回true，否则返回false</returns>
        public async Task<bool> CheckIfTokenExpired()
        {
            // 推荐使用：使用TokenManager的统一验证方法
            var validationResult = await _tokenManager.ValidateStoredTokenAsync();
            bool isExpired = !validationResult.IsValid;
            
            if (isExpired)
            {
                Console.WriteLine($"Token已过期或无效，需要刷新。原因: {validationResult.ErrorMessage}");
            }
            else
            {
                Console.WriteLine("Token有效，可以继续使用");
            }
            
            return isExpired;
        }

        /// <summary>
        /// 示例：如何使用TokenManager.ValidateStoredTokenAsync()进行统一Token验证
        /// 推荐的新方法，提供更详细的验证结果
        /// </summary>
        /// <returns>Token验证结果</returns>
        public async Task<TokenValidationResult> ValidateTokenAsync()
        {
            try
            {
                // 使用TokenManager的统一验证方法
                var validationResult = await _tokenManager.ValidateStoredTokenAsync();
                
                if (validationResult.IsValid)
                {
                    Console.WriteLine($"Token验证成功");
                }
                else
                {
                    Console.WriteLine($"Token验证失败: {validationResult.ErrorMessage}");
                }
                
                return validationResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token验证异常: {ex.Message}");
                return new TokenValidationResult { IsValid = false, ErrorMessage = ex.Message };
            }
        }
        
        /// <summary>
        /// 示例：如何使用命令类（BaseCommand子类）
        /// AutoAttachToken方法会自动使用TokenManager中的Token
        /// </summary>
        public async Task UseCommandWithToken()
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
        public async Task ClearAuthenticationTokens()
        {
            // 通过TokenManager清除Token - 使用新的Token体系
            await _tokenManager.TokenStorage.ClearTokenAsync();
            
            Console.WriteLine("Token已清除");
        }
        
        /// <summary>
        /// 示例：如何获取当前存储的Token信息
        /// </summary>
        public async Task<(bool Success, string AccessToken, string RefreshToken)> GetCurrentTokens()
        {
            // 从TokenManager获取Token - 使用新的Token体系
            var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
            
            if (tokenInfo != null)
            {
                Console.WriteLine("成功获取Token信息");
                Console.WriteLine($"访问Token长度: {tokenInfo.AccessToken?.Length ?? 0} 字符");
                Console.WriteLine($"刷新Token长度: {tokenInfo.RefreshToken?.Length ?? 0} 字符");
                return (true, tokenInfo.AccessToken, tokenInfo.RefreshToken);
            }
            else
            {
                Console.WriteLine("未找到有效Token信息");
                return (false, null, null);
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