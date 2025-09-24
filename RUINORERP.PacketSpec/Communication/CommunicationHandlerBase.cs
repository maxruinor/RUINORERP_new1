using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Communication
{
    /// <summary>
    /// 通信处理器基类
    /// 提供命令分发和结果处理的通用实现
    /// </summary>
    public abstract class CommunicationHandlerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<CommunicationHandlerBase> Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        protected CommunicationHandlerBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected async Task<CommandResult> HandleCommandAsync(ICommand command, CancellationToken cancellationToken = default)
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

                // 将CommandResult转换为CoreCommandResult
                var coreResult = new CommandResult
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    ErrorCode = result.ErrorCode,
                    Data = result.Data,
                    CommandId = result.CommandId
                };

                // 命令后置处理
                await OnAfterCommandAsync(command, coreResult, cancellationToken);

                LogDebug($"命令处理完成: {command.GetType().Name} [ID: {command.CommandId}], 结果: {(coreResult.IsSuccess ? "成功" : "失败")}");
                return coreResult;
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
        protected virtual Task<CommandValidationResult> ValidateCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            // 默认实现，子类可以重写
            return Task.FromResult(CommandValidationResult.Success());
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
        protected virtual void LogDebug(string message)
        {
            Logger?.LogDebug(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected virtual void LogWarning(string message)
        {
            Logger?.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常对象</param>
        protected virtual void LogError(string message, Exception exception = null)
        {
            if (exception == null)
            {
                Logger?.LogError(message);
            }
            else
            {
                Logger?.LogError(exception, message);
            }
        }
    }
}
