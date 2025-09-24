using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Communication
{
    /// <summary>
    /// 统一通信处理器
    /// 整合命令分发和结果处理的通用实现，同时支持服务器端和客户端
    /// </summary>
    public class CommunicationHandler
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<CommunicationHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="logger">日志记录器</param>
        public CommunicationHandler(ICommandDispatcher commandDispatcher, ILogger<CommunicationHandler> logger)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _logger = logger;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        public async Task<CommandResult> HandleCommandAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                LogDebug($"准备处理命令: {command.GetType().Name} [ID: {command.CommandId}]");

                // 验证命令
                var validationResult = await ValidateCommandAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    LogWarning($"命令验证失败: {validationResult.ErrorMessage}");
                    return CommandResult.Failure(validationResult.ErrorMessage, ErrorCodes.ValidationFailed);
                }

                // 命令前置处理
                await OnBeforeCommandAsync(command, cancellationToken);

                // 执行命令
                var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

                // 命令后置处理
                await OnAfterCommandAsync(command, result, cancellationToken);

                LogDebug($"命令处理完成: {command.GetType().Name} [ID: {command.CommandId}], 结果: {(result.IsSuccess ? "成功" : "失败")}");
                return result;
            }
            catch (OperationCanceledException)
            {
                LogWarning($"命令处理被取消: {command.GetType().Name} [ID: {command.CommandId}]");
                return CommandResult.Failure("命令处理被取消", ErrorCodes.DispatchCancelled);
            }
            catch (Exception ex)
            {
                LogError($"命令处理异常: {command.GetType().Name} [ID: {command.CommandId}]", ex);
                return CommandResult.FromException(ex, command.CommandId);
            }
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        protected virtual Task<ValidationResult> ValidateCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            // 默认验证：命令不能为空且必须有有效的CommandId
            if (command == null)
            {
                return Task.FromResult(new ValidationResult { IsValid = false, ErrorMessage = "命令对象不能为空" });
            }

            if (command.Packet.Command.Category == 0 && command.Packet.Command.OperationCode == 0)
            {
                return Task.FromResult(new ValidationResult { IsValid = false, ErrorMessage = "命令ID无效" });
            }

            return Task.FromResult(new ValidationResult { IsValid = true });
        }

        /// <summary>
        /// 命令执行前处理
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        protected virtual Task OnBeforeCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            // 默认实现，子类可以重写
            return Task.CompletedTask;
        }

        /// <summary>
        /// 命令执行后处理
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="result">执行结果</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>任务</returns>
        protected virtual Task OnAfterCommandAsync(ICommand command, CommandResult result, CancellationToken cancellationToken)
        {
            // 默认实现，子类可以重写
            return Task.CompletedTask;
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected void LogDebug(string message)
        {
            _logger?.LogDebug(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected void LogWarning(string message)
        {
            _logger?.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常对象</param>
        protected void LogError(string message, Exception exception = null)
        {
            if (exception == null)
            {
                _logger?.LogError(message);
            }
            else
            {
                _logger?.LogError(exception, message);
            }
        }

        /// <summary>
        /// 验证结果类
        /// </summary>
        protected class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
