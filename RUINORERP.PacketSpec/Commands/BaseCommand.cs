using System;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RUINORERP.PacketSpec.Handlers;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;
using System.Security.Cryptography;
using MessagePack;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Commands.Authentication;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Errors;

namespace RUINORERP.PacketSpec.Commands
{
    [MessagePackObject(AllowPrivate = true)]
    public class BaseCommand<TRequest, TResponse> : BaseCommand where TRequest : class, IRequest where TResponse : class, IResponse
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseCommand() : base()
        {
        }

        /// <summary>
        /// 构造函数 - 支持依赖注入
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="logger">日志记录器</param>
        public BaseCommand(IRequest request = null, ILogger<BaseCommand> logger = null)
            : base(logger)
        {
            if (request != null)
                Request = request as TRequest;
        }

        /// <summary>
        /// 构造函数 - 用于快速创建命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        public BaseCommand(CommandId commandId, TRequest request)
            : base()
        {
            CommandIdentifier = commandId;
            Request = request;
        }

        [IgnoreMember]
        private CommandDataContainer<TRequest> _requestContainer;

        [IgnoreMember]
        private CommandDataContainer<TResponse> _responseContainer;

        [Key(50)]
        public TRequest Request
        {
            get => _requestContainer?.ObjectData;
            set
            {
                if (_requestContainer == null)
                    _requestContainer = new CommandDataContainer<TRequest>();
                _requestContainer.ObjectData = value;
            }
        }

        [Key(51)]
        public TResponse Response
        {
            get => _responseContainer?.ObjectData;
            set
            {
                if (_responseContainer == null)
                    _responseContainer = new CommandDataContainer<TResponse>();
                _responseContainer.ObjectData = value;
            }
        }

        // 重写基类方法，提供智能数据访问
        protected override object GetSerializableDataCore()
        {
            // 方向已由 PacketModel 统一控制，此处不再依赖 Direction
            // 始终返回 Request 作为待序列化数据
            return Request;
        }

        // 新增高效二进制访问方法
        public byte[] GetRequestBinary() => _requestContainer?.BinaryData;
        public byte[] GetResponseBinary() => _responseContainer?.BinaryData;

        // 从二进制数据初始化请求,有被使用放射，用字符搜索
        public void SetRequestFromBinary(byte[] data)
        {
            if (_requestContainer == null)
                _requestContainer = new CommandDataContainer<TRequest>();
            _requestContainer.BinaryData = data;
        }

        /// <summary>
        /// 清空请求数据并设置响应数据 - 用于服务器处理完请求后返回响应
        /// 这样可以减少网络传输的数据量
        /// </summary>
        /// <param name="responseData">响应数据对象</param>
        public void ClearRequestAndSetResponse(TResponse responseData)
        {
            // 清空请求数据
            if (_requestContainer != null)
            {
                _requestContainer.ObjectData = null;
                // 如果有二进制数据，也清空
                if (_requestContainer.BinaryData != null)
                {
                    Array.Clear(_requestContainer.BinaryData, 0, _requestContainer.BinaryData.Length);
                    _requestContainer.BinaryData = null;
                }
            }
            
            // 设置响应数据
            Response = responseData;
        }

        /// <summary>
        /// 清空请求数据并设置响应数据 - 二进制版本
        /// </summary>
        /// <param name="responseData">响应数据字节数组</param>
        public void ClearRequestAndSetResponseFromBinary(byte[] responseData)
        {
            // 清空请求数据
            if (_requestContainer != null)
            {
                _requestContainer.ObjectData = null;
                // 如果有二进制数据，也清空
                if (_requestContainer.BinaryData != null)
                {
                    Array.Clear(_requestContainer.BinaryData, 0, _requestContainer.BinaryData.Length);
                    _requestContainer.BinaryData = null;
                }
            }
            
            // 设置响应数据
            if (_responseContainer == null)
                _responseContainer = new CommandDataContainer<TResponse>();
            _responseContainer.BinaryData = responseData;
        }
    }

    /// <summary>
    /// 命令基类 - 提供命令的通用实现
    /// </summary>
    public abstract class BaseCommand : ICommand
    {

        public BaseCommand()
        {
        }


        protected virtual object GetSerializableDataCore() { return null; }
        // 新增智能访问方法
        public byte[] GetBinaryData()
        {
            var data = GetSerializableDataCore();
            return data != null ? UnifiedSerializationService.SerializeWithMessagePack(data) : Array.Empty<byte>();
        }
        public T GetObjectData<T>() where T : class
        {
            var data = GetSerializableDataCore();
            return data as T;
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        [IgnoreMember]
        protected ILogger<BaseCommand> Logger { get; set; }

        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        [Key(30)]
        public CommandId CommandIdentifier { get; set; }

        // 命令方向已移除 - 简化设计

        /// <summary>
        /// 命令优先级
        /// </summary>
        [Key(32)]
        public CommandPriority Priority { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand(ILogger<BaseCommand> logger = null)
        {
            Priority = CommandPriority.Normal;
            Logger = logger ?? NullLogger<BaseCommand>.Instance;
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        public virtual async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 使用验证服务进行验证
            var validationService = ValidationService.Instance;
            var result = await validationService.ValidateAsync(this);
            return result;
        }

        /// <summary>
        /// 序列化命令数据
        /// </summary>
        public virtual byte[] Serialize()
        {
            try
            {
                // 使用UnifiedSerializationService进行序列化
                return UnifiedSerializationService.SerializeWithMessagePack(this);
            }
            catch (Exception ex)
            {
                LogError($"序列化命令失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 反序列化命令数据
        /// </summary>
        public virtual bool Deserialize(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    return false;

                // 使用UnifiedSerializationService进行反序列化
                var deserializedCommand = UnifiedSerializationService.DeserializeWithMessagePack<BaseCommand>(data);

                // 恢复基本属性
                if (deserializedCommand != null)
                {
                    CommandIdentifier = deserializedCommand.CommandIdentifier;
                    Priority = deserializedCommand.Priority;

                    // 恢复自定义数据
                    return DeserializeCustomData(deserializedCommand.GetSerializableData());
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"反序列化命令失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 使用MessagePack反序列化命令数据
        /// </summary>
        public virtual bool DeserializeWithMessagePack(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    return false;

                // 使用UnifiedSerializationService进行MessagePack反序列化
                var deserializedCommand = UnifiedSerializationService.DeserializeWithMessagePack<BaseCommand>(data);

                // 恢复基本属性
                if (deserializedCommand != null)
                {
                    CommandIdentifier = deserializedCommand.CommandIdentifier;
                    Priority = deserializedCommand.Priority;

                    // 恢复自定义数据
                    return DeserializeCustomData(deserializedCommand.GetSerializableData());
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"MessagePack反序列化命令失败: {ex.Message}", ex);
                return false;
            }
        }


        #region 虚方法 - 子类可以重写

        /// <summary>
        /// 是否需要会话信息
        /// </summary>
        protected virtual bool RequiresSession()
        {
            return false;
        }

        /// <summary>
        /// 是否需要数据包
        /// </summary>
        protected virtual bool RequiresData()
        {
            return false;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        public virtual object GetSerializableData()
        {
            return null;
        }

        /// <summary>
        /// 包体数据（业务请求响应数据序列化后的字节）
        /// </summary>
        [Key(40)]
        public byte[] RequestDataByMessagePack { get; set; }

        /// <summary>
        /// 响应数据（业务响应数据序列化后的字节）
        /// </summary>
        [Key(41)]
        public byte[] ResponseDataByMessagePack { get; set; }

        /// <summary>
        /// 获取强类型的数据载荷
        /// </summary>
        /// <typeparam name="T">数据载荷类型</typeparam>
        /// <returns>指定类型的载荷数据</returns>
        public virtual T GetPayload<T>()
        {
            return (T)GetSerializableData();
        }

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        public void SetRequestData<T>(T data)
        {
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);
            SetRequestData(jsonBytes);
        }

        #region 核心方法

        /// <summary>
        /// 设置数据内容
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public void SetRequestData(byte[] data)
        {
            RequestDataByMessagePack = data;
        }

        /// <summary>
        /// 设置响应数据
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public void SetResponseData(byte[] data)
        {
            ResponseDataByMessagePack = data;
        }

        /// <summary>
        /// 清空请求数据并设置响应数据 - 用于服务器处理完请求后返回响应
        /// 这样可以减少网络传输的数据量
        /// </summary>
        /// <param name="responseData">响应数据字节数组</param>
        public void ClearRequestAndSetResponse(byte[] responseData)
        {
            // 清空请求数据
            if (RequestDataByMessagePack != null)
            {
                Array.Clear(RequestDataByMessagePack, 0, RequestDataByMessagePack.Length);
                RequestDataByMessagePack = null;
            }
            
            // 设置响应数据
            ResponseDataByMessagePack = responseData;
        }

        /// <summary>
        /// 清空请求数据并设置响应数据 - 强类型版本
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="responseData">响应数据对象</param>
        public void ClearRequestAndSetResponse<T>(T responseData) where T : class
        {
            // 序列化响应数据
            var responseBytes = UnifiedSerializationService.SerializeWithMessagePack(responseData);
            
            // 调用字节数组版本
            ClearRequestAndSetResponse(responseBytes);
        }
        /// <summary>
        /// JSON序列化缓存，用于缓存高频序列化操作（如心跳包）
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> _jsonCache = new ConcurrentDictionary<string, byte[]>();

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>当前实例</returns>
        public void SetJsonBizData<T>(T data)
        {
            // 生成缓存键：类型名 + 数据哈希
            var type = typeof(T);
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);

            // 计算JSON的哈希值作为缓存键的一部分
            string hash;
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(jsonBytes);
                hash = Convert.ToBase64String(hashBytes);
            }

            var cacheKey = $"{type.FullName}:{hash}";
            // 尝试从缓存获取或添加
            RequestDataByMessagePack = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            // Extensions?.Clear();
            // 清理包体数据
            if (RequestDataByMessagePack != null)
            {
                Array.Clear(RequestDataByMessagePack, 0, RequestDataByMessagePack.Length);
                RequestDataByMessagePack = null;
            }

            // 清理响应数据
            if (ResponseDataByMessagePack != null)
            {
                Array.Clear(ResponseDataByMessagePack, 0, ResponseDataByMessagePack.Length);
                ResponseDataByMessagePack = null;
            }
        }

        #endregion

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据对象</returns>
        public T GetJsonData<T>()
        {
            var json = GetDataAsText();
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>文本数据</returns>
        public string GetDataAsText(Encoding encoding = null)
        {
            if (RequestDataByMessagePack == null || RequestDataByMessagePack.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(RequestDataByMessagePack);
        }

        /// <summary>
        /// 设置响应JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">响应数据对象</param>
        public void SetJsonResponseData<T>(T data)
        {
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);
            ResponseDataByMessagePack = jsonBytes;
        }

        /// <summary>
        /// 获取响应JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>响应数据对象</returns>
        public T GetJsonResponseData<T>()
        {
            if (ResponseDataByMessagePack == null || ResponseDataByMessagePack.Length == 0)
                return default(T);

            var json = Encoding.UTF8.GetString(ResponseDataByMessagePack);
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取响应数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>响应文本数据</returns>
        public string GetResponseDataAsText(Encoding encoding = null)
        {
            if (ResponseDataByMessagePack == null || ResponseDataByMessagePack.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(ResponseDataByMessagePack);
        }

        /// <summary>
        /// 反序列化自定义数据
        /// </summary>
        protected virtual bool DeserializeCustomData(dynamic data)
        {
            return true;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 记录调试日志
        /// </summary>
        protected void LogDebug(string message)
        {
            Logger.LogDebug(message);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            Logger.LogInformation(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                Logger.LogError(ex, message);
            }
            else
            {
                Logger.LogError(message);
            }
        }

        /// <summary>
        /// 创建响应数据包
        /// </summary>
        /// <param name="responseCommand">完整的响应命令ID</param>
        /// <param name="data1">第一部分数据</param>
        /// <param name="data2">第二部分数据</param>
        /// <returns>原始数据包</returns>
        protected OriginalData CreateResponseData(uint responseCommand, byte[] data1 = null, byte[] data2 = null)
        {
            // 将uint类型的命令ID转换为字节数组
            byte[] commandBytes = BitConverter.GetBytes(responseCommand);

            // 构造OriginalData: Cmd使用命令ID的低8位(Category)，One使用命令ID的次低8位(OperationCode)
            byte cmd = commandBytes[0]; // 命令类别
            byte[] one = commandBytes.Length > 1 ? new byte[] { commandBytes[1] } : Array.Empty<byte>(); // 操作码

            return new OriginalData(cmd, one, data2);
        }

 
 

        #endregion
    }

    /// <summary>
    /// 统一的命令响应包装类 - 服务器端响应构建器
    /// 提供标准化的响应格式，包含指令信息和业务响应数据
    /// </summary>
    /// <typeparam name="TResponse">响应数据类型</typeparam>
    [Serializable]
    [MessagePackObject(AllowPrivate = true)]
    public class BaseCommand<TResponse> : BaseCommand where TResponse : class, IResponse
    {

        /// <summary>
        /// 业务响应数据
        /// </summary>
        [Key(3)]
        public TResponse ResponseData { get; set; }

        /// <summary>
        /// 命令标识符
        /// </summary>
        [Key(4)]
        public CommandId CommandId { get; set; }

        /// <summary>
        /// 元数据字典 - 用于存储额外的响应信息
        /// </summary>
        [Key(9)]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public BaseCommand()
        {
        }

        /// <summary>
        /// 构造函数 - 创建成功响应
        /// </summary>
        /// <param name="responseData">响应数据</param>
        /// <param name="message">成功消息</param>
        public BaseCommand(TResponse responseData, string message = "操作成功")
        {
            ResponseData = responseData;
            responseData.IsSuccess = true;
            responseData.Message = message;
        }

        /// <summary>
        /// 构造函数 - 创建错误响应
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        public BaseCommand(string errorMessage, int Code = 400)
        {
  
            // 如果TResponse是ResponseBase类型，创建错误响应
            if (typeof(TResponse) == typeof(ResponseBase) || typeof(TResponse).IsSubclassOf(typeof(ResponseBase)))
            {
                var errorResponse = Activator.CreateInstance(typeof(TResponse)) as ResponseBase;
                if (errorResponse != null)
                {
                    errorResponse.Message = errorMessage;
                    errorResponse.IsSuccess = false;
                    errorResponse.ErrorCode = Code;
                    ResponseData = errorResponse as TResponse;
                }
            }
        }

        public BaseCommand(ErrorCode errorCode)
        {
        
            // 如果TResponse是ResponseBase类型，创建错误响应
            if (typeof(TResponse) == typeof(ResponseBase) || typeof(TResponse).IsSubclassOf(typeof(ResponseBase)))
            {
                var errorResponse = Activator.CreateInstance(typeof(TResponse)) as ResponseBase;
                if (errorResponse != null)
                {
                    errorResponse.Message = errorCode.Message;
                    errorResponse.ErrorCode = errorCode.Code;
                    errorResponse.IsSuccess = false;
                    ResponseData = errorResponse as TResponse;
                }
            }
        }

        /// <summary>
        /// 静态方法 - 创建成功响应
        /// </summary>
        /// <param name="responseData">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应</returns>
        public static BaseCommand<TResponse> Success(TResponse responseData, string message = "操作成功")
        {
            return new BaseCommand<TResponse>(responseData, message);
        }

        /// <summary>
        /// 静态方法 - 创建成功响应（兼容ResponseBase.CreateSuccess）
        /// </summary>
        /// <param name="responseData">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应</returns>
        public static BaseCommand<TResponse> CreateSuccess(TResponse responseData, string message = "操作成功")
        {
            return Success(responseData, message);
        }

        /// <summary>
        /// 静态方法 - 创建错误响应
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误响应</returns>
        public static BaseCommand<TResponse> Error(string errorMessage)
        {
            return new BaseCommand<TResponse>(errorMessage);
        }
        public static BaseCommand<TResponse> Error(string errorMessage, int errorCode)
        {
            return new BaseCommand<TResponse>(errorMessage, errorCode);
        }
        /// <summary>
        /// 静态方法 - 创建错误响应（兼容ResponseBase.CreateError）
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误响应</returns>
        public static BaseCommand<TResponse> CreateError(string errorMessage)
        {
            return Error(errorMessage);
        }

        public static BaseCommand<TResponse> CreateError(string errorMessage, int ErrorCode)
        {
            return Error(errorMessage, ErrorCode);
        }

        public static BaseCommand<TResponse> CreateError(ErrorCode error)
        {
            return Error(error);
        }

        /// <summary>
        /// 从FluentValidation验证结果创建失败响应
        /// </summary>
        /// <param name="validationResult">FluentValidation验证结果</param>
        /// <param name="code">错误代码</param>
        /// <returns>验证错误响应</returns>
        public static BaseCommand<TResponse> CreateValidationError(FluentValidation.Results.ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
                return CreateError("验证失败");

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var message = string.Join("; ", errorMessages);

            var errorResponse = CreateError(message);

            // 添加详细的验证错误信息到元数据
            errorResponse.WithMetadata("ValidationErrors", validationResult.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage,
                AttemptedValue = e.AttemptedValue
            }).ToList());

            return errorResponse;
        }

        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        /// <summary>
        /// 获取元数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>元数据值</returns>
        public T GetMetadata<T>(string key)
        {
            if (Metadata.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return default(T);
        }

        /// <summary>
        /// 添加元数据（链式调用）
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public BaseCommand<TResponse> WithMetadata(string key, object value)
        {
            Metadata[key] = value;
            return this;
        }

        /// <summary>
        /// 批量添加元数据（链式调用）
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public BaseCommand<TResponse> WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var item in metadata)
            {
                Metadata[item.Key] = item.Value;
            }
            return this;
        }




        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"BaseCommand<TResponse>:  CommandName={CommandId.Name}, CommandId={CommandId}";
        }
    }
}
