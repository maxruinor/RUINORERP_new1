using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Commands.Authentication;
using System.Net.Sockets;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 统一数据包模型 - 核心通信协议实体
    /// 
    /// 网络传输层 - 只关心网络协议格式
    /// 职责：包头、加密、压缩、序列化等网络传输相关属性
    /// 不包含任何业务逻辑或业务属性
    /// </summary>
    [JsonObject]
    public class PacketModel : ITimestamped
    {

        // 简单的命令标识（不包含业务逻辑）
        //命令类型
        public CommandId CommandId { get; set; }

        /// <summary>
        /// 数据包状态
        /// </summary>
        public PacketStatus Status { get; set; }

        #region 网络传输属性

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        // 网络标识
        public string PacketId { get; set; }



        /// <summary>
        /// 数据包方向
        /// </summary>
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 扩展属性字典（用于存储非核心但需要传输的元数据）
        /// </summary>
        public Dictionary<string, object> Extensions { get; set; }

        /// <summary>
        /// 命令执行上下文 - 网络传输层使用
        /// 包含会话、认证、追踪等基础设施信息
        /// </summary>
        public CommandContext ExecutionContext { get; set; }

        /// <summary>
        /// 会话ID（通过ExecutionContext获取）
        /// </summary>
        public string SessionId
        {
            get => ExecutionContext?.SessionId;
            set
            {
                if (ExecutionContext == null)
                    ExecutionContext = new CommandContext();
                ExecutionContext.SessionId = value;
            }
        }
    

        #endregion

        /// <summary>
        /// 创建数据包的便捷方法 - 不再依赖ICommand接口
        /// </summary>
        /// <typeparam name="TData">数据对象类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">数据对象</param>
        /// <returns>创建的数据包</returns>
        public static PacketModel Create<TData>(CommandId commandId, TData data)
        {
            if (commandId.OperationCode == 0)
                throw new ArgumentNullException(nameof(commandId), "命令标识符不能为空");

            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(commandId.Name),
                CommandId = commandId,
            };
        }

        /// <summary>
        /// 使用请求对象创建数据包
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求对象</param>
        /// <returns>创建的数据包</returns>
        public static PacketModel CreateFromRequest<TRequest>(CommandId commandId, TRequest request) where TRequest : RequestBase
        {
            var packet = Create(commandId, request);
            packet.Request = request;
            packet.Direction = PacketDirection.ClientToServer;
            return packet;
        }

        #region ITimestamped 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// 记录对象的当前状态时间点，会随着对象状态变化而更新
        /// </summary>
        public DateTime TimestampUtc { get; set; }


        /// <summary>
        /// 请求模型
        /// </summary>
        public IRequest Request { get; set; }


        /// <summary>
        /// 响应模型
        /// </summary>
        public IResponse Response { get; set; }

        public PacketPriority PacketPriority { get; set; }
        /// <summary>
        /// 验证模型有效性（实现 ICoreEntity 接口）
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            // 添加1分钟的时间容差范围，允许客户端和服务器之间有一定的时间差异
            var now = DateTime.Now;
            var toleranceMinutes = 1; // 1分钟容差
            
            return CreatedTime <= now.AddMinutes(toleranceMinutes) && // 允许客户端时间最多比服务器快1分钟
                   CreatedTime >= now.AddYears(-1) && 
                   !string.IsNullOrEmpty(PacketId);
        }

        /// <summary>
        /// 更新时间戳（实现 ITimestamped 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PacketModel()
        {
            PacketId = Guid.NewGuid().ToString();
            CreatedTime = DateTime.Now;
            TimestampUtc = DateTime.UtcNow;
            Extensions = new Dictionary<string, object>();
            // 初始化默认扩展属性
            SetExtension("Version", "2.0");
            // 移除CommandData的强制初始化，允许外部设置数据
            // CommandData = Array.Empty<byte>();
        }




        #endregion

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {

            // 清理ExecutionContext中的敏感数据
            if (ExecutionContext != null)
            {
                ExecutionContext.SessionId = null;
                if (ExecutionContext.Token != null)
                {
                    ExecutionContext.Token.AccessToken = null;
                }
            }

            Extensions?.Clear();
            // 清理包体数据
            if (Response != null)
            {
                Response = null;
            }
            if (Request != null)
            {
                Request = null;
            }
        }




        #region 扩展数据存储方法

        /// <summary>
        /// 获取扩展属性值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">属性键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性值或默认值</returns>
        public T GetExtension<T>(string key, T defaultValue = default)
        {
            return Extensions.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }

        /// <summary>
        /// 设置扩展属性值
        /// </summary>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        public void SetExtension(string key, object value)
        {
            Extensions[key] = value;
        }

        /// <summary>
        /// 获取包标志位
        /// </summary>
        public string Flag
        {
            get => GetExtension<string>("Flag");
            set => SetExtension("Flag", value);
        }

        /// <summary>
        /// 获取模型版本
        /// </summary>
        public string Version
        {
            get => GetExtension<string>("Version");
            set => SetExtension("Version", value);
        }


        #endregion

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Direction:{Direction}, Flag:{Flag}, Version:{Version}]";
        }








        #endregion


        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(CommandId.Category.ToString()),
                CommandId = CommandId,
                Status = Status,
                SessionId = SessionId,
                CreatedTime = CreatedTime,
                Extensions = new System.Collections.Generic.Dictionary<string, object>(Extensions),
                TimestampUtc = TimestampUtc
            };
        }

        /// <summary>
        /// 计算数据包的哈希值
        /// </summary>
        /// <returns>数据包的哈希值</returns>
        private string ComputeHash()
        {
            // 这里实现具体的哈希计算逻辑
            // 例如使用SHA256或其他哈希算法
            // 为简化起见，这里返回一个占位符
            return string.Empty; // 实际实现中应该计算CommandData的哈希值
        }
    }

    /// <summary>
    /// PacketModel的扩展方法类
    /// </summary>
    public static class PacketModelExtensions
    {
        /// <summary>
        /// 从数据包中提取命令ID
        /// 供服务端Pipeline解码时直接拿到CommandId，无需再new CommandId(...)重复拼装
        /// </summary>
        /// <param name="packet">数据包模型实例</param>
        /// <returns>命令ID对象</returns>
        public static CommandId ExtractCommandId(this PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet), "数据包不能为空");

            // 直接返回已有的Command属性，避免重新创建CommandId对象
            return packet.CommandId;
        }
    }
}



