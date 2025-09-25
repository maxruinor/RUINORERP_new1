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
    /// 统一通信处理器基类
    /// 提供命令验证、执行和结果处理的标准化框架，支持服务器端和客户端的通信处理
    /// 包含完整的异常处理、日志记录和生命周期管理机制
    /// </summary>
    public class CommunicationHandler : IDisposable
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<CommunicationHandler> _logger;
        private readonly object _lock = new object();
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当commandDispatcher为null时抛出</exception>
        public CommunicationHandler(ICommandDispatcher commandDispatcher, ILogger<CommunicationHandler> logger)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _logger = logger;
            
            LogDebug($"CommunicationHandler已初始化，命令调度器类型: {commandDispatcher.GetType().Name}");
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        /// <exception cref="ObjectDisposedException">当对象已释放时抛出</exception>
        /// <exception cref="ArgumentNullException">当command为null时抛出</exception>
        public async Task<CommandResult> HandleCommandAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CommunicationHandler));

            if (command == null)
                throw new ArgumentNullException(nameof(command));

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
            if (_disposed) return;
            _logger?.LogDebug(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected void LogWarning(string message)
        {
            if (_disposed) return;
            _logger?.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常对象</param>
        protected void LogError(string message, Exception exception = null)
        {
            if (_disposed) return;
            
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    lock (_lock)
                    {
                        LogDebug("开始释放CommunicationHandler资源");
                        // 可以在这里释放其他托管资源
                    }
                }

                // 释放非托管资源
                _disposed = true;
                LogDebug("CommunicationHandler资源已释放");
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
