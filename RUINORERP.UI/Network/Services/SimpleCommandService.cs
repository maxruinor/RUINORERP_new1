using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 简单指令服务 - 提供使用简单请求/响应的命令发送功能
    /// 避免为简单指令创建单独的请求实体类
    /// </summary>
    public class SimpleCommandService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ILogger<SimpleCommandService> _logger;
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志服务</param>
        /// <param name="authService">认证服务</param>
        public SimpleCommandService(
            IClientCommunicationService communicationService,
            ILogger<SimpleCommandService> logger,
            IAuthenticationService authService)
        {
            _communicationService = communicationService;
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// 发送简单字符串指令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">字符串值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>简单响应</returns>
        public async Task<SimpleResponse> SendStringCommandAsync(
            CommandId commandId,
            string value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = SimpleRequest.CreateString(value, operationType);
                request.RequestId = IdGenerator.GenerateRequestId(commandId);
                
                var command = new GenericCommand<SimpleRequest, SimpleResponse>(commandId, request);
                
                _logger.LogInformation("发送字符串命令: {CommandId}, 值: {Value}", commandId, value);
                
                var response = await _communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(
                    command, cancellationToken);
                
                if (response != null && response.IsSuccess)
                {
                    _logger.LogInformation("字符串命令执行成功: {CommandId}", commandId);
                }
                else
                {
                    _logger.LogWarning("字符串命令执行失败: {CommandId}, 消息: {Message}", 
                        commandId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送字符串命令失败: {CommandId}", commandId);
                return SimpleResponse.CreateFailure($"发送命令失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送简单布尔值指令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">布尔值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>简单响应</returns>
        public async Task<SimpleResponse> SendBoolCommandAsync(
            CommandId commandId,
            bool value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = SimpleRequest.CreateBool(value, operationType);
                request.RequestId = IdGenerator.GenerateRequestId(commandId);
                
                var command = new GenericCommand<SimpleRequest, SimpleResponse>(commandId, request);
                
                _logger.LogInformation("发送布尔值命令: {CommandId}, 值: {Value}", commandId, value);
                
                var response = await _communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(
                    command, cancellationToken);
                
                if (response != null && response.IsSuccess)
                {
                    _logger.LogInformation("布尔值命令执行成功: {CommandId}", commandId);
                }
                else
                {
                    _logger.LogWarning("布尔值命令执行失败: {CommandId}, 消息: {Message}", 
                        commandId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送布尔值命令失败: {CommandId}", commandId);
                return SimpleResponse.CreateFailure($"发送命令失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送简单整数值指令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">整数值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>简单响应</returns>
        public async Task<SimpleResponse> SendIntCommandAsync(
            CommandId commandId,
            int value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = SimpleRequest.CreateInt(value, operationType);
                request.RequestId = IdGenerator.GenerateRequestId(commandId);
                
                var command = new GenericCommand<SimpleRequest, SimpleResponse>(commandId, request);
                
                _logger.LogInformation("发送整数值命令: {CommandId}, 值: {Value}", commandId, value);
                
                var response = await _communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(
                    command, cancellationToken);
                
                if (response != null && response.IsSuccess)
                {
                    _logger.LogInformation("整数值命令执行成功: {CommandId}", commandId);
                }
                else
                {
                    _logger.LogWarning("整数值命令执行失败: {CommandId}, 消息: {Message}", 
                        commandId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送整数值命令失败: {CommandId}", commandId);
                return SimpleResponse.CreateFailure($"发送命令失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送通用简单指令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">数据对象</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>简单响应</returns>
        public async Task<SimpleResponse> SendSimpleCommandAsync(
            CommandId commandId,
            object data,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = SimpleRequest.CreateObject(data, operationType: operationType);
                request.RequestId = IdGenerator.GenerateRequestId(commandId);
                
                var command = new GenericCommand<SimpleRequest, SimpleResponse>(commandId, request);
                
                _logger.LogInformation("发送简单命令: {CommandId}, 数据类型: {DataType}", 
                    commandId, data?.GetType().Name ?? "null");
                
                var response = await _communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(
                    command, cancellationToken);
                
                if (response != null && response.IsSuccess)
                {
                    _logger.LogInformation("简单命令执行成功: {CommandId}", commandId);
                }
                else
                {
                    _logger.LogWarning("简单命令执行失败: {CommandId}, 消息: {Message}", 
                        commandId, response?.Message ?? "未知错误");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送简单命令失败: {CommandId}", commandId);
                return SimpleResponse.CreateFailure($"发送命令失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 发送简单指令并返回强类型结果
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">数据对象</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>强类型值</returns>
        public async Task<T> SendSimpleCommandAsync<T>(
            CommandId commandId,
            object data,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            var response = await SendSimpleCommandAsync(commandId, data, operationType, cancellationToken);
            
            if (response == null || !response.IsSuccess)
            {
                throw new InvalidOperationException($"命令执行失败: {response?.Message ?? "未知错误"}");
            }
            
            return response.GetValue<T>();
        }

        /// <summary>
        /// 发送简单字符串指令并返回字符串结果
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">字符串值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>字符串结果</returns>
        public async Task<string> SendStringCommandAsync(
            CommandId commandId,
            string value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            var response = await SendStringCommandAsync(commandId, value, operationType, cancellationToken);
            
            if (response == null || !response.IsSuccess)
            {
                throw new InvalidOperationException($"命令执行失败: {response?.Message ?? "未知错误"}");
            }
            
            return response.GetStringValue();
        }

        /// <summary>
        /// 发送简单布尔值指令并返回布尔值结果
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">布尔值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>布尔值结果</returns>
        public async Task<bool> SendBoolCommandAsync(
            CommandId commandId,
            bool value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            var response = await SendBoolCommandAsync(commandId, value, operationType, cancellationToken);
            
            if (response == null || !response.IsSuccess)
            {
                throw new InvalidOperationException($"命令执行失败: {response?.Message ?? "未知错误"}");
            }
            
            return response.GetBoolValue();
        }

        /// <summary>
        /// 发送简单整数值指令并返回整数值结果
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="value">整数值</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>整数值结果</returns>
        public async Task<int> SendIntCommandAsync(
            CommandId commandId,
            int value,
            string operationType = null,
            CancellationToken cancellationToken = default)
        {
            var response = await SendIntCommandAsync(commandId, value, operationType, cancellationToken);
            
            if (response == null || !response.IsSuccess)
            {
                throw new InvalidOperationException($"命令执行失败: {response?.Message ?? "未知错误"}");
            }
            
            return response.GetIntValue();
        }
    }
}