﻿using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RUINORERP.PacketSpec.Commands
{
    [PacketCommand("GenericCmd", CommandCategory.System)]
    public class GenericCommand<TPayload> : BaseCommand
    {
        public TPayload Payload { get; set; }

        public GenericCommand(CommandId id, TPayload payload)
        {
            CommandIdentifier = id;
            Payload = payload;
            Direction = PacketDirection.ServerToClient;
        }

        public override object GetSerializableData() => Payload;

        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default) =>
            Payload == null
                ? new ValidationResult(new[] { new ValidationFailure(nameof(Payload), "Payload为空") })
                : new ValidationResult();

        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken _)
            => Task.FromResult((ResponseBase)ResponseBase.CreateSuccess("执行成功").WithMetadata("Data", Payload));

    }
}
