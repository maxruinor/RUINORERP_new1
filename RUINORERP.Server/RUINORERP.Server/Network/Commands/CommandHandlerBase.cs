using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.PacketSpec.Commands;
using global::RUINORERP.PacketSpec.Protocol;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using Newtonsoft.Json;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一命令处理器基类 - 提供标准化的命令处理模板
    /// </summary>
    public abstract class CommandHandlerBase : BaseCommandHandler
    {

        protected ILogger<CommandHandlerBase> logger { get; set; }
        
        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        protected CommandHandlerBase() : base(new LoggerFactory().CreateLogger<BaseCommandHandler>())
        {
            logger = new LoggerFactory().CreateLogger<CommandHandlerBase>();
        }

        public CommandHandlerBase(ILogger<CommandHandlerBase> _Logger) : base(_Logger)
        {
            logger = _Logger;
        }

        

        /// <summary>
        /// 执行核心处理逻辑（模板方法模式）
        /// </summary>
        protected override async Task<ResponseBase> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 验证输入参数
                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }
                
                LogInfo(command, $"开始处理命令: {command.CommandIdentifier}");

                // 验证命令
                var validationResult = command.Validate();
                if (!validationResult.IsValid)
                {
                    // 使用ResponseFactory统一处理验证失败
                    return ResponseFactory.Fail(
                        RUINORERP.PacketSpec.Errors.ErrorCode.Command_ValidateFail, 
                        validationResult.ErrorMessage);
                }

                // 执行具体处理
                var result = await ProcessCommandAsync(command, cancellationToken);

                LogInfo(command, $"命令处理完成: {command.CommandIdentifier}, 结果: {result.IsSuccess}");
                return result;
            }
            catch (OperationCanceledException ex)
            {
                LogError($"命令处理被取消: {ex.Message}", ex);
                // 使用ResponseFactory统一处理取消异常
                return ResponseFactory.Except(ex, RUINORERP.PacketSpec.Errors.ErrorCode.Command_Timeout);
            }
            catch (Exception ex)
            {
                LogError($"处理命令异常: {ex.Message}", ex);
                // 使用ResponseFactory统一处理其他异常
                return ResponseFactory.Except(ex);
            }
        }

        /// <summary>
        /// 具体的命令处理逻辑（由子类实现）
        /// </summary>
        [MustOverride]
        protected abstract Task<ResponseBase> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken);

        /// <summary>
        /// 创建成功响应
        /// </summary>
        protected ResponseBase CreateSuccessResult(object data = null, string message = "操作成功")
        {
            if (data != null)
            {
                return ResponseFactory.Ok(data, message);
            }
            else
            {
                return ResponseFactory.Ok(message);
            }
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        protected ResponseBase CreateFailureResult(string message, string errorCode = "OPERATION_FAILED")
        {
            // 尝试将错误代码转换为枚举
            if (Enum.TryParse<RUINORERP.PacketSpec.Errors.ErrorCode>(errorCode, out var parsedErrorCode))
            {
                return ResponseFactory.Fail(parsedErrorCode, message);
            }
            else
            {
                // 如果无法解析，则使用通用错误代码
                return ResponseFactory.Fail(RUINORERP.PacketSpec.Errors.ErrorCode.System_Error, message)
                    .WithMetadata("OriginalErrorCode", errorCode);
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            logger?.LogInformation($"[{GetType().Name}] {message}");
        }
        
        /// <summary>
        /// 记录信息日志（带RequestId）
        /// </summary>
        protected void LogInfo(ICommand command, string message)
        {
            var requestId = "N/A";
            // 尝试从BaseCommand获取RequestId
            if (command is RUINORERP.PacketSpec.Commands.BaseCommand baseCommand && !string.IsNullOrEmpty(baseCommand.RequestId))
            {
                requestId = baseCommand.RequestId;
            }
            // 如果没有RequestId，尝试从Packet的Extensions中获取
            else if (command?.Packet?.Extensions != null && 
                     command.Packet.Extensions.TryGetValue("RequestId", out var requestIdObj) && 
                     requestIdObj is string rid && !string.IsNullOrEmpty(rid))
            {
                requestId = rid;
            }
            
            logger?.LogInformation("[{RequestId}] {Message}", requestId, $"[{GetType().Name}] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            logger?.LogError(ex, $"[{GetType().Name}] {message}");
        }

        /// <summary>
        /// 从原始数据解析对象 - 使用系统默认的JSON序列化器
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
        /// 使用GetJsonData方法从PacketModel中解析业务数据
        /// 推荐使用此方法以确保数据解析的统一性
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <returns>解析后的数据对象，如果解析失败则返回null</returns>
        protected T ParseBusinessData<T>(ICommand command) where T : class
        {
            try
            {
                // 检查命令是否包含原始数据
                if (command == null || command == null)
                {
                    LogWarning("命令或原始数据为空，无法解析业务数据");
                    return null;
                }
                //TODO 这里要这样吗

                // 创建PacketModel并填充数据
                var packetModel = new RUINORERP.PacketSpec.Models.Core.PacketModel
                {
                    Command = command.CommandIdentifier,
                    SessionId = command.SessionID,
                    Body = command.Packet.Body
                };

                // 使用GetJsonData方法解析业务数据
                return packetModel.GetJsonData<T>();
            }
            catch (Exception ex)
            {
                LogError($"解析业务数据失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            logger?.LogWarning($"[{GetType().Name}] {message}");
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

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        protected ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }
}