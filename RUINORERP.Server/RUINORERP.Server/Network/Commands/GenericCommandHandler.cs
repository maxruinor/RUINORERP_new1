using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace RUINORERP.PacketSpec.Handlers
{
    /// <summary>
    /// 一条 Handler 吃所有“新体系”指令
    /// </summary>
    /// <typeparam name="TPayload">业务数据 POCO</typeparam>
    public sealed class GenericCommandHandler<TPayload> : BaseCommandHandler
    {
        // 构造时把“我要支持哪些命令码”传进来
        public GenericCommandHandler(IEnumerable<CommandId> supportedCommands)
        {
            //TODO  可能是只读，类转换时需要 调试
            //SupportedCommandsList = supportedCommands?.Select(c => c.FullCode).ToArray()
                                      //?? Array.Empty<uint>();
        }

        private readonly uint[] SupportedCommandsList;

        public override IReadOnlyList<uint> SupportedCommands => SupportedCommandsList;

        public override string Name => $"GenericHandler<{typeof(TPayload).Name}>";

        protected override Task<ApiResponse> OnHandleAsync(ICommand cmd, CancellationToken ct)
        {
            // 框架保证只会把 GenericCommand<TPayload> 派过来
            var payload = cmd.GetSerializableData();
            if (payload is not TPayload biz)
                return Task.FromResult(ApiResponse.CreateError(400).WithMetadata("ErrorCode", "INVALID_PAYLOAD_TYPE").WithMetadata("ErrorMessage", "Payload 类型不匹配"));

            // 这里写统一业务入口，也可以再分派到更细的逻辑
            return HandleCoreAsync(biz, ct);
        }

        private Task<ApiResponse> HandleCoreAsync(TPayload payload, CancellationToken ct)
        {
            // 示例：直接返回成功 + 原数据
            return Task.FromResult(ApiResponse.CreateSuccess(payload).WithMetadata("Message", $"[{typeof(TPayload).Name}] 处理成功"));
        }
    }
}

