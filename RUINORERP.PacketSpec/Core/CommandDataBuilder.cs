using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.ComponentModel;

namespace RUINORERP.PacketSpec.Core
{
    /// <summary>
    /// 统一命令构建器 - 简化客户端数据构建
    /// </summary>
    public static class CommandDataBuilder
    {
        /// <summary>
        /// 构建强类型命令（BaseCommand<TRequest, TResponse>）
        /// </summary>
        public static BaseCommand<TRequest, TResponse> BuildCommand<TRequest, TResponse>(
            CommandId commandId,
            TRequest request,
            Action<BaseCommand<TRequest, TResponse>> config = null)
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            var command = new GenericCommand<TRequest, TResponse>(commandId, request);
            config?.Invoke(command);
            
            // 自动设置常用属性
            command.TimeoutMs = command.TimeoutMs > 0 ? command.TimeoutMs : 30000;
            command.UpdateTimestamp();
            
            return command;
        }
        
        /// <summary>
        /// 构建泛型命令（GenericCommand<TPayload>）
        /// </summary>
        public static GenericCommand<TPayload> BuildGenericCommand<TPayload>(
            CommandId commandId,
            TPayload payload,
            Action<GenericCommand<TPayload>> config = null)
        {
            var command = new GenericCommand<TPayload>(commandId, payload);
            config?.Invoke(command);
            
            command.TimeoutMs = command.TimeoutMs > 0 ? command.TimeoutMs : 30000;
            command.UpdateTimestamp();
            
            return command;
        }
        
        /// <summary>
        /// 构建基础命令（BaseCommand）
        /// </summary>
        public static BaseCommand BuildBaseCommand(
            CommandId commandId,
            object data = null,
            Action<BaseCommand> config = null)
        {
            var command = new GenericCommand<object>(commandId, data);
            config?.Invoke(command);
            
            command.TimeoutMs = command.TimeoutMs > 0 ? command.TimeoutMs : 30000;
            command.UpdateTimestamp();
            
            return command;
        }
        

        
        /// <summary>
        /// 构建Token验证命令
        /// </summary>
        public static GenericCommand<TokenValidationRequest> BuildTokenValidationCommand(string token)
        {
            var request = new TokenValidationRequest { Token = token };
            return BuildGenericCommand(
                AuthenticationCommands.ValidateToken,
                request,
                cmd => cmd.TimeoutMs = 15000
            );
        }
        
        /// <summary>
        /// 构建Token刷新命令
        /// </summary>
        public static GenericCommand<TokenRefreshRequest> BuildTokenRefreshCommand(string token, string refreshToken)
        {
            var request = new TokenRefreshRequest
            {
                Token = token,
                RefreshToken = refreshToken
            };
            return BuildGenericCommand(
                AuthenticationCommands.RefreshToken,
                request,
                cmd => cmd.TimeoutMs = 20000
            );
        }
    }


}
