using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.PacketSpec.Commands;
using global::RUINORERP.PacketSpec.Protocol;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;
using System.Threading;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一命令处理器基类 - 提供标准化的命令处理模板
    /// </summary>
    public abstract class UnifiedCommandHandlerBase : BaseCommandHandler
    {

        protected ILogger<UnifiedCommandHandlerBase> logger { get; set; }
        
        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        protected UnifiedCommandHandlerBase() : base(new LoggerFactory().CreateLogger<BaseCommandHandler>())
        {
            logger = new LoggerFactory().CreateLogger<UnifiedCommandHandlerBase>();
        }

        public UnifiedCommandHandlerBase(ILogger<UnifiedCommandHandlerBase> _Logger) : base(_Logger)
        {
            logger = _Logger;
        }

        

        /// <summary>
        /// 执行核心处理逻辑（模板方法模式）
        /// </summary>
        protected override async Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"开始处理命令: {command.CommandIdentifier}");

                // 验证命令
                var validationResult = command.Validate();
                if (!validationResult.IsValid)
                {
                    return CommandResult.Failure(validationResult.ErrorMessage, "VALIDATION_FAILED");
                }

                // 执行具体处理
                var result = await ProcessCommandAsync(command, cancellationToken);

                LogInfo($"命令处理完成: {command.CommandIdentifier}, 结果: {result.IsSuccess}");
                return result;
            }
            catch (Exception ex)
            {
                LogError($"处理命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "PROCESS_ERROR", ex);
            }
        }

        /// <summary>
        /// 具体的命令处理逻辑（由子类实现）
        /// </summary>
        protected abstract Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken);

        /// <summary>
        /// 创建成功响应
        /// </summary>
        protected CommandResult CreateSuccessResult(object data = null, string message = "操作成功")
        {
            return CommandResult.Success(data, message);
        }

        /// <summary>
        /// 创建带响应数据的成功结果
        /// </summary>
        protected CommandResult CreateSuccessWithResponse(OriginalData responseData, object data = null, string message = "操作成功")
        {
            return CommandResult.SuccessWithResponse(responseData, data, message);
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        protected CommandResult CreateFailureResult(string message, string errorCode = "OPERATION_FAILED")
        {
            return CommandResult.Failure(message, errorCode);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            logger?.LogInformation($"[{GetType().Name}] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            logger?.LogError(ex, $"[{GetType().Name}] {message}");
        }

        /// <summary>
        /// 从原始数据解析对象
        /// </summary>
        protected T ParseData<T>(OriginalData originalData) where T : class
        {
            if (originalData.One == null || originalData.One.Length == 0)
                return null;

            try
            {
                var json = System.Text.Encoding.UTF8.GetString(originalData.One);
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                LogError($"解析数据失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建响应数据
        /// </summary>
        protected OriginalData CreateResponseData(uint command, object data)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(data);
                var dataBytes = System.Text.Encoding.UTF8.GetBytes(json);
                
                // 将完整的CommandId正确分解为Category和OperationCode
                byte category = (byte)(command & 0xFF); // 取低8位作为Category
                byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
                
                return new OriginalData(category, new byte[] { operationCode }, dataBytes);
            }
            catch (Exception ex)
            {
                LogError($"创建响应数据失败: {ex.Message}", ex);
                
                // 将完整的CommandId正确分解为Category和OperationCode
                byte category = (byte)(command & 0xFF); // 取低8位作为Category
                byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
                
                return new OriginalData(category, new byte[] { operationCode }, null);
            }
        }
    }
}

