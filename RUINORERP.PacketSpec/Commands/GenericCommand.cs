﻿﻿using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    [PacketCommand("GenericCmd", CommandCategory.System)]
    public class GenericCommand<TPayload> : BaseCommand
    {
        public override CommandId CommandIdentifier { get; }   //由外部注入
        public TPayload Payload { get; set; }

        public GenericCommand(CommandId id, TPayload payload)
        {
            CommandIdentifier = id;
            Payload = payload;
            Direction = PacketDirection.ServerToClient;
        }

        public override object GetSerializableData() => Payload;

        public override CommandValidationResult Validate() =>
            Payload == null
                ? CommandValidationResult.Failure("Payload为空")
                : CommandValidationResult.Success();

        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken _)
            => Task.FromResult((ResponseBase)ResponseBase.CreateSuccess("执行成功").WithMetadata("Data", Payload));

    }
}
