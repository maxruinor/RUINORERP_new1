using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Commands
{
    // 定义具体的业务命令
    [Command(0x5001, "GetUserData", CommandCategory.DataSync, Description = "获取用户数据命令")]
    public class GetUserDataCommand : BaseCommand
    {
        public override CommandId CommandIdentifier => new CommandId(CommandCategory.DataSync, 0x01);

        public long UserId { get; set; }
        public bool IncludeProfile { get; set; }

        public GetUserDataCommand(long userId, bool includeProfile = false)
        {
            UserId = userId;
            IncludeProfile = includeProfile;
            TimeoutMs = 10000; // 10秒超时
        }

        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 客户端通常只构建命令，不执行逻辑
            return CommandResult.Success(new { UserId, IncludeProfile });
        }

        protected override object GetSerializableData()
        {
            return new { UserId, IncludeProfile };
        }
    }
}
