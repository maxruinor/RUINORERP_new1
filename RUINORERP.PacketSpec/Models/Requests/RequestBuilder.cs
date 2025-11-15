using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using System;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 请求数据构建器 - 简化客户端请求数据构建
    /// 用于创建和配置各种类型的请求对象，不依赖于BaseCommand
    /// </summary>
    public static class RequestBuilder
    {
        /// <summary>
        /// 创建带CommandId的请求对象
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求对象</param>
        /// <param name="config">配置操作</param>
        /// <returns>配置好的请求对象</returns>
        public static TRequest CreateRequest<TRequest>(
            CommandId commandId,
            TRequest request,
            Action<TRequest> config = null) 
            where TRequest : class, IRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
                
            // 设置请求ID
            request.RequestId = IdGenerator.GenerateRequestId(commandId);
            
            // 应用配置
            config?.Invoke(request);
            
            return request;
        }

        /// <summary>
        /// 创建Token验证请求
        /// </summary>
        /// <param name="token">令牌信息</param>
        /// <returns>Token验证请求对象</returns>
        public static TokenValidationRequest CreateTokenValidationRequest(TokenInfo token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
                
            return CreateRequest(
                AuthenticationCommands.ValidateToken,
                new TokenValidationRequest { Token = token }
            );
        }

        /// <summary>
        /// 创建Token刷新请求
        /// </summary>
        /// <param name="token">当前令牌</param>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>Token刷新请求对象</returns>
        public static TokenRefreshRequest CreateTokenRefreshRequest(string token, string refreshToken)
        {
            return CreateRequest(
                AuthenticationCommands.RefreshToken,
                new TokenRefreshRequest
                {
                    RefreshToken = refreshToken
                }
            );
        }
        
         
        
        /// <summary>
        /// 创建布尔值请求
        /// </summary>
        /// <param name="commandId">命令标识符</param>
        /// <param name="value">布尔值</param>
        /// <returns>布尔值请求对象</returns>
        public static BooleanRequest CreateBooleanRequest(CommandId commandId, bool value)
        {
            return CreateRequest(
                commandId,
                new BooleanRequest { Value = value }
            );
        }
        
        /// <summary>
        /// 创建数值请求
        /// </summary>
        /// <param name="commandId">命令标识符</param>
        /// <param name="value">数值</param>
        /// <returns>数值请求对象</returns>
        public static NumericRequest CreateNumericRequest(CommandId commandId, decimal value)
        {
            return CreateRequest(
                commandId,
                new NumericRequest { Value = value }
            );
        }
    }
}
