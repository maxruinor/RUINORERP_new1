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
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Errors;

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
        protected ResponseBase CreateFailureResult(ErrorCode errorCode, string message = null)
        {
            return ResponseFactory.Fail(errorCode, message);
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
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            logger?.LogWarning($"[{GetType().Name}] {message}");
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
                TimestampUtc = baseResponse.TimestampUtc,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }
}