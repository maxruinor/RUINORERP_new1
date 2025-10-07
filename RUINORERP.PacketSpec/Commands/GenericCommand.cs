﻿using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Commands
{
    [PacketCommand("GenericCmd", CommandCategory.System)]
    public class GenericCommand<TPayload> : BaseCommand
    {
        public TPayload Payload { get; set; }
        
        public GenericCommand() : base()
        {
        }
        
        public GenericCommand(CommandId id, TPayload payload) : base()
        {
            CommandIdentifier = id;
            Payload = payload;
            Direction = PacketDirection.ClientToServer;
        }
        
        public GenericCommand<TRequest, TResponse> AsTypedCommand<TRequest, TResponse>()
            where TRequest : class, IRequest
            where TResponse : class, IResponse
        {
            if (Payload is TRequest request)
            {
                return new GenericCommand<TRequest, TResponse>(CommandIdentifier, request)
                {
                    TimeoutMs = this.TimeoutMs,
                    Priority = this.Priority,
                    Direction = this.Direction
                };
            }
            throw new InvalidCastException($"Payload无法转换为{typeof(TRequest).Name}");
        }
        
        public override object GetSerializableData() => Payload;
        
       
    }
    
    // 强类型泛型命令
    [PacketCommand("GenericTypedCmd", CommandCategory.System)]
    public class GenericCommand<TRequest, TResponse> : BaseCommand<TRequest, TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        public GenericCommand() : base()
        {
        }
        
        public GenericCommand(CommandId id, TRequest request) : base()
        {
            CommandIdentifier = id;
            Request = request;
            Direction = PacketDirection.ClientToServer;
        }
        
      
    }
}
