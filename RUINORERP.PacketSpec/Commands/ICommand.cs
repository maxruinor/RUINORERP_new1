using System;
using System.Threading;
using System.Threading.Tasks;

using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Commands
{

    /// <summary>
    /// 统一命令接口 - 定义所有命令的基本契约
    /// </summary>
    public interface ICommand 
    {
        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        CommandPriority Priority { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        byte[] RequestDataByMessagePack { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        byte[] ResponseDataByMessagePack { get; set; }

        /// <summary>
        /// 验证命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 序列化命令数据
        /// </summary>
        /// <returns>序列化后的数据</returns>
        byte[] Serialize();

        /// <summary>
        /// 反序列化命令数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>反序列化是否成功</returns>
        bool Deserialize(byte[] data);

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的数据</returns>
        object GetSerializableData();
    }
}
